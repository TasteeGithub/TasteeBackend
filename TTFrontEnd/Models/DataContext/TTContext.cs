using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace TTFrontEnd.Models.DataContext
{
    public partial class TTContext : DbContext
    {
        public TTContext()
        {
        }

        public TTContext(DbContextOptions<TTContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Roles> Roles { get; set; }
        public virtual DbSet<UserRoles> UserRoles { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=TT;Username=devhn;Password=devhn@2019");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Roles>(entity =>
            {
                entity.Property(e => e.Id).HasMaxLength(200);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(256);
            });

            modelBuilder.Entity<UserRoles>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId })
                    .HasName("UserRoles_pkey");

                entity.Property(e => e.UserId).HasMaxLength(200);

                entity.Property(e => e.RoleId).HasMaxLength(200);
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.ToTable("users");

                entity.Property(e => e.Id).HasMaxLength(200);

                entity.Property(e => e.Address).HasMaxLength(500);

                entity.Property(e => e.Avatar).HasMaxLength(50);

                entity.Property(e => e.Birthday).HasColumnType("timestamp(3) without time zone");

                entity.Property(e => e.CreatedDate).HasColumnType("timestamp(3) without time zone");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.FullName).IsRequired();

                entity.Property(e => e.Gender).HasMaxLength(10);

                entity.Property(e => e.LastLogin).HasColumnType("timestamp(3) without time zone");

                entity.Property(e => e.PasswordHash).IsRequired();

                entity.Property(e => e.Role).HasMaxLength(50);

                entity.Property(e => e.Status).HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
