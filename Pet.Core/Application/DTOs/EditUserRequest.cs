namespace Pet.Core.Application.DTOs
{
    public class EditUserRequest
    {
        public string FullName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public DateTime? DateOfBirth { get; set; }
        public string? NewPassword { get; set; }
    }

}
