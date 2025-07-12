using AutoMapper;
using Azure.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Pet.Core.Application.DTOs;
using Pet.Core.Application.Infrastructure;
using Pet.Core.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Pet.Core.Application.ServicesApplication
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public AuthService(UserManager<AppUser> userManager, IConfiguration configuration , IMapper mapper)
        {
            _userManager = userManager;
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task<ApiResult<LoginResponse>> Authenticate(LoginDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null || user.IsDeleted || !await _userManager.CheckPasswordAsync(user, dto.Password))
                return ApiResult<LoginResponse>.Fail("Email hoặc mật khẩu không hợp lệ.");

            var accessToken = await GenerateJwtToken(user);
            var refreshToken = GenRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
            await _userManager.UpdateAsync(user);

            var userInfo = _mapper.Map<LoggedInUser>(user);
            return ApiResult<LoginResponse>.Success(new LoginResponse(userInfo, accessToken, refreshToken));

        }

        public async Task<ApiResult<LoginResponse>> RefreshToken(string refreshToken)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u =>
                u.RefreshToken == refreshToken && u.RefreshTokenExpiry > DateTime.UtcNow);

            if (user == null)
                return ApiResult<LoginResponse>.Fail("Refresh token không hợp lệ hoặc đã hết hạn.");

            var newAccessToken = await GenerateJwtToken(user);
            var newRefreshToken = GenRefreshToken();
            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
            await _userManager.UpdateAsync(user);

            var userInfo = _mapper.Map<LoggedInUser>(user);
            return ApiResult<LoginResponse>.Success(new LoginResponse(userInfo, newAccessToken, newRefreshToken));

        }

        public async Task<ApiResult> Register(RegisterDto dto)
        {
            var existingUser = await _userManager.FindByEmailAsync(dto.Email);
            if (existingUser != null)
                return ApiResult.Fail("Email đã được sử dụng.");

            var user = new AppUser
            {
                UserName = dto.Email,
                Email = dto.Email,
                FullName = dto.FullName,
                CreatedAt = DateTime.UtcNow,
                RefreshToken = GenRefreshToken(),
                RefreshTokenExpiry = DateTime.UtcNow.AddDays(7)
            };

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
                return ApiResult.Fail(string.Join(", ", result.Errors.Select(e => e.Description)));

            await _userManager.AddToRoleAsync(user, "User");
            return ApiResult.Success();
        }

        public async Task<ApiResult> ChangePassword(int userId, string oldPassword, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null || user.IsDeleted)
                return ApiResult.Fail("Người dùng không tồn tại.");

            var result = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
            return result.Succeeded
                ? ApiResult.Success()
                : ApiResult.Fail(string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        public async Task<ApiResult> ResetPassword(int userId, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null || user.IsDeleted)
                return ApiResult.Fail("Người dùng không tồn tại.");

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);

            return result.Succeeded
                ? ApiResult.Success()
                : ApiResult.Fail(string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        private async Task<string> GenerateJwtToken(AppUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var roles = await _userManager.GetRolesAsync(user);
            claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(int.Parse(_configuration["Jwt:ExpireInMinutes"]!)),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private AppUserDto MapToAppUserDto(AppUser user) =>
            new()
            {
                Id = user.Id,
                UserName = user.UserName!,
                Email = user.Email!,
                FullName = user.FullName,
                CreatedAt = user.CreatedAt,
                AvatarUrl = user.PhotoUrl ?? "add_a_photo.png",
                PhotoUrl = user.PhotoUrl ?? ""
            };

        public string GenRefreshToken()
        {
            return Convert.ToBase64String(Guid.NewGuid().ToByteArray());
        }
    }
}
