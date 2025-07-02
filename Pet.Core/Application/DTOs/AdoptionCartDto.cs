namespace Pet.Core.Application.DTOs
{
    public class AdoptionCartDto
    {
        public int Id { get; set; }
        public int AppUserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<AdoptionCartItemDto> CartItems { get; set; }
    }
}
