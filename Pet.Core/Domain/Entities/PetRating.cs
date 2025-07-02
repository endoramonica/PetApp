namespace Pet.Core.Domain.Entities
{
    public class PetRating
        {
            public int Id { get; set; }
            public int PetId { get; set; }
            public int AppUserId { get; set; }
            public int Rating { get; set; }
            public string Comment { get; set; }
            public DateTime CreatedAt { get; set; }
            public virtual Pet Pet { get; set; }
            public virtual AppUser AppUser { get; set; }
        }
}
