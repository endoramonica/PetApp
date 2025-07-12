using Pet.Core.Application.DTOs;
using Pet.Core.Contracts.Response;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Pet.Core.Application.Infrastructure
{
    public interface IUserService
    {
        Task<LoggedInUser> GetUserByID(int userId);
        Task<PagedResponse<AppUserDto>> GetAllInRolePaging(string role, int pageNumber, int pageSize, string searchTerm);
        Task<PagedResponse<AppUserDto>> GetAllPaging(int pageNumber, int pageSize, string searchTerm);
        Task<List<string>> GetRolesOfUser(int userId);
        Task AssignRoleToUser(int userId, List<string> roles);
        Task EditUser(int userId, string fullName, string email, DateTime? dateOfBirth, string? newPassword = null);
        Task DeleteUser(int userId);
        Task DeleteUserPermanently(int userId);
        Task ChangePhotoAsync(int userId, Stream fileStream, string fileName);
        Task<PagedResponse<NotificationDto>> GetNotificationsAsync(int userId, int pageNumber, int pageSize);
    }
}
