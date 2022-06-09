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
    public class CityConfiguration : IEntityTypeConfiguration<City>
    {
        public void Configure(EntityTypeBuilder<City> builder)
        {
            builder.Property(x => x.Name).IsRequired().HasMaxLength(50);

            //relaciones
            builder
                .HasOne(x => x.Country)
                .WithMany(x => x.Cities)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
