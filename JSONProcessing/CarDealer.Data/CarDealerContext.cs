namespace CarDealer.Data
{
    using Microsoft.EntityFrameworkCore;

    using Models;

    public class CarDealerContext : DbContext
    {
        public CarDealerContext()
        { }

        public CarDealerContext(DbContextOptions options) 
            : base(options)
        { }

        public DbSet<Car> Cars { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Part> Parts { get; set; }
        public DbSet<PartCar> PartsCars { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
                optionsBuilder.EnableSensitiveDataLogging().UseSqlServer(Configuration.ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<PartCar>(e => e
                .HasKey(a => new
                                {
                                    a.CarId,
                                    a.PartId
                                }));
        }
    }
}