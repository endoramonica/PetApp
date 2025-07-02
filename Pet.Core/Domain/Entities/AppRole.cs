using Microsoft.AspNetCore.Identity;


namespace Pet.Core.Domain.Entities
{
    public class AppRole : IdentityRole<int>
    {
        public string Description { get; set; }
        public virtual ICollection<AppUserRole> UserRoles { get; set; } = new List<AppUserRole>();
    }
}
