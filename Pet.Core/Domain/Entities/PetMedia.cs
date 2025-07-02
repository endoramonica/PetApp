namespace Pet.Core.Domain.Entities
{
    public class PetMedia
    {
        public int Id { get; set; }
        public int PetId { get; set; }
        public string MediaUrl { get; set; }
        public string MediaType { get; set; }
        public virtual Pet Pet { get; set; }
    }
}
