using Microsoft.EntityFrameworkCore;

namespace StargateAPI.Business.Data
{
    public class PersonDbContext : DbContext
    {
        public PersonDbContext(DbContextOptions<PersonDbContext> options) : base(options) { }

        public DbSet<Person> Persons { get; set; }
        public DbSet<AstronautDetail> AstronautDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new PersonConfiguration());
            modelBuilder.ApplyConfiguration(new AstronautDetailConfiguration());
            // Apply other configurations if necessary
        }
    }
}
