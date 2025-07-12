namespace Pet.Core.Application.DTOs;

public class NotificationDto
{
    public int Id { get; set; }
    public int AppUserId { get; set; }
    public string Message { get; set; } = string.Empty;
    public bool IsRead { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? UserPhotoUrl { get; set; }
    public string? SenderPhotoUrl { get; set; }
    public int SenderId { get; set; }

    public string CommentedOnDisplay => CreatedAt.ToString("dd MMM yyyy | HH:mm");

    public string DisplayPhotoUrl =>
        string.IsNullOrWhiteSpace(UserPhotoUrl)
            ? "add_a_photo.png"
            : UserPhotoUrl;
}
    