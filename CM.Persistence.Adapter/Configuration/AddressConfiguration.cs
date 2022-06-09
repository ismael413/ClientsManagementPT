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
    public class AddressConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.Property(x => x.FullAddress).IsRequired().HasMaxLength(200);
            builder.Property(x => x.BuildingName).HasMaxLength(50);
            builder.Property(x => x.StreetName).HasMaxLength(50);

            //relaciones
            builder
                .HasOne(x => x.Client)
                .WithMany(x => x.Addresses)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
