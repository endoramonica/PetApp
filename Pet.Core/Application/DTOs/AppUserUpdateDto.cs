namespace Pet.Core.Application.DTOs
{
    public class AppUserUpdateDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string AvatarUrl { get; set; }
    }
}
