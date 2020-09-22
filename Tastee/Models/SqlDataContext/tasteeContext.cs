using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Tastee.Models.SqlDataContext
{
    public partial class TasteeContext : DbContext
    {
        public TasteeContext()
        {
        }

        public TasteeContext(DbContextOptions<TasteeContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Brands> Brands { get; set; }
        public virtual DbSet<Nlogs> Nlogs { get; set; }
        public virtual DbSet<OperatorRoles> OperatorRoles { get; set; }
        public virtual DbSet<Operators> Operators { get; set; }
        public virtual DbSet<Roles> Roles { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Data Source=MINHTHU-PC;Initial Catalog=tastee;Persist Security Info=True;User ID=tas77143_tastee;Password=K5EOcP3MPgUpc");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Brands>(entity =>
            {
                entity.ToTable("Brands", "tas77143_tastee");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Address)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.Area).HasMaxLength(100);

                entity.Property(e => e.Categories).HasMaxLength(100);

                entity.Property(e => e.City).HasMaxLength(100);

                entity.Property(e => e.CreatedDate).HasColumnType("smalldatetime");

                entity.Property(e => e.Cuisines).HasMaxLength(100);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.HeadOffice)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Hotline)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Logo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.MerchantId)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Name).IsRequired();

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.SeoImage)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.SeoTitle).HasMaxLength(100);

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.UpdateBy).HasMaxLength(50);

                entity.Property(e => e.UpdatedDate).HasColumnType("smalldatetime");

                entity.Property(e => e.Uri)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<Nlogs>(entity =>
            {
                entity.ToTable("NLogs", "tas77143_tastee");

                entity.Property(e => e.Application)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Level)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.LoggedDate).HasColumnType("datetime");

                entity.Property(e => e.Logger).HasMaxLength(250);

                entity.Property(e => e.Message).IsRequired();
            });

            modelBuilder.Entity<OperatorRoles>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId });

                entity.ToTable("OperatorRoles", "tas77143_tastee");

                entity.Property(e => e.UserId)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RoleId)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Operators>(entity =>
            {
                entity.ToTable("Operators", "tas77143_tastee");

                entity.Property(e => e.Id)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasColumnType("smalldatetime");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FullName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.LastLogin).HasColumnType("smalldatetime");

                entity.Property(e => e.PasswordHash)
                    .IsRequired()
                    .HasColumnType("text");

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.Status)
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Roles>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Roles", "tas77143_tastee");

                entity.Property(e => e.Id)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Users", "tas77143_tastee");

                entity.Property(e => e.Address).HasMaxLength(200);

                entity.Property(e => e.Avatar)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Birthday).HasColumnType("smalldatetime");

                entity.Property(e => e.CreatedDate).HasColumnType("smalldatetime");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FullName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Gender)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Id)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LastLogin).HasColumnType("smalldatetime");

                entity.Property(e => e.PasswordHash)
                    .IsRequired()
                    .HasColumnType("text");

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.Qrcode)
                    .HasColumnName("QRCode")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Status)
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
