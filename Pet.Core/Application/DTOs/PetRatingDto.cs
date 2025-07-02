namespace Pet.Core.Application.DTOs
{
    public class PetRatingDto
    {
        public int Id { get; set; }
        public int PetId { get; set; }
        public int AppUserId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
