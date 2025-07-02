namespace Pet.Core.Application.DTOs
{
    public class AdoptionDetailDto
    {
        public int Id { get; set; }
        public int AdoptionRequestId { get; set; }
        public int PetId { get; set; }
    }
}
