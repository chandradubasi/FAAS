using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace AIN.FAAS.Repository.Models
{
    public partial class AINDatabaseContext : DbContext
    {
        public AINDatabaseContext()
        {
        }

        public AINDatabaseContext(DbContextOptions<AINDatabaseContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Hospital> Hospital { get; set; }
        public virtual DbSet<InventoryItem> InventoryItem { get; set; }
        public virtual DbSet<Lab> Lab { get; set; }
        public virtual DbSet<Location> Location { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<Site> Site { get; set; }
        public virtual DbSet<Storage> Storage { get; set; }
        public virtual DbSet<TransferRequest> TransferRequest { get; set; }
        public virtual DbSet<Vendor> Vendor { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=MD3CY9YC;Database=AINDatabase;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Hospital>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Name).HasMaxLength(150);
            });

            modelBuilder.Entity<InventoryItem>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ExpiryDate).HasColumnType("datetime");

                entity.Property(e => e.Remarks).HasMaxLength(200);

                entity.Property(e => e.Sgtin)
                    .HasColumnName("SGTIN")
                    .HasMaxLength(150);

                entity.Property(e => e.Status)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.InventoryItem)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK__Inventory__Produ__3F466844");

                entity.HasOne(d => d.Storage)
                    .WithMany(p => p.InventoryItem)
                    .HasForeignKey(d => d.StorageId)
                    .HasConstraintName("FK__Inventory__Stora__3E52440B");
            });

            modelBuilder.Entity<Lab>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Name).HasMaxLength(150);

                entity.HasOne(d => d.Site)
                    .WithMany(p => p.Lab)
                    .HasForeignKey(d => d.SiteId)
                    .HasConstraintName("FK__Lab__SiteId__2C3393D0");
            });

            modelBuilder.Entity<Location>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Name).HasMaxLength(150);

                entity.HasOne(d => d.Lab)
                    .WithMany(p => p.Location)
                    .HasForeignKey(d => d.LabId)
                    .HasConstraintName("FK__Location__LabId__300424B4");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.CatalogNumber).HasMaxLength(120);

                entity.Property(e => e.Gtin)
                    .HasColumnName("GTIN")
                    .HasMaxLength(150);

                entity.Property(e => e.Name).HasMaxLength(150);

                entity.HasOne(d => d.Vendor)
                    .WithMany(p => p.Product)
                    .HasForeignKey(d => d.VendorId)
                    .HasConstraintName("FK__Product__VendorI__3A81B327");
            });

            modelBuilder.Entity<Site>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Name).HasMaxLength(150);

                entity.HasOne(d => d.Hospital)
                    .WithMany(p => p.Site)
                    .HasForeignKey(d => d.HospitalId)
                    .HasConstraintName("FK__Site__HospitalId__286302EC");
            });

            modelBuilder.Entity<Storage>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Name).HasMaxLength(150);

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.Storage)
                    .HasForeignKey(d => d.LocationId)
                    .HasConstraintName("FK__Storage__Locatio__33D4B598");
            });

            modelBuilder.Entity<TransferRequest>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Approver).HasMaxLength(200);

                entity.Property(e => e.Comments).HasMaxLength(250);

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.RequestedDate).HasColumnType("datetime");

                entity.Property(e => e.Requester).HasMaxLength(200);

                entity.Property(e => e.Status).HasMaxLength(200);

                entity.HasOne(d => d.Destination)
                    .WithMany(p => p.TransferRequestDestination)
                    .HasForeignKey(d => d.DestinationId)
                    .HasConstraintName("FK__TransferR__Desti__60A75C0F");

                entity.HasOne(d => d.ItemsNavigation)
                    .WithMany(p => p.TransferRequest)
                    .HasForeignKey(d => d.Items)
                    .HasConstraintName("FK__TransferR__Items__5EBF139D");

                entity.HasOne(d => d.Source)
                    .WithMany(p => p.TransferRequestSource)
                    .HasForeignKey(d => d.SourceId)
                    .HasConstraintName("FK__TransferR__Sourc__5FB337D6");
            });

            modelBuilder.Entity<Vendor>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Address)
                    .HasColumnName("address")
                    .HasMaxLength(250);

                entity.Property(e => e.Name).HasMaxLength(150);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
