namespace Pet.Core.Domain.Entities
{
    public class UserFavorite
    {
        public int Id { get; set; }
        public int AppUserId { get; set; }
        public int PetId { get; set; }
        public DateTime AddedAt { get; set; }
        public virtual AppUser AppUser { get; set; }
        public virtual Pet Pet { get; set; }
    }
}
