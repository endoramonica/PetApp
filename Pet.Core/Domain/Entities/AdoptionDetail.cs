namespace Pet.Core.Domain.Entities
{
    public class AdoptionDetail
    {
        public int Id { get; set; }
        public int AdoptionRequestId { get; set; }
        public int PetId { get; set; }
        public virtual AdoptionRequest AdoptionRequest { get; set; }
        public virtual Pet Pet { get; set; }
    }
}
