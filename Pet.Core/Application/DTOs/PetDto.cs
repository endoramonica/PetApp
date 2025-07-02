namespace Pet.Core.Application.DTOs
{
    public class PetDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<PetCategoryDto> Categories { get; set; }
        public List<PetMediaDto> Media { get; set; }
    }
}
