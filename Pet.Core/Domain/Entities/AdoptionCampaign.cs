namespace Pet.Core.Domain.Entities
{
    public class AdoptionCampaign
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public virtual ICollection<AdoptionCampaignPet> CampaignPets { get; set; } = new List<AdoptionCampaignPet>();
    }
}
