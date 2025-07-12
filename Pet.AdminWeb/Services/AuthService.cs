using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Pet.AdminWeb.Apis;
using Pet.Core.Application.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Pet.AdminWeb.Services
{
    public class AuthService
    {
        private const string AccessTokenCookie = "AuthToken";
        private const string RefreshTokenCookie = "RefreshToken";

        private readonly IAuthApi _authApi;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthService(IAuthApi authApi, IHttpContextAccessor httpContextAccessor)
        {
            _authApi = authApi;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ApiResult<LoginResponse>> LoginAsync(LoginDto dto)
        {
            var result = await _authApi.LoginAsync(dto);
            if (!result.IsSuccess) return result;

            SaveTokenToCookie(result.Data.Token);
            SaveRefreshTokenToCookie(result.Data.RefreshToken);
            await SignInAsync(result.Data.Token);

            return result;
        }

        public async Task<ApiResult> RegisterAsync(RegisterDto dto)
        {
            return await _authApi.RegisterAsync(dto);
        }

        public async Task<ApiResult> ChangePasswordAsync(ChangePasswordDto dto)
        {
            return await _authApi.ChangePasswordAsync(dto);
        }

        public async Task<ApiResult> ResetPasswordAsync(ResetPasswordDto dto)
        {
            return await _authApi.ResetPasswordAsync(dto);
        }

        public async Task<ApiResult<LoginResponse>> RefreshTokenAsync()
        {
            var refreshToken = GetRefreshToken();
            if (string.IsNullOrEmpty(refreshToken))
                return ApiResult<LoginResponse>.Fail("Không tìm thấy Refresh Token");

            var result = await _authApi.RefreshTokenAsync(refreshToken);
            if (!result.IsSuccess) return result;

            SaveTokenToCookie(result.Data.Token);
            SaveRefreshTokenToCookie(result.Data.RefreshToken);
            await SignInAsync(result.Data.Token);

            return result;
        }

        public async Task LogoutAsync()
        {
            var ctx = _httpContextAccessor.HttpContext;
            if (ctx != null)
            {
                ctx.Response.Cookies.Delete(AccessTokenCookie);
                ctx.Response.Cookies.Delete(RefreshTokenCookie);
                await ctx.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            }
        }

        public string? GetAccessToken() =>
            _httpContextAccessor.HttpContext?.Request.Cookies[AccessTokenCookie];

        public string? GetRefreshToken() =>
            _httpContextAccessor.HttpContext?.Request.Cookies[RefreshTokenCookie];

        public LoggedInUser? GetCurrentUser()
        {
            var token = GetAccessToken();
            if (string.IsNullOrEmpty(token)) return null;

            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadToken(token) as JwtSecurityToken;
            if (jwt == null) return null;

            return new LoggedInUser
            {
                Id = int.TryParse(jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value, out int id) ? id : 0,
                Email = jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value ?? "",
                UserName = jwt.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Name)?.Value ?? "",
                FullName = jwt.Claims.FirstOrDefault(c => c.Type == "fullName")?.Value ?? "",
                AvatarUrl = jwt.Claims.FirstOrDefault(c => c.Type == "photoUrl")?.Value ?? "add_a_photo.png",
                Role = jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value ?? ""
            };
        }

        private async Task SignInAsync(string jwt)
        {
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(jwt);
            var identity = new ClaimsIdentity(token.Claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await _httpContextAccessor.HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1)
                });
        }

        private void SaveTokenToCookie(string token)
        {
            _httpContextAccessor.HttpContext?.Response.Cookies.Append(
                AccessTokenCookie,
                token,
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTimeOffset.UtcNow.AddHours(1)
                });
        }

        private void SaveRefreshTokenToCookie(string token)
        {
            _httpContextAccessor.HttpContext?.Response.Cookies.Append(
                RefreshTokenCookie,
                token,
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTimeOffset.UtcNow.AddDays(7)
                });
        }
    }
}
