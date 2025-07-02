namespace Pet.Core.Application.DTOs
{
    public class AdoptionRequestDto
    {
        public int Id { get; set; }
        public int AppUserId { get; set; }
        public int AddressId { get; set; }
        public int AdoptionStatusId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public List<AdoptionDetailDto> AdoptionDetails { get; set; }
    }
}
