
using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using Pet.Core.Application.DTOs;
using Microsoft.AspNetCore.Identity;


namespace Pet.Core.Domain.Entities
{
    public class AppUser : IdentityUser<int>
    {
        public string FullName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? PhotoPath { get; set; } // so that we may move or delete 
        public string? PhotoUrl { get; set; } // so that it can be consumed by other apps 
        public bool IsDeleted { get; set; } 
        public string RefreshToken { get;  set; }
        public DateTime RefreshTokenExpiry { get;  set; }
        public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();
        public virtual ICollection<AdoptionCart> AdoptionCarts { get; set; } = new List<AdoptionCart>();
        public virtual ICollection<AdoptionRequest> AdoptionRequests { get; set; } = new List<AdoptionRequest>();
        public virtual ICollection<PetRating> PetRatings { get; set; } = new List<PetRating>();
        public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();
        public virtual ICollection<AppUserRole> UserRoles { get; set; } = new List<AppUserRole>();
        public virtual ICollection<UserAdoption> UserAdoptions { get; set; } = new List<UserAdoption>();
        public virtual ICollection<UserFavorite> UserFavorites { get; set; } = new List<UserFavorite>();
      
    }
}
