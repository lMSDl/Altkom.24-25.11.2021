﻿using Microsoft.EntityFrameworkCore;
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

            builder.Property(x => x.DisplayName)
                .HasComputedColumnSql("[Name] + ' ' + CONVERT(varchar, [Price]) + 'zł'", true);
            builder
                //.Property(x => x.DaysToExpire)
                //Możemy utworzyć "Field-only property" poprzez podanie nazwy pola
                .Property("_daysToExpire")
                .HasColumnName("DaysToExpire")
                .HasComputedColumnSql($"DATEDIFF(DAY, GETDATE(), [{nameof(Product.ExpirationDate)}])");

            //Domyślne ustawienie dla EF Core 5 to PreferField
            builder.Property(x => x.DisplayName).UsePropertyAccessMode(PropertyAccessMode.PreferProperty);


            builder.HasQueryFilter(x => !EF.Property<bool>(x, "IsDeleted") && EF.Property<int>(x, "_daysToExpire") >= 0);
        }
    }
}
