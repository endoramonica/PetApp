namespace Pet.Core.Application.DTOs
{
    // DTO thể hiện người dùng đã đăng nhập
    public class LoggedInUser
    {
        public int Id { get; set; }
        public string UserName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string FullName { get; set; } = default!;
        public string AvatarUrl { get; set; } = "add_a_photo.png";

        public string Role { get; set; }


        public string DisplayName => string.IsNullOrWhiteSpace(FullName) ? Email : FullName;

        public string DisplayPhotoUrl =>
            string.IsNullOrWhiteSpace(AvatarUrl) ? "add_a_photo.png" : AvatarUrl;
    }
}
