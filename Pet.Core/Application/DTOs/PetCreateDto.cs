using Pet.Core.Domain.Enumerations;

namespace Pet.Core.Application.DTOs
{
    public class PetCreateDto
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public Gender Gender { get; set; }
        public string Description { get; set; }
        public List<int> CategoryIds { get; set; }
    }
}
