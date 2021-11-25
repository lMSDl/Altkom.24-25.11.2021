using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Configurations
{
    public class ProductConfiguration : EntityConfiguration<Product>
    {
        public override void Configure(EntityTypeBuilder<Product> builder)
        {
            base.Configure(builder);

            builder.Property(x => x.Name).IsConcurrencyToken();
            builder.Property(x => x.Timestamp).IsRowVersion();

            //builder.Property(x => x.Price).HasDefaultValue(10);
            builder.Property(x => x.Price).HasDefaultValueSql("NEXT VALUE FOR sequences.ProductPrice");
        }
    }
}
