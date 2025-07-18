namespace Pet.Core.Application.DTOs
{
    public class AdoptionCampaignDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<AdoptionCampaignPetDto> CampaignPets { get; set; }

    }
}
