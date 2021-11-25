using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Configurations
{
    public class OrderConfiguration : EntityConfiguration<Order>
    {
        public override void Configure(EntityTypeBuilder<Order> builder)
        {
            base.Configure(builder);

            //Konwertery

            //builder.Property(x => x.OrderType).HasConversion<string>();
            //builder.Property(x => x.OrderType).HasConversion(
            //    x => x.ToString(),
            //    x => (OrderTypes)Enum.Parse(typeof(OrderTypes), x));

            builder.Property(x => x.OrderType).HasConversion(Converters.ToBase64Converter<OrderTypes>());

            //builder.Property(x => x.DeliveryLocation)
            //    .HasConversion(
            //    x => new Point(x.Longitude, x.Latitude) { SRID = 4326 },
            //    x => new Models.Location { Longitude = (float)x.X, Latitude = (float)x.Y });
        }


    }
}
