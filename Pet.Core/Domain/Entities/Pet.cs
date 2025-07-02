using Pet.Core.Domain.Enumerations;


namespace Pet.Core.Domain.Entities
{
    public class Pet
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public Gender Gender { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public virtual ICollection<PetPetCategory> PetPetCategories { get; set; } = new List<PetPetCategory>();
        public virtual ICollection<PetCategory> PetCategories { get; set; } = new List<PetCategory>();
        public virtual ICollection<PetMedicalRecord> MedicalRecords { get; set; } = new List<PetMedicalRecord>();
        public virtual ICollection<PetMedia> Media { get; set; } = new List<PetMedia>();
        public virtual ICollection<AdoptionCartItem> CartItems { get; set; } = new List<AdoptionCartItem>();
        public virtual ICollection<AdoptionDetail> AdoptionDetails { get; set; } = new List<AdoptionDetail>();
        public virtual ICollection<PetRating> Ratings { get; set; } = new List<PetRating>();
        public virtual ICollection<AdoptionCampaignPet> CampaignPets { get; set; } = new List<AdoptionCampaignPet>();
        public virtual ICollection<UserAdoption> UserAdoptions { get; set; } = new List<UserAdoption>();
        public virtual ICollection<UserFavorite> UserFavorites { get; set; } = new List<UserFavorite>();
    }
}
