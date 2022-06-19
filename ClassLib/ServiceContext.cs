using Microsoft.EntityFrameworkCore;

namespace ClassLib
{
    public class ServiceContext : DbContext
    {
        public DbSet<Car> cars { get; set; }
        public DbSet<Rent> rents { get; set; }
        public DbSet<User> users { get; set; }

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
            modelBuilder.Entity<Rent>()
                .HasOne(p => p.car)
                .WithMany(t => t.rents)
                .HasForeignKey(p => p.car_id)
                .HasPrincipalKey(t=>t.id);

            modelBuilder.Entity<Rent>()
                .HasOne(p => p.client)
                .WithMany(t => t.rents)
                .HasForeignKey(p => p.client_id)
                .HasPrincipalKey(t=>t.id);
        }
    }
}