using Kurskcartuning.Server_v2.Core.Entities.AppDB;
using Kurskcartuning.Server_v2.Core.Entities.AppDB.VisitInside;
using Kurskcartuning.Server_v2.Core.Entities.Application;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Kurskcartuning.Server_v2.Core.DbContext;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {

    }

    public DbSet<Log> Logs { get; set; }

    public DbSet<Feedback> Feedbacks { get; set; }

    public DbSet<Client> Clients { get; set; }

    public DbSet<Vehicle> Vehicles { get; set; }

    public DbSet<Visit> Visits { get; set; }

    public DbSet<Manufacturer> Manufacturers { get; set; }

    public DbSet<Model> Models { get; set; }

    public DbSet<ListOfWorks> ListOfWorks { get; set; }

    public DbSet<ListOfWorksPrices> ListOfWorksPrices { get; set; }

    public DbSet<Malfunction> Malfunctions { get; set; }

    public DbSet<ScreenShot> ScreenShots { get; set; }

    public DbSet<Suggestion> Suggestions { get; set; }



    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<ApplicationUser>(e =>
        {
            e.ToTable("Users");
        });

        builder.Entity<IdentityUserClaim<string>>(e =>
        {
            e.ToTable("UserClaims");
        });

        builder.Entity<IdentityUserLogin<string>>(e =>
        {
            e.ToTable("UserLogins");
        });

        builder.Entity<IdentityUserToken<string>>(e =>
        {
            e.ToTable("UserTokens");
        });

        builder.Entity<IdentityRole>(e =>
        {
            e.ToTable("Roles");
        });

        builder.Entity<IdentityRoleClaim<string>>(e =>
        {
            e.ToTable("RoleClaims");
        });

        builder.Entity<IdentityUserRole<string>>(e =>
        {
            e.ToTable("UserRoles");
        });


        builder.Entity<Client>(e =>
        {
            e.ToTable("Clients");

            e.HasKey(e => e.Id);
            e.HasMany(e => e.Vehicles).WithOne(e => e.Client).HasForeignKey(e => e.ClientId);
        });

        builder.Entity<Vehicle>(e =>
        {
            e.ToTable("Vehicles");

            e.HasKey(e => e.Id);
            e.HasMany(e => e.Visits).WithOne(e => e.Vehicle).HasForeignKey(e => e.VehicleId);
            e.HasMany(e => e.Manufacturer).WithOne(e => e.Vehicle).HasForeignKey(e => e.VehicleId);
        });

        builder.Entity<Visit>(e =>
        {
            e.ToTable("Visits");

            e.HasKey(e => e.Id);
            e.HasMany(e => e.ScreenShots).WithOne(e => e.Visit).HasForeignKey(e => e.VisitId);
            e.HasMany(e => e.ListWorks).WithOne(e => e.Visit).HasForeignKey(e => e.VisitId);
            e.HasMany(e => e.Malfunctions).WithOne(e => e.Visit).HasForeignKey(e => e.VisitId);
            e.HasMany(e => e.Suggestions).WithOne(e => e.Visit).HasForeignKey(e => e.VisitId);
        });

        builder.Entity<Manufacturer>(e =>
        {
            e.ToTable("Manufacturers");

            e.HasKey(e => e.Id);
            e.HasMany(e => e.Models).WithOne(e => e.Manufacturer).HasForeignKey(e => e.ManufacturerId);
        });

        builder.Entity<Model>(e =>
        {
            e.ToTable("Models");

            e.HasKey(e => e.Id);
        });

        builder.Entity<ListOfWorks>(e =>
        {
            e.ToTable("ListOfWorks");

            e.HasKey(e => e.Id);
            e.HasMany(e => e.Prices).WithOne(e => e.ListOfWorks).HasForeignKey(e => e.ListOfWorksId);
        });

        builder.Entity<ListOfWorksPrices>(e =>
        {
            e.ToTable("ListOfWorksPrices");

            e.HasKey(e => e.Id);
        });

        builder.Entity<Malfunction>(e =>
        {
            e.ToTable("Malfunctions");

            e.HasKey(e => e.Id);
        });

        builder.Entity<ScreenShot>(e =>
        {
            e.ToTable("ScreenShots");

            e.HasKey(e => e.Id);
        });

        builder.Entity<Suggestion>(e =>
        {
            e.ToTable("Suggestions");

            e.HasKey(e => e.Id);
        });
    }

}

