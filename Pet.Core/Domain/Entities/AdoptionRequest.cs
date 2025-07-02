namespace Pet.Core.Domain.Entities
{
    public class AdoptionRequest
    {
        public int Id { get; set; }
        public int AppUserId { get; set; }
        public int AddressId { get; set; }
        public int AdoptionStatusId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public virtual AppUser AppUser { get; set; }
        public virtual Address Address { get; set; }
        public virtual AdoptionStatus AdoptionStatus { get; set; }
        public virtual ICollection<AdoptionDetail> AdoptionDetails { get; set; } = new List<AdoptionDetail>();
    }
}
