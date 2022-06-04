using Microsoft.EntityFrameworkCore;

namespace Classlib
{
    public class ServiceContext : DbContext
    {
        public DbSet<Car> cars { get; set; }
        public DbSet<Engine> engines { get; set; }
        public DbSet<Rent> rents { get; set; }
        public DbSet<Client> clients { get; set; }
        public DbSet<Worker> workers { get; set; }
        public string connectionString { get; set; }
        public ServiceContext(string connectionString)
        {
            this.connectionString = connectionString;
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql(connectionString);
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Car>()
                .HasOne(p => p.client)
                .WithMany(t => t.reviews)
                .HasForeignKey(p => p.client_id)
                .HasPrincipalKey(t=>t.id);

            modelBuilder.Entity<Review>()
                .HasOne(p => p.garment)
                .WithMany(t => t.reviews)
                .HasForeignKey(p => p.garment_id)
                .HasPrincipalKey(t=>t.id);

            modelBuilder.Entity<Order>()
                .HasOne(p => p.client)
                .WithMany(t => t.orders)
                .HasForeignKey(p => p.client_id)
                .HasPrincipalKey(t=>t.id);

            modelBuilder.Entity<Garment>()
                .HasMany(c => c.orders)
                .WithMany(s => s.garments)
                .UsingEntity<OrderItem>(
                    j => j
                    .HasOne(pt => pt.order)
                    .WithMany(t => t.order_items)
                    .HasForeignKey(pt => pt.order_id),
                    j => j
                    .HasOne(pt => pt.garment)
                    .WithMany(p => p.order_items)
                    .HasForeignKey(pt => pt.garment_id));
        }
    }
}