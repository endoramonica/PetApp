namespace Pet.Core.Application.DTOs
{
    public class AppUserCreateDto
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string AvatarUrl { get; set; }
    }
}
