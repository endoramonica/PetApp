namespace Pet.Core.Domain.Entities
{
    public class AdoptionCampaignPet
    {
        public int PetId { get; set; }
        public int AdoptionCampaignId { get; set; }
        public virtual Pet Pet { get; set; }
        public virtual AdoptionCampaign AdoptionCampaign { get; set; }
    }
}
