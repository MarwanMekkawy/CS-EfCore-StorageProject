using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Project_Storage
{
    class Context : DbContext
    {

        public DbSet<Items> Items { get; set; }
        public DbSet<Storages> Storages { get; set; }
        public DbSet<Clients> Clients { get; set; }
        public DbSet<Transfers> Transfers { get; set; }
        public DbSet<Stored> Stored { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                "Server=MARO\\SQLEXPRESS;Database=Storage;Integrated Security=True;TrustServerCertificate=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Stored key 
            modelBuilder.Entity<Stored>()
                .HasKey(s => s.Id);

            // Stored relations
            modelBuilder.Entity<Stored>()
                .HasOne(s => s.Storage)
                .WithMany(st => st.StoredItems)
                .HasForeignKey(s => s.StorageName)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Stored>()
                .HasOne(s => s.Item)
                .WithMany(i => i.StoredEntries)
                .HasForeignKey(s => s.ItemName)
                .OnDelete(DeleteBehavior.Restrict);

            // Transfers relations 
            modelBuilder.Entity<Transfers>(entity =>
            {
                entity.HasKey(t => t.TransferId);

                entity.Property(t => t.Type)
                    .HasMaxLength(20)
                    .IsRequired(false);

                entity.Property(t => t.Move)
                    .IsRequired(false);

                entity.Property(t => t.TransferDate)
                    .IsRequired(false);

                entity.Property(t => t.ProductionDate)
                    .IsRequired(false);

                entity.Property(t => t.ExpiryDate)
                    .IsRequired(false);

                entity.Property(t => t.UnitCount)
                    .IsRequired(false);

                entity.Property(t => t.ClientName)
                    .HasMaxLength(100)
                    .IsRequired(false);

                entity.HasOne(t => t.Client)
                    .WithMany(c => c.Transfers)
                    .HasForeignKey(t => t.ClientName)
                    .IsRequired(false)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.Property(t => t.ExporterStorageName)
                    .HasMaxLength(100)
                    .IsRequired(false);

                entity.HasOne(t => t.ExporterStorage)
                    .WithMany(s => s.ExportTransfers)
                    .HasForeignKey(t => t.ExporterStorageName)
                    .IsRequired(false)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.Property(t => t.ImporterStorageName)
                    .HasMaxLength(100)
                    .IsRequired(false);

                entity.HasOne(t => t.ImporterStorage)
                    .WithMany(s => s.ImportTransfers)
                    .HasForeignKey(t => t.ImporterStorageName)
                    .IsRequired(false)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.Property(t => t.ItemName)
                    .HasMaxLength(100)
                    .IsRequired(false);

                entity.HasOne(t => t.Item)
                    .WithMany(i => i.Transfers)
                    .HasForeignKey(t => t.ItemName)
                    .IsRequired(false)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            base.OnModelCreating(modelBuilder);
        }



    }
}

