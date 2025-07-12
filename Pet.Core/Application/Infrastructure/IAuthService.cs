using Microsoft.AspNetCore.Http;
using Pet.Core.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pet.Core.Application.Infrastructure
{
    public interface IAuthService
    {
        Task<ApiResult<LoginResponse>> Authenticate(LoginDto dto);
        Task<ApiResult<LoginResponse>> RefreshToken(string refreshToken);
        Task<ApiResult> Register(RegisterDto dto);
        Task<ApiResult> ChangePassword(int userId, string oldPassword, string newPassword);
        Task<ApiResult> ResetPassword(int userId, string newPassword);
    }
    public interface IPhotoUploadService
    {
        Task<(string PhotoPath, string PhotoUrl)> UploadPhotoAsync(IFormFile photo, params string[] folderPaths);
        Task DeletePhotoAsync(string photoPath);
    }
}
