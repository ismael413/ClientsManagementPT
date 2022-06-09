using CM.DominioApi.Port.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CM.Persistence.Adapter.Configuration
{
    public class EnterpriseConfiguration : IEntityTypeConfiguration<Enterprise>
    {
        public void Configure(EntityTypeBuilder<Enterprise> builder)
        {
            builder.Property(x => x.Name).IsRequired().HasMaxLength(50);

            builder
                .HasMany(x => x.Clients)
                .WithOne(x => x.Enterprise)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
