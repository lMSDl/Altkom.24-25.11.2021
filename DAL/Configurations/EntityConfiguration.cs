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
    public class EntityConfiguration<T> : IEntityTypeConfiguration<T> where T : Entity
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {

            builder.Property(x => x.Created).HasDefaultValueSql("getdate()");
            //Generowanie wartości wbudowane w bazę danych (MS SQL nie posiada wsparcia do tego)
            //builder.Property(x => x.Updated).ValueGeneratedOnAddOrUpdate();
        }


    }
}
