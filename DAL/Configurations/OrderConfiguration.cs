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
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {

            //Konwertery

            //builder.Property(x => x.OrderType).HasConversion<string>();
            //builder.Property(x => x.OrderType).HasConversion(
            //    x => x.ToString(),
            //    x => (OrderTypes)Enum.Parse(typeof(OrderTypes), x));

            builder.Property(x => x.OrderType).HasConversion(Converters.ToBase64Converter<OrderTypes>());

        }


    }
}
