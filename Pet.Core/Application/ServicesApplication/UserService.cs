using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Pet.Core.Application.DTOs;
using Pet.Core.Application.Infrastructure;
using Pet.Core.Contracts.Response;
using Pet.Core.Domain.Entities;
using Pet.Core.INFRASTRUCTURE.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Pet.Core.Application.ServicesApplication
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly ApplicationDBContext _dbContext;
        private readonly IPhotoUploadService _photoUploadService;
        private readonly IMapper _mapper;

        public UserService(
            UserManager<AppUser> userManager,
            RoleManager<AppRole> roleManager,
            ApplicationDBContext dbContext,
            IPhotoUploadService photoUploadService,
            IMapper mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _dbContext = dbContext;
            _photoUploadService = photoUploadService;
            _mapper = mapper;
        }

        public async Task<LoggedInUser> GetUserByID(int userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null || user.IsDeleted)
                throw new Exception("User not found or deleted.");

            var dto = _mapper.Map<LoggedInUser>(user);
            return dto;
        }

        public async Task<PagedResponse<AppUserDto>> GetAllInRolePaging(string role, int pageNumber, int pageSize, string searchTerm)
        {
            var query = _userManager.Users.Where(u => !u.IsDeleted);

            if (!string.IsNullOrEmpty(role))
            {
                var allUsers = await _userManager.Users.ToListAsync();
                var userIdsInRole = (await Task.WhenAll(allUsers.Select(async u => new
                {
                    User = u,
                    Roles = await _userManager.GetRolesAsync(u)
                })))
                .Where(x => x.Roles.Contains(role))
                .Select(x => x.User.Id)
                .ToHashSet();

                query = query.Where(u => userIdsInRole.Contains(u.Id));
            }

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(u =>
                    u.UserName.Contains(searchTerm) ||
                    u.Email.Contains(searchTerm) ||
                    u.FullName.Contains(searchTerm));
            }

            var total = await query.CountAsync();
            var users = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            var userDtos = _mapper.Map<List<AppUserDto>>(users);

            return new PagedResponse<AppUserDto>
            {
                Items = userDtos,
                TotalCount = total,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<PagedResponse<AppUserDto>> GetAllPaging(int pageNumber, int pageSize, string searchTerm)
        {
            return await GetAllInRolePaging(null, pageNumber, pageSize, searchTerm);
        }

        public async Task<List<string>> GetRolesOfUser(int userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null || user.IsDeleted)
                throw new Exception("User not found or deleted.");

            return (await _userManager.GetRolesAsync(user)).ToList();
        }

        public async Task AssignRoleToUser(int userId, List<string> roles)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null || user.IsDeleted)
                throw new Exception("User not found or deleted.");

            var currentRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, currentRoles);
            await _userManager.AddToRolesAsync(user, roles);
        }

        public async Task EditUser(int userId, string fullName, string email, DateTime? dateOfBirth, string? newPassword = null)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null || user.IsDeleted)
                throw new Exception("User not found or deleted.");

            user.FullName = fullName;
            user.Email = email;
            if (dateOfBirth.HasValue)
                user.DateOfBirth = dateOfBirth.Value;

            if (!string.IsNullOrEmpty(newPassword))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                await _userManager.ResetPasswordAsync(user, token, newPassword);
            }

            await _userManager.UpdateAsync(user);
        }

        public async Task DeleteUser(int userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null || user.IsDeleted)
                throw new Exception("User not found or already deleted.");

            user.IsDeleted = true;
            user.UserName = $"{user.UserName}_Deleted_{DateTime.UtcNow.Ticks}";
            user.Email = $"{user.Email}_Deleted_{DateTime.UtcNow.Ticks}";
            await _userManager.UpdateAsync(user);
        }

        public async Task DeleteUserPermanently(int userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                throw new Exception("User not found.");

            await _userManager.DeleteAsync(user);
        }

        public async Task ChangePhotoAsync(int userId, Stream fileStream, string fileName)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null || user.IsDeleted)
                throw new Exception("User not found or deleted.");

            // Convert Stream to IFormFile
            IFormFile formFile = new FormFile(fileStream, 0, fileStream.Length, "file", fileName);

            var (photoPath, photoUrl) = await _photoUploadService.UploadPhotoAsync(formFile, "uploads", "photos");

            if (!string.IsNullOrEmpty(user.PhotoPath))
            {
                await _photoUploadService.DeletePhotoAsync(user.PhotoPath);
            }

            user.PhotoPath = photoPath;
            user.PhotoUrl = photoUrl;
            await _userManager.UpdateAsync(user);
        }

        public async Task<PagedResponse<NotificationDto>> GetNotificationsAsync(int userId, int pageNumber, int pageSize)
        {
            var notifications = await _dbContext.Notifications
                .Where(n => n.AppUserId == userId)
                .OrderByDescending(n => n.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Assuming sender is the AppUserId (adjust if you have a separate sender field)
            var senderIds = notifications.Select(n => n.AppUserId).Distinct().ToList();
            var senderPhotos = await _dbContext.Users
                .Where(u => senderIds.Contains(u.Id))
                .Select(u => new { u.Id, u.PhotoUrl })
                .ToDictionaryAsync(u => u.Id, u => u.PhotoUrl);

            var dtoList = _mapper.Map<List<NotificationDto>>(notifications);

            foreach (var item in dtoList)
            {
                item.SenderId = item.AppUserId;
                item.SenderPhotoUrl = senderPhotos.GetValueOrDefault(item.AppUserId);
            }

            var total = await _dbContext.Notifications.CountAsync(n => n.AppUserId == userId);

            return new PagedResponse<NotificationDto>
            {
                Items = dtoList,
                TotalCount = total,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }
    }
}
