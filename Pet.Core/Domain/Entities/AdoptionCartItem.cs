namespace Pet.Core.Domain.Entities
{
    public class AdoptionCartItem
    {
        public int Id { get; set; }
        public int AdoptionCartId { get; set; }
        public int PetId { get; set; }
        public virtual AdoptionCart AdoptionCart { get; set; }
        public virtual Pet Pet { get; set; }
    }
}
