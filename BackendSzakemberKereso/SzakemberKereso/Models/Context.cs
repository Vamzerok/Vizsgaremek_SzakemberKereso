using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SzakemberKereso.Models;

namespace SzakemberKereso.Models
{
    public class Context : IdentityDbContext<User, Role, int> 
    {
        public DbSet<Expert> Experts { get; set; } = default!;
        public DbSet<ExpertSpecialty> ExpertSpecialties { get; set; } = default!;
        public DbSet<Occupation> Occupations { get; set; } = default!;
        public DbSet<Service> Services { get; set; } = default!;
        public DbSet<Job> Jobs { get; set; } = default!;
        public DbSet<Settlement> Settlements { get; set; } = default!;
        public DbSet<ResidentialAddress> ResidentialAddresses { get; set; } = default!;
        public DbSet<TimeInterval> TimeIntervals { get; set; } = default!;
        public DbSet<Pricing> Pricings { get; set; } = default!;
        public Context(DbContextOptions<Context> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Now we override the table names to remove the "AspNet" prefixes
            modelBuilder.Entity<User>().ToTable("users");
            modelBuilder.Entity<Role>().ToTable("roles");
            modelBuilder.Entity<IdentityUserRole<int>>().ToTable("_user_roles");
            modelBuilder.Entity<IdentityUserClaim<int>>().ToTable("_user_claims");
            modelBuilder.Entity<IdentityUserLogin<int>>().ToTable("_user_logins");
            modelBuilder.Entity<IdentityRoleClaim<int>>().ToTable("_role_claims");
            modelBuilder.Entity<IdentityUserToken<int>>().ToTable("_user_tokens");

            //user-expert one-to-one relationship
            modelBuilder.Entity<Expert>()
                .HasKey(expert => expert.UserId);

            modelBuilder.Entity<Expert>()
                .HasOne(expert => expert.User)
                .WithOne(user => user.Expert)
                .HasForeignKey<Expert>(expert => expert.UserId);

            //expert_specialties many-to-many relationship
            modelBuilder.Entity<ExpertSpecialty>()
                    .HasOne(ef => ef.Expert)
                    .WithMany(e => e.ExpertSpecialties)
                    .HasForeignKey(ef => ef.ExpertId);

            modelBuilder.Entity<ExpertSpecialty>()
                .HasOne(ef => ef.Occupation)
                .WithMany(o => o.ExpertSpecialties)
                .HasForeignKey(ef => ef.OccupationId);

            modelBuilder.Entity<ExpertSpecialty>()
                .HasIndex(ef => new { ef.ExpertId, ef.OccupationId })
                .IsUnique();

            // expert_specialties - service one-to-many relationship
            modelBuilder.Entity<Service>()
                    .HasOne(s => s.ExpertSpecialty)
                    .WithMany(es => es.Services)
                    .HasForeignKey(s => s.ExpertSpecialtyId)
                    .OnDelete(DeleteBehavior.Cascade);

            // job - user (initiating user) one-to-many relationship
            modelBuilder.Entity<Job>()
                .HasOne(j => j.InitiatingUser)
                .WithMany(u => u.InitiatedJobs)
                .HasForeignKey(j => j.InitiatingUserId);

            // job - service one-to-many relationship
            modelBuilder.Entity<Job>()
                .HasOne(j => j.Service)
                .WithMany(s => s.Jobs)
                .HasForeignKey(j => j.ServiceId);

            // service - pricing one-to-one relationship
            modelBuilder.Entity<Service>()
                .HasOne(s => s.Pricing)
                .WithMany()
                .HasForeignKey(s => s.PricingId);

            // job - pricing (offer pricing, optional) relationship
            modelBuilder.Entity<Job>()
                .HasOne(j => j.Pricing)
                .WithMany()
                .HasForeignKey(j => j.PricingId)
                .IsRequired(false);

            // job - address (location) one-to-many relationship
            modelBuilder.Entity<Job>()
                .HasOne(j => j.Location)
                .WithMany(a => a.Jobs)
                .HasForeignKey(j => j.LocationId);

            // expert - settlement (work location) many-to-one relationship
            modelBuilder.Entity<Expert>()
                .HasOne(e => e.WorkLocation)
                .WithMany()
                .HasForeignKey(e => e.WorkLocationId)
                .IsRequired(false);

            // settlement - address one-to-many relationship
            modelBuilder.Entity<ResidentialAddress>()
                .HasOne(a => a.Settlement)
                .WithMany(s => s.Addresses)
                .HasForeignKey(a => a.SettlementId);

            // job - timeinterval one-to-many relationship 
            modelBuilder.Entity<TimeInterval>()
                .HasOne(t => t.Job)
                .WithMany(j => j.TimeIntervals)
                .HasForeignKey(t => t.JobId);

            //dummy data
            modelBuilder.Entity<Occupation>()
                .HasData(
                    new Occupation() { Id = 7223, Name = "Bútorasztalos", Description = "" },
                    new Occupation() { Id = 7321, Name = "Lakatos", Description = "" },
                    new Occupation() { Id = 7326, Name = "Kovács", Description = "" },
                    new Occupation() { Id = 7511, Name = "Kőműves", Description = "" },
                    new Occupation() { Id = 7513, Name = "Ács", Description = "" },
                    new Occupation() { Id = 7521, Name = "Vezeték- és csőhálózat-szerelő (víz, gáz, fűtés)", Description = "" },
                    new Occupation() { Id = 7524, Name = "Épületvillamossági szerelő, villanyszerelő", Description = "" },
                    new Occupation() { Id = 7532, Name = "Tetőfedő", Description = "" },
                    new Occupation() { Id = 7534, Name = "Burkoló", Description = "" },
                    new Occupation() { Id = 7535, Name = "Festő és mázoló", Description = "" },
                    new Occupation() { Id = 7537, Name = "Kályha- és kandallóépítő", Description = "" }
                );

            modelBuilder.Entity<Settlement>()
                .HasData(
                    new Settlement() { Id = 1, PostalCode = 8127, Name = "Aba", CountyName = "Fejér" },
                    new Settlement() { Id = 2, PostalCode = 5241, Name = "Abádszalók", CountyName = "Jász-Nagykun-Szolnok" },
                    new Settlement() { Id = 3, PostalCode = 7678, Name = "Abaliget", CountyName = "Baranya" },
                    new Settlement() { Id = 4, PostalCode = 3261, Name = "Abasár", CountyName = "Heves" },
                    new Settlement() { Id = 5, PostalCode = 3882, Name = "Abaújalpár", CountyName = "Borsod-Abaúj-Zemplén" },
                    new Settlement() { Id = 6, PostalCode = 3882, Name = "Abaújkér", CountyName = "Borsod-Abaúj-Zemplén" },
                    new Settlement() { Id = 7, PostalCode = 3815, Name = "Abaújlak", CountyName = "Borsod-Abaúj-Zemplén" },
                    new Settlement() { Id = 8, PostalCode = 3881, Name = "Abaújszántó", CountyName = "Borsod-Abaúj-Zemplén" },
                    new Settlement() { Id = 9,  PostalCode = 9700, Name = "Szombathely", CountyName = "Vas" },
                    new Settlement() { Id = 10, PostalCode = 9600, Name = "Sárvár", CountyName = "Vas" },
                    new Settlement() { Id = 11, PostalCode = 9798, Name = "Ják", CountyName = "Vas" },
                    new Settlement() { Id = 12, PostalCode = 8019, Name = "Székesfehérvár", CountyName = "Fejér" },
                    new Settlement() { Id = 13, PostalCode = 4042, Name = "Debrecen", CountyName = "Hajdú-Bihar" },
                    new Settlement() { Id = 14, PostalCode = 8308, Name = "Zalahaláp", CountyName = "Veszprém" },
                    new Settlement() { Id = 15, PostalCode = 8300, Name = "Tapolca", CountyName = "Veszprém" }
                );

            modelBuilder.Entity<ResidentialAddress>()
                .HasData(
                    new ResidentialAddress() { Id = 1, SettlementId = 1, StreetName = "Deák Ferenc", PublicAreaType = "utca", BuildingNumber = 12 },
                    new ResidentialAddress() { Id = 2, SettlementId = 1, StreetName = "Petőfi Sándor", PublicAreaType = "utca", BuildingNumber = 3 },
                    new ResidentialAddress() { Id = 3, SettlementId = 2, StreetName = "Kossuth Lajos", PublicAreaType = "tér", BuildingNumber = 7 },
                    new ResidentialAddress() { Id = 4, SettlementId = 3, StreetName = "Dózsa György", PublicAreaType = "út", BuildingNumber = 21 },
                    new ResidentialAddress() { Id = 5, SettlementId = 4, StreetName = "Rákóczi Ferenc", PublicAreaType = "utca", BuildingNumber = 5 },
                    new ResidentialAddress() { Id = 6, SettlementId = 5, StreetName = "Szabadság", PublicAreaType = "tér", BuildingNumber = 14 },
                    new ResidentialAddress() { Id = 7,  SettlementId = 9,  StreetName = "Zrínyi Ilona",   PublicAreaType = "utca",   BuildingNumber = 12 },
                    new ResidentialAddress() { Id = 8,  SettlementId = 9,  StreetName = "Kossuth Lajos",   PublicAreaType = "utca",   BuildingNumber = 5  },
                    new ResidentialAddress() { Id = 9,  SettlementId = 12, StreetName = "Ady Endre",       PublicAreaType = "utca",   BuildingNumber = 15 },
                    new ResidentialAddress() { Id = 10, SettlementId = 13, StreetName = "Bem",             PublicAreaType = "tér",    BuildingNumber = 18 },
                    new ResidentialAddress() { Id = 11, SettlementId = 14, StreetName = "Fő",              PublicAreaType = "utca",   BuildingNumber = 3  },
                    new ResidentialAddress() { Id = 12, SettlementId = 15, StreetName = "Batsányi János",  PublicAreaType = "utca",   BuildingNumber = 2  },
                    new ResidentialAddress() { Id = 13, SettlementId = 11, StreetName = "Petőfi Sándor",   PublicAreaType = "utca",   BuildingNumber = 8  },
                    new ResidentialAddress() { Id = 14, SettlementId = 9,  StreetName = "Arany János",     PublicAreaType = "utca",   BuildingNumber = 12 },
                    new ResidentialAddress() { Id = 15, SettlementId = 12, StreetName = "Vár",             PublicAreaType = "körút",  BuildingNumber = 8  }
                );

            //ExpertSpecialties, Services, and Studies are seeded at runtime by DummyDataGenerator
            //because they depend on users/experts created by ASP.NET Identity (not available during migrations).
        }
    }
}
