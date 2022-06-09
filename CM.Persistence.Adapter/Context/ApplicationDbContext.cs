using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using CM.DominioApi.Port.Models;
using CM.DominioApi.Port.Models.Addreses;
using Microsoft.EntityFrameworkCore;
using CM.Persistence.Adapter.Configuration;

namespace CM.Persistence.Adapter.Context
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseInMemoryDatabase("ClientsAddressManagementDB");
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new ClientConfiguration());
            builder.ApplyConfiguration(new AddressConfiguration());

        }

        public DbSet<Enterprise> Enterprises { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<City> Cities { get; set; }

    }
}
