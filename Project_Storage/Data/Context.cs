using Microsoft.EntityFrameworkCore;
using Project_Storage.Entities;

namespace Project_Storage.Data
{
    public class Context : DbContext
    {
        public DbSet<Items> Items { get; set; }
        public DbSet<Storages> Storages { get; set; }
        public DbSet<Clients> Clients { get; set; }
        public DbSet<Transfers> Transfers { get; set; }
        public DbSet<Stored> Stored { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(
                    "Server=MARO\\SQLEXPRESS;Database=Storage;Integrated Security=True;TrustServerCertificate=True;");
            }
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
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Stored>()
                .HasOne(s => s.Item)
                .WithMany(i => i.StoredEntries)
                .HasForeignKey(s => s.ItemName)
                .OnDelete(DeleteBehavior.Cascade);

            // Transfers relations
            modelBuilder.Entity<Transfers>(entity =>
            {
                entity.HasKey(t => t.TransferId);

                entity.Property(t => t.Type).HasMaxLength(20);

                entity.HasOne(t => t.Client)
                    .WithMany(c => c.Transfers)
                    .HasForeignKey(t => t.ClientName)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(t => t.ExporterStorage)
                    .WithMany(s => s.ExportTransfers)
                    .HasForeignKey(t => t.ExporterStorageName)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(t => t.ImporterStorage)
                    .WithMany(s => s.ImportTransfers)
                    .HasForeignKey(t => t.ImporterStorageName)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(t => t.Item)
                    .WithMany(i => i.Transfers)
                    .HasForeignKey(t => t.ItemName)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
