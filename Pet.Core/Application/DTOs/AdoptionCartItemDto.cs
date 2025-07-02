namespace Pet.Core.Application.DTOs
{
    public class AdoptionCartItemDto
    {
        public int Id { get; set; }
        public int AdoptionCartId { get; set; }
        public int PetId { get; set; }
    }
}
