using Microsoft.AspNetCore.Identity;


namespace Pet.Core.Domain.Entities
{
    public class AppUserRole : IdentityUserRole<int>
    {
        public virtual AppUser User { get; set; }
        public virtual AppRole Role { get; set; }
    }
}
