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
    public class OrderSummaryConfiguration : IEntityTypeConfiguration<OrderSummary>
    {
        public virtual void Configure(EntityTypeBuilder<OrderSummary> builder)
        {
            //Nie tworzymy tabeli dla tej klasy
            builder.ToTable(null);
        }


    }
}
