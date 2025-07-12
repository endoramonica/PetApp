namespace Pet.Core.Application.DTOs
{
    public record ChangePasswordDto(int UserId, string OldPassword, string NewPassword);

}
