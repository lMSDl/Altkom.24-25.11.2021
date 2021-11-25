using DAL.Configurations;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DAL
{
    public class Context : DbContext
    {
        public static string ConnectionString { get; } = "Server=(local);Database=EFC5A2;Integrated Security=true";

        public static Func<Context, int, IEnumerable<Product>> GetProductsForOrder { get; } =
            EF.CompileQuery((Context context, int id) => context.Set<Product>().Where(x => EF.Property<int>(x, "OrderId") == id));

        public Context()
        {

        }

        public Context(DbContextOptions options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(ConnectionString, x=> x.UseNetTopologySuite());
            }

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.ApplyConfiguration(new ProductConfiguration());
            //modelBuilder.ApplyConfiguration(new OrderConfiguration());
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProductConfiguration).Assembly);



            //foreach (Microsoft.EntityFrameworkCore.Metadata.IMutableProperty property in modelBuilder.Model.GetEntityTypes()
            //    .SelectMany(x => x.GetProperties())
            //    .Where(x => x.PropertyInfo?.Name == "Key").ToList())
            //{
            //    property.DeclaringEntityType.SetPrimaryKey(property);
            //}


            foreach (Microsoft.EntityFrameworkCore.Metadata.IMutableProperty property in modelBuilder.Model.GetEntityTypes()
            .SelectMany(x => x.GetProperties())
            .Where(x => x.PropertyInfo?.PropertyType == typeof(string)))
            {
                property.SetMaxLength(50);
                property.IsNullable = false;
            }

            //foreach (Microsoft.EntityFrameworkCore.Metadata.IMutableProperty property in modelBuilder.Model.GetEntityTypes()
            //    .SelectMany(x => x.GetProperties())
            //    .Where(x => x.PropertyInfo?.PropertyType == typeof(DateTime)))
            //{
            //    property.SetColumnType("dateTime");
            //}

            //Zadeklarowanie nowej sekwencji
            modelBuilder.HasSequence<int>("ProductPrice", "sequences")
                .StartsAt(10)
                .HasMax(55)
                .HasMin(3)
                .IncrementsBy(4)
                .IsCyclic();
        }

        public override int SaveChanges()
        {
            var now = DateTime.Now;
            ChangeTracker.Entries<Entity>()
                .Where(x => x.State == EntityState.Modified)
                .Select(x => x.Entity)
                .ToList()
                .ForEach(x => x.Updated = now);

            return base.SaveChanges();
        }
    }
}
