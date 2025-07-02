namespace Pet.Core.Domain.Entities
{
    public class UserAdoption
    {
        public int Id { get; set; }
        public int AppUserId { get; set; }
        public int PetId { get; set; }
        public int AdoptionStatusId { get; set; }
        public DateTime CreatedAt { get; set; }
        public virtual AppUser AppUser { get; set; }
        public virtual Pet Pet { get; set; }
        public virtual AdoptionStatus AdoptionStatus { get; set; }
    }
}
