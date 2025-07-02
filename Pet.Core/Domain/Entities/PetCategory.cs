namespace Pet.Core.Domain.Entities
{
    public class PetCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<PetPetCategory> PetPetCategories { get; set; } = new List<PetPetCategory>();
        public virtual ICollection<PetCategory> PetCategories { get; set; } = new List<PetCategory>();
    }
    public class PetPetCategory
    {
        public int PetId { get; set; }
        public Pet Pet { get; set; }

        public int PetCategoryId { get; set; }
        public PetCategory PetCategory { get; set; }
    }

}
