using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Pet.Core.Domain.Entities;
using Pet.Core.Domain.Enumerations;
using System;
using System.Linq;

namespace Pet.Core.INFRASTRUCTURE.Data
{
    public class ApplicationDBContext : IdentityDbContext<AppUser, AppRole, int>
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options) { }

        public DbSet<Address> Addresses { get; set; }
        public DbSet<Pet.Core.Domain.Entities.Pet> Pets { get; set; }
        public DbSet<PetCategory> PetCategories { get; set; }
        public DbSet<PetPetCategory> PetPetCategories { get; set; }
        public DbSet<PetMedicalRecord> PetMedicalRecords { get; set; }
        public DbSet<PetMedia> PetMedia { get; set; }
        public DbSet<PetRating> PetRatings { get; set; }
        public DbSet<AdoptionCart> AdoptionCarts { get; set; }
        public DbSet<AdoptionCartItem> AdoptionCartItems { get; set; }
        public DbSet<AdoptionRequest> AdoptionRequests { get; set; }
        public DbSet<AdoptionDetail> AdoptionDetails { get; set; }
        public DbSet<AdoptionStatus> AdoptionStatuses { get; set; }
        public DbSet<AdoptionCampaign> AdoptionCampaigns { get; set; }
        public DbSet<AdoptionCampaignPet> AdoptionCampaignPets { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<UserAdoption> UserAdoptions { get; set; }
        public DbSet<UserFavorite> UserFavorites { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Pet - PetCategory (many-to-many)
            builder.Entity<PetPetCategory>()
                .HasKey(ppc => new { ppc.PetId, ppc.PetCategoryId });

            builder.Entity<PetPetCategory>()
                .HasOne(ppc => ppc.Pet)
                .WithMany(p => p.PetPetCategories)
                .HasForeignKey(ppc => ppc.PetId);

            builder.Entity<PetPetCategory>()
                .HasOne(ppc => ppc.PetCategory)
                .WithMany(pc => pc.PetPetCategories)
                .HasForeignKey(ppc => ppc.PetCategoryId);

            // Pet - Campaign (many-to-many)
            builder.Entity<AdoptionCampaignPet>()
                .HasKey(acp => new { acp.PetId, acp.AdoptionCampaignId });

            builder.Entity<AppUserRole>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId);

            builder.Entity<AppUserRole>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId);

            // AdoptionRequest → AdoptionStatus
            builder.Entity<AdoptionRequest>()
                .HasOne(ar => ar.AdoptionStatus)
                .WithMany(ads => ads.AdoptionRequests)
                .HasForeignKey(ar => ar.AdoptionStatusId)
                .OnDelete(DeleteBehavior.Restrict); // fix multiple cascade paths

            // AdoptionRequest → AppUser
            builder.Entity<AdoptionRequest>()
                .HasOne(ar => ar.AppUser)
                .WithMany(u => u.AdoptionRequests)
                .HasForeignKey(ar => ar.AppUserId)
                .OnDelete(DeleteBehavior.Restrict); // fix multiple cascade paths

            // AdoptionRequest → Address
            builder.Entity<AdoptionRequest>()
                .HasOne(ar => ar.Address)
                .WithMany(a => a.AdoptionRequests)
                .HasForeignKey(ar => ar.AddressId)
                .OnDelete(DeleteBehavior.Restrict); // fix multiple cascade paths

            // UserAdoption → AdoptionStatus
            builder.Entity<UserAdoption>()
                .HasOne(ua => ua.AdoptionStatus)
                .WithMany(ads => ads.UserAdoptions)
                .HasForeignKey(ua => ua.AdoptionStatusId);

            // Seed enum AdoptionStatusEnum
            builder.Entity<AdoptionStatus>().HasData(
                Enum.GetValues(typeof(AdoptionStatusEnum)).Cast<AdoptionStatusEnum>().Select(e =>
                    new AdoptionStatus
                    {
                        Id = (int)e,
                        StatusName = e.ToString()
                    }
                ).ToArray()
            );

            // Seed PetCategory
            builder.Entity<PetCategory>().HasData(
                new PetCategory { Id = 1, Name = "Dog", Description = "Dog category" },
                new PetCategory { Id = 2, Name = "Cat", Description = "Cat category" }
            );
        }

    }
}
