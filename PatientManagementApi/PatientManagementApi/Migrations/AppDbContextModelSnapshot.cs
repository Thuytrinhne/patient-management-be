// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using PatientManagementApi.Data;

#nullable disable

namespace PatientManagementApi.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("integer");

                NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                b.Property<string>("ClaimType")
                    .HasColumnType("text");

                b.Property<string>("ClaimValue")
                    .HasColumnType("text");

                b.Property<Guid>("RoleId")
                    .HasColumnType("uuid");

                b.HasKey("Id");

                b.HasIndex("RoleId");

                b.ToTable("AspNetRoleClaims", (string)null);
            });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("integer");

                NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                b.Property<string>("ClaimType")
                    .HasColumnType("text");

                b.Property<string>("ClaimValue")
                    .HasColumnType("text");

                b.Property<Guid>("UserId")
                    .HasColumnType("uuid");

                b.HasKey("Id");

                b.HasIndex("UserId");

                b.ToTable("AspNetUserClaims", (string)null);
            });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
            {
                b.Property<string>("LoginProvider")
                    .HasColumnType("text");

                b.Property<string>("ProviderKey")
                    .HasColumnType("text");

                b.Property<string>("ProviderDisplayName")
                    .HasColumnType("text");

                b.Property<Guid>("UserId")
                    .HasColumnType("uuid");

                b.HasKey("LoginProvider", "ProviderKey");

                b.HasIndex("UserId");

                b.ToTable("AspNetUserLogins", (string)null);
            });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
            {
                b.Property<Guid>("UserId")
                    .HasColumnType("uuid");

                b.Property<Guid>("RoleId")
                    .HasColumnType("uuid");

                b.HasKey("UserId", "RoleId");

                b.HasIndex("RoleId");

                b.ToTable("AspNetUserRoles", (string)null);
            });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
            {
                b.Property<Guid>("UserId")
                    .HasColumnType("uuid");

                b.Property<string>("LoginProvider")
                    .HasColumnType("text");

                b.Property<string>("Name")
                    .HasColumnType("text");

                b.Property<string>("Value")
                    .HasColumnType("text");

                b.HasKey("UserId", "LoginProvider", "Name");

                b.ToTable("AspNetUserTokens", (string)null);
            });

            modelBuilder.Entity("PatientManagementApi.Models.Address", b =>
            {
                b.Property<Guid>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("uuid");

                b.Property<DateTime>("CreatedAt")
                    .HasColumnType("timestamp without time zone");

                b.Property<Guid?>("CreatedBy")
                    .HasColumnType("uuid");

                b.Property<string>("DetailAddress")
                    .IsRequired()
                    .HasColumnType("text");

                b.Property<string>("District")
                    .IsRequired()
                    .HasColumnType("text");

                b.Property<bool>("IsDefault")
                    .HasColumnType("boolean");

                b.Property<Guid>("PatientId")
                    .HasColumnType("uuid");

                b.Property<string>("Province")
                    .IsRequired()
                    .HasColumnType("text");

                b.Property<DateTime?>("UpdatedAt")
                    .HasColumnType("timestamp without time zone");

                b.Property<Guid?>("UpdatedBy")
                    .HasColumnType("uuid");

                b.Property<string>("Ward")
                    .IsRequired()
                    .HasColumnType("text");

                b.HasKey("Id");

                b.HasIndex("PatientId");

                b.ToTable("Addresses");
            });

            modelBuilder.Entity("PatientManagementApi.Models.ApplicationRole", b =>
            {
                b.Property<Guid>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("uuid");

                b.Property<string>("ConcurrencyStamp")
                    .IsConcurrencyToken()
                    .HasColumnType("text");

                b.Property<string>("Name")
                    .HasMaxLength(256)
                    .HasColumnType("character varying(256)");

                b.Property<string>("NormalizedName")
                    .HasMaxLength(256)
                    .HasColumnType("character varying(256)");

                b.HasKey("Id");

                b.HasIndex("NormalizedName")
                    .IsUnique()
                    .HasDatabaseName("RoleNameIndex");

                b.ToTable("AspNetRoles", (string)null);
            });

            modelBuilder.Entity("PatientManagementApi.Models.ApplicationUser", b =>
            {
                b.Property<Guid>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("uuid");

                b.Property<int>("AccessFailedCount")
                    .HasColumnType("integer");

                b.Property<string>("ConcurrencyStamp")
                    .IsConcurrencyToken()
                    .HasColumnType("text");

                b.Property<string>("Email")
                    .HasMaxLength(256)
                    .HasColumnType("character varying(256)");

                b.Property<bool>("EmailConfirmed")
                    .HasColumnType("boolean");

                b.Property<string>("FirstName")
                    .IsRequired()
                    .HasColumnType("text");

                b.Property<string>("LastName")
                    .IsRequired()
                    .HasColumnType("text");

                b.Property<bool>("LockoutEnabled")
                    .HasColumnType("boolean");

                b.Property<DateTimeOffset?>("LockoutEnd")
                    .HasColumnType("timestamp with time zone");

                b.Property<string>("NormalizedEmail")
                    .HasMaxLength(256)
                    .HasColumnType("character varying(256)");

                b.Property<string>("NormalizedUserName")
                    .HasMaxLength(256)
                    .HasColumnType("character varying(256)");

                b.Property<string>("PasswordHash")
                    .HasColumnType("text");

                b.Property<string>("PhoneNumber")
                    .HasColumnType("text");

                b.Property<bool>("PhoneNumberConfirmed")
                    .HasColumnType("boolean");

                b.Property<string>("RefreshToken")
                    .HasColumnType("text");

                b.Property<string>("SecurityStamp")
                    .HasColumnType("text");

                b.Property<bool>("TwoFactorEnabled")
                    .HasColumnType("boolean");

                b.Property<string>("UserName")
                    .HasMaxLength(256)
                    .HasColumnType("character varying(256)");

                b.HasKey("Id");

                b.HasIndex("NormalizedEmail")
                    .HasDatabaseName("EmailIndex");

                b.HasIndex("NormalizedUserName")
                    .IsUnique()
                    .HasDatabaseName("UserNameIndex");

                b.ToTable("AspNetUsers", (string)null);
            });

            modelBuilder.Entity("PatientManagementApi.Models.ContactInfor", b =>
            {
                b.Property<Guid>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("uuid");

                b.Property<DateTime>("CreatedAt")
                    .HasColumnType("timestamp without time zone");

                b.Property<Guid?>("CreatedBy")
                    .HasColumnType("uuid");

                b.Property<Guid>("PatientId")
                    .HasColumnType("uuid");

                b.Property<string>("Type")
                    .IsRequired()
                    .HasColumnType("text");

                b.Property<DateTime?>("UpdatedAt")
                    .HasColumnType("timestamp without time zone");

                b.Property<Guid?>("UpdatedBy")
                    .HasColumnType("uuid");

                b.Property<string>("Value")
                    .IsRequired()
                    .HasColumnType("text");

                b.HasKey("Id");

                b.HasIndex("PatientId");

                b.HasIndex("Value")
                    .IsUnique();

                b.ToTable("ContactInfors");
            });

            modelBuilder.Entity("PatientManagementApi.Models.Patient", b =>
            {
                b.Property<Guid>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("uuid");

                b.Property<DateTime>("CreatedAt")
                    .HasColumnType("timestamp without time zone");

                b.Property<Guid?>("CreatedBy")
                    .HasColumnType("uuid");

                b.Property<DateTime>("DateOfBirth")
                    .HasColumnType("timestamp without time zone");

                b.Property<DateTime?>("DeactivatedAt")
                    .HasColumnType("timestamp without time zone");

                b.Property<string>("DeactivationReason")
                    .HasColumnType("text");

                b.Property<string>("FirstName")
                    .IsRequired()
                    .HasColumnType("text");

                b.Property<int>("Gender")
                    .HasColumnType("integer");

                b.Property<bool>("IsActive")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("boolean")
                    .HasDefaultValue(true);

                b.Property<string>("LastName")
                    .IsRequired()
                    .HasColumnType("text");

                b.Property<DateTime?>("UpdatedAt")
                    .HasColumnType("timestamp without time zone");

                b.Property<Guid?>("UpdatedBy")
                    .HasColumnType("uuid");

                b.HasKey("Id");

                b.ToTable("Patients");
            });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
            {
                b.HasOne("PatientManagementApi.Models.ApplicationRole", null)
                    .WithMany()
                    .HasForeignKey("RoleId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();
            });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
            {
                b.HasOne("PatientManagementApi.Models.ApplicationUser", null)
                    .WithMany()
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();
            });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
            {
                b.HasOne("PatientManagementApi.Models.ApplicationUser", null)
                    .WithMany()
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();
            });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
            {
                b.HasOne("PatientManagementApi.Models.ApplicationRole", null)
                    .WithMany()
                    .HasForeignKey("RoleId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.HasOne("PatientManagementApi.Models.ApplicationUser", null)
                    .WithMany()
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();
            });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
            {
                b.HasOne("PatientManagementApi.Models.ApplicationUser", null)
                    .WithMany()
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();
            });

            modelBuilder.Entity("PatientManagementApi.Models.Address", b =>
            {
                b.HasOne("PatientManagementApi.Models.Patient", "Patient")
                    .WithMany("Addresses")
                    .HasForeignKey("PatientId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.Navigation("Patient");
            });

            modelBuilder.Entity("PatientManagementApi.Models.ContactInfor", b =>
            {
                b.HasOne("PatientManagementApi.Models.Patient", "Patient")
                    .WithMany("ContactInfors")
                    .HasForeignKey("PatientId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.Navigation("Patient");
            });

            modelBuilder.Entity("PatientManagementApi.Models.Patient", b =>
            {
                b.Navigation("Addresses");

                b.Navigation("ContactInfors");
            });
#pragma warning restore 612, 618
        }
    }
}
