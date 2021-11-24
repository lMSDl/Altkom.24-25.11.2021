using DAL;
using Microsoft.EntityFrameworkCore;
using Models;
using System.Linq;
using System;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            ChangeTracker();


            using (var context = GetContext())
            {
                
                    var product = context.Set<Product>().Find(3);

                    product.Price = 3.2f;
                    DetectChanges(context);
                var saved = false;
                while (!saved)
                {
                    try
                    {
                        context.SaveChanges();
                        saved = true;
                    }
                    catch (DbUpdateConcurrencyException e)
                    {
                        foreach (var entry in e.Entries)
                        {
                            if (entry.Entity is Product)
                            {
                                //Wartości jakie my wprowadziliśmy do encji (stan jaki próbowaliśmy zapisać)
                                var currentValues = entry.CurrentValues;
                                //Pobieramy wartości jakie są aktualnie w bazie danych
                                var databaseValues = entry.GetDatabaseValues();

                                foreach (var property in currentValues.Properties)
                                {
                                    var currentValue = currentValues[property];
                                    var databaseValue = databaseValues[property];

                                    //Przypisanie wartości, która ma się znaleźć w bazie danych
                                    currentValues[property] = databaseValue;
                                }

                                DetectChanges(context);
                                //Synchronizacja wartości początkowych (które modyfikowaliśmy na początku) do aktualnych wartości z bazy danych
                                //w celu aktualizacji tokena współbieżności
                                entry.OriginalValues.SetValues(databaseValues);
                                DetectChanges(context);
                            }
                            else if (entry.Entity is Order)
                            {
                                //....
                            }
                        }
                    }
                }
            }

        }

        private static void ChangeTracker()
        {
            using (var context = GetContext())
            {
                context.Database.EnsureDeleted();
                context.Database.Migrate();

                for (int i = 0; i < 5; i++)
                {
                    var order = new Order();
                    order.DateTime = DateTime.Now;

                    context.Add(order);

                    for (int ii = 0; ii < 5; ii++)
                    {
                        var product = new Product();
                        if (i % 2 == 0)
                            product.Name = $"Product {i}";
                        order.Products.Add(product);
                    }

                }

                DetectChanges(context);

                context.SaveChanges();

                DetectChanges(context);

                var localOrder = context.Set<Order>().Local.First();
                localOrder.Products.ToList().ForEach(x => x.Price = 2.2f);

                foreach (var localOrderProduct in localOrder.Products.Take(2))
                {
                    Console.WriteLine($"{localOrderProduct.Name} - {localOrderProduct.Price}");

                    context.Entry(localOrderProduct).State = EntityState.Unchanged;

                    Console.WriteLine($"{localOrderProduct.Name} - {localOrderProduct.Price}");

                }

                foreach (var localOrderProduct in localOrder.Products.Skip(2).Take(1))
                {
                    localOrderProduct.Name = $"{localOrderProduct.Name} - {localOrderProduct.Price}";

                    context.Entry(localOrderProduct).Member(nameof(localOrderProduct.Price)).IsModified = false;
                }

                DetectChanges(context);
                //context.ChangeTracker.Clear();

                context.SaveChanges();
            }

            using (var context = GetContext())
            {
                var products = context.Set<Product>().AsNoTracking().Take(3).ToList();

                DetectChanges(context);
            }
        }

        private static void DetectChanges(Context context)
        {
            context.ChangeTracker.DetectChanges();
            Console.WriteLine(context.ChangeTracker.DebugView.ShortView);
            Console.WriteLine("--------------------------------------");
            Console.WriteLine(context.ChangeTracker.DebugView.LongView);
            Console.WriteLine("--------------------------------------");
        }

        private static Context GetContext()
        {
            return new Context(new DbContextOptionsBuilder().UseSqlServer(Context.ConnectionString).Options);
        }
    }
}
