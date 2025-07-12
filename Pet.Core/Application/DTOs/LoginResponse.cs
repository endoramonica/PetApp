namespace Pet.Core.Application.DTOs
{
    // DTO trả về sau khi đăng nhập, bao gồm cả RefreshToken
    public record LoginResponse(LoggedInUser User, string Token, string RefreshToken);
    public class RefreshTokenResponse
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
    public class RefreshTokenRequest
    {
        public string RefreshToken { get; set; }
    }

}
