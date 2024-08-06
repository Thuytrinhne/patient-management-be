using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PatientManagementApi.Core;
using PatientManagementApi.Models;

namespace PatientManagementApi.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public DbSet<Patient> Patients { get; set; }
        public DbSet<ContactInfor> ContactInfors { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<ApplicationRole> ApplicationRoles { get; set; }


        private readonly ConnectionStringOptions _options;
        public AppDbContext(IOptionsSnapshot<ConnectionStringOptions> options)
        {
            _options = options.Value;
        }

       
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
          => optionsBuilder.UseNpgsql(_options.PostgresConstr);

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            ConfigurePatient(modelBuilder);
            ConfigureContactInfor(modelBuilder);
            ConfigureAddress(modelBuilder);          
        }
        private void ConfigureAddress(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Address>(entity =>
            {
                ConfigureTimestamps<Address>(modelBuilder);
                entity.HasIndex(p => p.PatientId);
                entity.HasOne(p => p.User)
                     .WithMany()
                     .HasForeignKey(p => p.CreatedBy)
                     .IsRequired(false);

            });
        }

        private void ConfigureContactInfor(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ContactInfor>(entity =>
            {
                entity.Property(c => c.Type)
                      .HasConversion<string>();
                entity.HasIndex(c => c.Value)
                      .IsUnique();
                ConfigureTimestamps<ContactInfor>(modelBuilder);

                entity.HasIndex(p => p.PatientId);
                entity.HasOne(p => p.User)
                     .WithMany()
                     .HasForeignKey(p => p.CreatedBy)
                     .IsRequired(false);
            });
        }

        private void ConfigurePatient(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Patient>(entity =>
            {

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

                entity.Property(p => p.DeactivatedAt)
                     .IsRequired(false);

                entity.Property(p => p.DateOfBirth)
                        .HasColumnType("timestamp without time zone");
                entity.Property(p => p.DeactivatedAt)
                      .HasColumnType("timestamp without time zone");
                ConfigureTimestamps<Patient>(modelBuilder);

                entity.HasOne(p => p.User)
                      .WithMany()
                      .HasForeignKey(p => p.CreatedBy)  
                      .IsRequired(false);

            });
        }

     
            private void ConfigureTimestamps<TEntity>(ModelBuilder modelBuilder) where TEntity : class
        {
            modelBuilder.Entity<TEntity>().Property<DateTime>("CreatedAt")
                .HasColumnType("timestamp without time zone");
            modelBuilder.Entity<TEntity>().Property<DateTime>("UpdatedAt")
                .HasColumnType("timestamp without time zone");
        }
    }
}
