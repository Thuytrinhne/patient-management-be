using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PatientManagementApi.Core;
using PatientManagementApi.Models;

namespace PatientManagementApi.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Patient> Patients { get; set; }
        public DbSet<ContactInfor> ContactInfors { get; set; }
        public DbSet<Address> Addresses { get; set; }

        private readonly ConnectionStringOptions _options;
        public AppDbContext(IOptions<ConnectionStringOptions> options)
        {
            _options = options.Value;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
          => optionsBuilder.UseNpgsql(_options.PostgresConstr);

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Patient>(entity =>
            {
                entity.Navigation(p => p.ContactInfors).AutoInclude();
                entity.Navigation(p => p.Addresses).AutoInclude();
                entity.HasMany(p => p.ContactInfors)
                      .WithOne(c => c.Patient)
                      .HasForeignKey(c => c.PatientId);
                entity.HasMany(p => p.Addresses)
                      .WithOne(a => a.Patient)
                      .HasForeignKey(a => a.PatientId);
                entity.Property(p => p.Gender)
                      .HasConversion<int>();
                entity.Property(p => p.IsActive)
                      .HasDefaultValue(true); 

                entity.Property(p => p.DeactivationReason)
                      .IsRequired(false); 
            });

            modelBuilder.Entity<ContactInfor>(entity =>
            {
                entity.Property(c => c.Type)
                      .HasConversion<string>();
                entity.HasIndex(c => c.Value)
                      .IsUnique();   
            });
            modelBuilder.Entity<Address>()
            .HasIndex(a => a.PatientId);

        }

    }
}
