using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace TTBackEndApi.Models.DataContext
{
    public partial class sw_insideContext : DbContext
    {
        public sw_insideContext()
        {
        }

        public sw_insideContext(DbContextOptions<sw_insideContext> options)
            : base(options)
        {
        }

        public virtual DbSet<IswRequestHistory> IswRequestHistory { get; set; }
        public virtual DbSet<IswRequests> IswRequests { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseNpgsql("Host=13.229.0.235;Port=4432;Database=sw_inside;Username=u_inside;Password=7mjJGj7dc7waQah");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IswRequestHistory>(entity =>
            {
                entity.HasKey(e => e.HistoryId)
                    .HasName("ISW_REQUEST_HISTORY_pkey");

                entity.ToTable("ISW_REQUEST_HISTORY");

                entity.Property(e => e.HistoryId).UseIdentityAlwaysColumn();

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.CreatedDate).HasColumnType("timestamp(6) without time zone");

                entity.Property(e => e.Ip)
                    .HasColumnName("IP")
                    .HasMaxLength(50);

                entity.Property(e => e.Reason).HasMaxLength(300);

                entity.Property(e => e.RequestId)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.RequestStatus)
                    .IsRequired()
                    .HasMaxLength(15);

                entity.Property(e => e.TransType)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<IswRequests>(entity =>
            {
                entity.HasKey(e => e.RequestId)
                    .HasName("ISW_REQUESTS_pkey");

                entity.ToTable("ISW_REQUESTS");

                entity.Property(e => e.RequestId).HasMaxLength(100);

                entity.Property(e => e.Amount).HasColumnType("numeric(12,0)");

                entity.Property(e => e.CollectId).HasMaxLength(100);

                entity.Property(e => e.CollectType).HasMaxLength(50);

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasComment("Operator thực hiện tạo request");

                entity.Property(e => e.CreatedDate).HasColumnType("timestamp(6) without time zone");

                entity.Property(e => e.Description).HasMaxLength(300);

                entity.Property(e => e.FromId).HasMaxLength(100);

                entity.Property(e => e.HoldTime).HasComment("Số ngày tạm giữ(Hold time)");

                entity.Property(e => e.OrderId).HasMaxLength(100);

                entity.Property(e => e.PaymentId).HasMaxLength(100);

                entity.Property(e => e.Reason).HasMaxLength(300);

                entity.Property(e => e.RequestType)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasComment("Topup, Transfer ...");

                entity.Property(e => e.Status).HasMaxLength(20);

                entity.Property(e => e.ToId).HasMaxLength(100);

                entity.Property(e => e.TransactionId).HasMaxLength(100);

                entity.Property(e => e.VerifiedBy)
                    .HasMaxLength(20)
                    .HasComment("Operator checker");

                entity.Property(e => e.VerifiedDate).HasColumnType("timestamp(6) without time zone");
            });

            modelBuilder.HasSequence("ISW_REQUEST_HISTORY_HistoryId_seq");

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
