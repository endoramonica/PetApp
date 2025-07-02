using Microsoft.Extensions.DependencyInjection;
namespace Pet.API.Extensions
{
    public static class MapperExtensions
    {
        public static IServiceCollection AddMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(Pet.Core.Application.AutoMapper.MappingProfile).Assembly);
            return services;
        }
    }
}