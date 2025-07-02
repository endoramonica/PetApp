namespace Pet.Core.Domain.Entities
{
    public class AdoptionCart
    {
        public int Id { get; set; }
        public int AppUserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public virtual AppUser AppUser { get; set; }
        public virtual ICollection<AdoptionCartItem> CartItems { get; set; } = new List<AdoptionCartItem>();
    }
}
