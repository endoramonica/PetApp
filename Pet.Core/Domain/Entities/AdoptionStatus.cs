namespace Pet.Core.Domain.Entities
{
    public class AdoptionStatus
    {
        public int Id { get; set; }
        public string StatusName { get; set; }
        public virtual ICollection<AdoptionRequest> AdoptionRequests { get; set; } = new List<AdoptionRequest>();
        public virtual ICollection<UserAdoption> UserAdoptions { get; set; } = new List<UserAdoption>();
    }
}
