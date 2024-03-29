﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Tastee.Infrastucture.Data.Context
{
    public partial class tasteeContext : DbContext
    {
        public tasteeContext()
        {
        }

        public tasteeContext(DbContextOptions<tasteeContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Areas> Areas { get; set; }
        public virtual DbSet<Banners> Banners { get; set; }
        public virtual DbSet<BrandDecorations> BrandDecorations { get; set; }
        public virtual DbSet<BrandImages> BrandImages { get; set; }
        public virtual DbSet<BrandMerchants> BrandMerchants { get; set; }
        public virtual DbSet<Brands> Brands { get; set; }
        public virtual DbSet<Categories> Categories { get; set; }
        public virtual DbSet<Cities> Cities { get; set; }
        public virtual DbSet<CollectionBrands> CollectionBrands { get; set; }
        public virtual DbSet<Collections> Collections { get; set; }
        public virtual DbSet<Customers> Customers { get; set; }
        public virtual DbSet<DeviceTokens> DeviceTokens { get; set; }
        public virtual DbSet<GroupItemMapping> GroupItemMapping { get; set; }
        public virtual DbSet<GroupItems> GroupItems { get; set; }
        public virtual DbSet<GroupToppings> GroupToppings { get; set; }
        public virtual DbSet<MenuItemToppingMappings> MenuItemToppingMappings { get; set; }
        public virtual DbSet<MenuItems> MenuItems { get; set; }
        public virtual DbSet<Menus> Menus { get; set; }
        public virtual DbSet<Nlogs> Nlogs { get; set; }
        public virtual DbSet<NotificationMapping> NotificationMapping { get; set; }
        public virtual DbSet<Notifications> Notifications { get; set; }
        public virtual DbSet<OperatorRoles> OperatorRoles { get; set; }
        public virtual DbSet<Operators> Operators { get; set; }
        public virtual DbSet<ProductSliders> ProductSliders { get; set; }
        public virtual DbSet<Roles> Roles { get; set; }
        public virtual DbSet<ShippingAddresses> ShippingAddresses { get; set; }
        public virtual DbSet<SuggestBrands> SuggestBrands { get; set; }
        public virtual DbSet<Toppings> Toppings { get; set; }
        public virtual DbSet<UserTokens> UserTokens { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<VerifySms> VerifySms { get; set; }
        public virtual DbSet<Videos> Videos { get; set; }
        public virtual DbSet<WidgetImages> WidgetImages { get; set; }
        public virtual DbSet<Widgets> Widgets { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Data Source=DESKTOP-1T5J9OI;Initial Catalog=tastee;Persist Security Info=True;User ID=sa;Password=123456");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Areas>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200);
            });

            modelBuilder.Entity<Banners>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.BrandId)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Image)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.IsDisplay)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Link).IsRequired();

                entity.Property(e => e.Name).IsRequired();

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Title).HasMaxLength(200);

                entity.Property(e => e.UpdateBy).HasMaxLength(50);
            });

            modelBuilder.Entity<BrandDecorations>(entity =>
            {
                entity.Property(e => e.Id).HasMaxLength(50);

                entity.Property(e => e.BrandId)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.UpdatedBy).HasMaxLength(50);
            });

            modelBuilder.Entity<BrandImages>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.BrandId)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Image)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.UpdatedBy).HasMaxLength(50);
            });

            modelBuilder.Entity<BrandMerchants>(entity =>
            {
                entity.Property(e => e.Id).HasMaxLength(50);

                entity.Property(e => e.BrandId)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Brands>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Address).IsRequired();

                entity.Property(e => e.Area).HasMaxLength(100);

                entity.Property(e => e.BusinessType).HasDefaultValueSql("((1))");

                entity.Property(e => e.Categories).HasMaxLength(100);

                entity.Property(e => e.City).HasMaxLength(100);

                entity.Property(e => e.CloseTimeA).HasColumnName("CloseTime_A");

                entity.Property(e => e.CloseTimeP).HasColumnName("CloseTime_P");

                entity.Property(e => e.CreatedDate).HasColumnType("smalldatetime");

                entity.Property(e => e.Cuisines).HasMaxLength(100);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.EndDate).HasColumnType("smalldatetime");

                entity.Property(e => e.HeadOffice)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Hotline)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Logo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.MerchantId)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Name).IsRequired();

                entity.Property(e => e.OpenTimeA).HasColumnName("OpenTime_A");

                entity.Property(e => e.OpenTimeP).HasColumnName("OpenTime_P");

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.RefCode).HasMaxLength(50);

                entity.Property(e => e.RestaurantImages).HasMaxLength(200);

                entity.Property(e => e.SeoImage)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.SeoTitle).HasMaxLength(100);

                entity.Property(e => e.StartDate).HasColumnType("smalldatetime");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Tax).HasMaxLength(200);

                entity.Property(e => e.UpdateBy).HasMaxLength(50);

                entity.Property(e => e.UpdatedDate).HasColumnType("smalldatetime");

                entity.Property(e => e.Uri)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.WebMap).HasMaxLength(200);
            });

            modelBuilder.Entity<Categories>(entity =>
            {
                entity.Property(e => e.Id).HasMaxLength(50);

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Image).HasMaxLength(200);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.UpdatedBy).HasMaxLength(50);
            });

            modelBuilder.Entity<Cities>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200);
            });

            modelBuilder.Entity<CollectionBrands>(entity =>
            {
                entity.Property(e => e.Id).HasMaxLength(50);

                entity.Property(e => e.BrandId)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.CollectionId)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Collections>(entity =>
            {
                entity.Property(e => e.Id).HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Uid)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Customers>(entity =>
            {
                entity.Property(e => e.Id).HasMaxLength(50);

                entity.Property(e => e.Address).HasMaxLength(200);

                entity.Property(e => e.BrandId).HasMaxLength(50);

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.Gender)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Note).HasMaxLength(500);

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasMaxLength(15);

                entity.Property(e => e.Type).HasMaxLength(50);

                entity.Property(e => e.UpdatedBy).HasMaxLength(50);
            });

            modelBuilder.Entity<DeviceTokens>(entity =>
            {
                entity.HasIndex(e => new { e.UserId, e.AllowPush })
                    .HasName("NonClusteredIndex-20220626-163928");

                entity.Property(e => e.AllowPush)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.DeviceToken)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<GroupItemMapping>(entity =>
            {
                entity.HasKey(e => new { e.GroupId, e.ItemId })
                    .HasName("PK_GroupItemMapping_1");

                entity.Property(e => e.GroupId).HasMaxLength(50);

                entity.Property(e => e.ItemId).HasMaxLength(50);

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<GroupItems>(entity =>
            {
                entity.Property(e => e.Id).HasMaxLength(50);

                entity.Property(e => e.BrandId)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.UpdatedBy).HasMaxLength(50);
            });

            modelBuilder.Entity<GroupToppings>(entity =>
            {
                entity.Property(e => e.Id).HasMaxLength(50);

                entity.Property(e => e.BrandId)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.MenuItemId).HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.UpdatedBy).HasMaxLength(50);
            });

            modelBuilder.Entity<MenuItemToppingMappings>(entity =>
            {
                entity.Property(e => e.Id).HasMaxLength(50);

                entity.Property(e => e.MenuItemId)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.RefId)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<MenuItems>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Barcode).HasMaxLength(15);

                entity.Property(e => e.BrandId).HasMaxLength(50);

                entity.Property(e => e.CapitalPrice).HasColumnType("money");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Image).HasMaxLength(200);

                entity.Property(e => e.MenuId)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Price).HasColumnType("money");

                entity.Property(e => e.ProductCode).HasMaxLength(50);

                entity.Property(e => e.PromotionPrice).HasColumnType("money");

                entity.Property(e => e.ShortDescription).HasMaxLength(500);

                entity.Property(e => e.Unit).HasMaxLength(50);

                entity.Property(e => e.UpdatedBy).HasMaxLength(50);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.Property(e => e.WholesalePrice).HasColumnType("money");
            });

            modelBuilder.Entity<Menus>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.BrandId)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.UpdatedBy).HasMaxLength(50);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<Nlogs>(entity =>
            {
                entity.ToTable("NLogs");

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

            modelBuilder.Entity<NotificationMapping>(entity =>
            {
                entity.HasIndex(e => new { e.NotificationId, e.UserId, e.BrandId, e.Status })
                    .HasName("idx_notidication_id_ref_id");

                entity.Property(e => e.BrandId)
                    .HasMaxLength(50);

                entity.Property(e => e.NotificationId)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Notifications>(entity =>
            {
                entity.Property(e => e.Id).HasMaxLength(50);

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Description).IsRequired();

                entity.Property(e => e.Image).HasMaxLength(200);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(500);
            });

            modelBuilder.Entity<OperatorRoles>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId });

                entity.Property(e => e.UserId)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RoleId)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Operators>(entity =>
            {
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

            modelBuilder.Entity<ProductSliders>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.BrandId)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.CreatedDate).HasColumnType("smalldatetime");

                entity.Property(e => e.EndDate).HasColumnType("smalldatetime");

                entity.Property(e => e.Image)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.StartDate).HasColumnType("smalldatetime");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.UpdateBy).HasMaxLength(50);

                entity.Property(e => e.UpdateDate).HasColumnType("smalldatetime");
            });

            modelBuilder.Entity<Roles>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.Id)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<ShippingAddresses>(entity =>
            {
                entity.Property(e => e.Id).HasMaxLength(50);

                entity.Property(e => e.Address).IsRequired();

                entity.Property(e => e.Area)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.City)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<SuggestBrands>(entity =>
            {
                entity.Property(e => e.Id).HasMaxLength(50);

                entity.Property(e => e.Address).IsRequired();

                entity.Property(e => e.Area)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.City)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Description).IsRequired();

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Name).IsRequired();

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.RestaurantImages)
                    .IsRequired()
                    .HasMaxLength(200);
            });

            modelBuilder.Entity<Toppings>(entity =>
            {
                entity.Property(e => e.Id).HasMaxLength(50);

                entity.Property(e => e.BrandId).HasMaxLength(50);

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.GroupToppingId).HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Note).HasMaxLength(200);

                entity.Property(e => e.UpdatedBy).HasMaxLength(50);
            });

            modelBuilder.Entity<UserTokens>(entity =>
            {
                entity.Property(e => e.Token)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.Uid)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(50)
                    .IsUnicode(false);

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

                entity.Property(e => e.RefCode).HasMaxLength(11);

                entity.Property(e => e.RefId).HasMaxLength(50);

                entity.Property(e => e.Status)
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<VerifySms>(entity =>
            {
                entity.ToTable("VerifySMS");

                entity.Property(e => e.Id).HasMaxLength(50);

                entity.Property(e => e.PhoneNumber)
                    .IsRequired()
                    .HasMaxLength(11)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.VerifyCode)
                    .IsRequired()
                    .HasMaxLength(6);
            });

            modelBuilder.Entity<Videos>(entity =>
            {
                entity.Property(e => e.Id).HasMaxLength(50);

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.Image).HasMaxLength(200);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.UpdatedBy).HasMaxLength(50);

                entity.Property(e => e.Video)
                    .IsRequired()
                    .HasMaxLength(200);
            });

            modelBuilder.Entity<WidgetImages>(entity =>
            {
                entity.Property(e => e.Id).HasMaxLength(50);

                entity.Property(e => e.Image)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.WidgetId)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Widgets>(entity =>
            {
                entity.Property(e => e.Id).HasMaxLength(50);

                entity.Property(e => e.DecorationId)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
