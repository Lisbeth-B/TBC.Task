using Microsoft.EntityFrameworkCore;
using TBC.Task.Infrastructure.Database.Tables;

namespace TBC.Task.Infrastructure.Database;
public class DBContext(DbContextOptions<DBContext> options) : DbContext(options)
{
    public DbSet<Person> Persons { get; set; }
    public DbSet<PhoneNumber> PhoneNumbers { get; set; }
    public DbSet<Relation> Relationships { get; set; }
    public DbSet<City> Cities { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Person>(entity =>
        {
            entity.ToTable("Person");

            entity.Property(x => x.FirstName).HasMaxLength(50);
            entity.Property(x => x.LastName).HasMaxLength(50);
            entity.Property(x => x.PersonalNumber).HasMaxLength(20);
            entity.Property(x => x.Image).HasMaxLength(1000);

            entity.HasOne(x => x.City)
            .WithMany(x => x.People)
            .HasForeignKey(x => x.CityId);

            entity.HasMany(x => x.PhoneNumbers)
            .WithOne(x => x.Person)
            .HasForeignKey(x => x.PersonId)
            .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<PhoneNumber>(entity =>
        {
            entity.ToTable("PhoneNumber");

            entity.Property(x => x.Number).HasMaxLength(20);
        });

        modelBuilder.Entity<City>(entity =>
        {
            entity.ToTable("City");

            entity.Property(x => x.Name).HasMaxLength(50);

            var id = 1;
            entity.HasData(new[]
        {
                   new {Id = 1, Name = "Tbilisi"},
                   new {Id = 2, Name = "Batumi"},
                   new {Id = 3, Name = "Kutaisi"},
                   new {Id = 4, Name = "Rustavi"},
                   new {Id = 5, Name = "Gori"},
                   new {Id = 6, Name = "Zugdidi"},
                   new {Id = 7, Name = "Poti"},
                   new {Id = 8, Name = "Khashuri"},
                   new {Id = 9, Name = "Samtredia"},
                   new {Id = 10, Name = "Senaki"},

                    }
        .Select(a => new City
        {
            Id = id++,
            Name = a.Name,
        }));


        });

        modelBuilder.Entity<Relation>(entity =>
        {
            entity.ToTable("Relation");

            entity.HasOne(x => x.Person)
                  .WithMany(x => x.Relationships)
                  .HasForeignKey(x => x.PersonId);
        });
    }
}
