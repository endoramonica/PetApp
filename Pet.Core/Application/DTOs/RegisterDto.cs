namespace Pet.Core.Application.DTOs
{
    // DTO dùng cho yêu cầu đăng ký
    public record RegisterDto(string FullName, string Email, string Password);
}
