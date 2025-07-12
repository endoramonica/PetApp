namespace Pet.Core.Domain.Entities
{
    public class Notification
    {
        public int Id { get; set; }
        public int AppUserId { get; set; }
        public string Message { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public virtual AppUser AppUser { get; set; }
    }
}
