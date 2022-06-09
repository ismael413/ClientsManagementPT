using CM.DominioApi.Port.Models.Addreses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CM.Persistence.Adapter.Configuration
{
    public class CountryConfiguration : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            builder.Property(x => x.Name).IsRequired().HasMaxLength(50);

            //relaciones
            builder
                .HasMany(x => x.Cities)
                .WithOne(x => x.Country)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
