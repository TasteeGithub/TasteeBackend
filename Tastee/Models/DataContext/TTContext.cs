﻿using System;
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

        public virtual DbSet<Brands> Brands { get; set; }
        public virtual DbSet<OperatorRoles> OperatorRoles { get; set; }
        public virtual DbSet<Operators> Operators { get; set; }
        public virtual DbSet<Roles> Roles { get; set; }
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
            modelBuilder.Entity<Brands>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("character varying");

                entity.Property(e => e.Address).IsRequired();

                entity.Property(e => e.Area).HasColumnType("character varying");

                entity.Property(e => e.Categories).HasColumnType("character varying");

                entity.Property(e => e.City).HasColumnType("character varying");

                entity.Property(e => e.CreatedDate).HasColumnType("timestamp(3) without time zone");

                entity.Property(e => e.Cuisines).HasColumnType("character varying");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnType("character varying");

                entity.Property(e => e.HeadOffice)
                    .IsRequired()
                    .HasColumnType("character varying");

                entity.Property(e => e.Hotline)
                    .IsRequired()
                    .HasColumnType("character varying");

                entity.Property(e => e.Logo).HasColumnType("character varying");

                entity.Property(e => e.MerchantId)
                    .HasColumnType("character varying")
                    .HasComment("User id của merchant quản lý brand này");

                entity.Property(e => e.Name).IsRequired();

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasColumnType("character varying");

                entity.Property(e => e.SeoImage).HasColumnType("character varying");

                entity.Property(e => e.SeoTitle).HasColumnType("character varying");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasColumnType("character varying");

                entity.Property(e => e.UpdateBy).HasColumnType("character varying");

                entity.Property(e => e.UpdatedDate).HasColumnType("timestamp(3) without time zone");

                entity.Property(e => e.Uri)
                    .IsRequired()
                    .HasColumnType("character varying");
            });

            modelBuilder.Entity<OperatorRoles>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId })
                    .HasName("UserRoles_pkey");

                entity.Property(e => e.UserId).HasMaxLength(200);

                entity.Property(e => e.RoleId).HasMaxLength(200);
            });

            modelBuilder.Entity<Operators>(entity =>
            {
                entity.Property(e => e.Id).HasMaxLength(200);

                entity.Property(e => e.CreatedDate).HasColumnType("timestamp(3) without time zone");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.FullName).IsRequired();

                entity.Property(e => e.LastLogin).HasColumnType("timestamp(3) without time zone");

                entity.Property(e => e.LoginFailedCount).HasColumnType("numeric");

                entity.Property(e => e.PasswordHash).IsRequired();

                entity.Property(e => e.Status).HasMaxLength(50);
            });

            modelBuilder.Entity<Roles>(entity =>
            {
                entity.Property(e => e.Id).HasMaxLength(200);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(256);
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.Property(e => e.Id).HasMaxLength(200);

                entity.Property(e => e.Address).HasMaxLength(500);

                entity.Property(e => e.Avatar).HasMaxLength(300);

                entity.Property(e => e.Birthday).HasColumnType("timestamp(3) without time zone");

                entity.Property(e => e.CreatedDate).HasColumnType("timestamp(3) without time zone");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.FullName).IsRequired();

                entity.Property(e => e.Gender).HasMaxLength(10);

                entity.Property(e => e.LastLogin).HasColumnType("timestamp(3) without time zone");

                entity.Property(e => e.LoginFailedCount).HasColumnType("numeric");

                entity.Property(e => e.PasswordHash).IsRequired();

                entity.Property(e => e.Qrcode)
                    .HasColumnName("QRCode")
                    .HasMaxLength(100);

                entity.Property(e => e.Status).HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}