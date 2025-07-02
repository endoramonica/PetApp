using Pet.Core.Application.DTOs;
using Pet.Core.Domain.Entities;
using System.Linq;

namespace Pet.Core.Application.Selectors
{
    public static class PetSelectors
    {
        public static IQueryable<PetDto> ToPetDto(this IQueryable<Domain.Entities.Pet> query)
        {
            return query.Select(p => new PetDto
            {
                Id = p.Id,
                Name = p.Name,
                Age = p.Age,
                Gender = p.Gender.ToString(),
                Description = p.Description,
                CreatedAt = p.CreatedAt,
                Categories = p.PetPetCategories
                    .Select(pc => new PetCategoryDto
                    {
                        Id = pc.PetCategory.Id,
                        Name = pc.PetCategory.Name,
                        Description = pc.PetCategory.Description
                    })
                    .ToList(),
                Media = p.Media
                    .Select(m => new PetMediaDto
                    {
                        Id = m.Id,
                        PetId = m.PetId,
                        MediaUrl = m.MediaUrl,
                        MediaType = m.MediaType
                    })
                    .ToList()
            });
        }
    }
}
