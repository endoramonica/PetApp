using Pet.Core.Application.DTOs;
using Refit;

namespace Pet.AdminWeb.Apis
{  
    public interface IAuthApi
    {  
        

        [Post("/api/auth/login")]
        Task<ApiResult<LoginResponse>> LoginAsync([Body] LoginDto dto);

        [Post("/api/auth/refresh")]
        Task<ApiResult<LoginResponse>> RefreshTokenAsync([AliasAs("refreshToken")] string refreshToken);

        [Post("/api/auth/register")]
        Task<ApiResult> RegisterAsync([Body] RegisterDto dto);

        [Post("/api/auth/change-password")]
        Task<ApiResult> ChangePasswordAsync([Body] ChangePasswordDto dto);

        [Post("/api/auth/reset-password")]
        Task<ApiResult> ResetPasswordAsync([Body] ResetPasswordDto dto);
      
        
    }
}
