using DAL;
using Microsoft.EntityFrameworkCore;
using Models;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace ConsoleApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            ChangeTracker();
            ConcurrencyToken();
            Transactions();

            Order order;

            using (Context context = GetContext())
            {
                //EagerLoading - ładowanie wbudowane w zapytanie do bazy danych
                //order = context.Set<Order>().Include(x => x.Products).First();


                //Aby skorzystać z wartości "Field-only property" możemy użyć klasy EF, ale tylko w zapytaniach Linq To Sql
                order = context.Set<Order>()
                    .Where(x => !EF.Property<bool>(x, "IsDeleted"))
                    .Where(x => x.Products.Any(xx => EF.Property<int>(xx, "OrderId") >= 4))
                    .Where(x => x.Products.Any(xx => EF.Property<int>(xx, "_daysToExpire") <= 5) ).First();

                // W standardowym zapytaniu Linq otrzymamy błąd
                //var products = order.Products.Where(x => EF.Property<int>(x, "_daysToExpire") <= 5).ToList();

                //ExplicitLoading - ładowanie w późniejszym czasie (na żądanie)
                //context.Entry(order).Collection(x => x.Products).Load();

                //ładowanie całej tabeli do kontekstu
                //context.Set<Product>().Load();

                //LazyLoading - ładowanie z opóźnieniem (w momencie użycia relacji)
                Console.WriteLine(JsonConvert.SerializeObject(order, Formatting.Indented));
            }



        }

        private static void Transactions()
        {
            using (Context context = GetContext())
            {
                Product product = context.Set<Product>().Find(3);

                using (Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {

                        product.Name = "Sałata";
                        context.SaveChanges();

                        transaction.CreateSavepoint("Savepoint1");

                        product.Name = "Kapusta";
                        context.SaveChanges();

                        if (new Random().Next(0, 10) % 2 == 0)
                        {
                            throw new Exception();
                        }

                        product.Price = 13.2f;
                        context.SaveChanges();

                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.RollbackToSavepoint("Savepoint1");

                        transaction.Commit();
                    }
                }
            }
        }

        private static void ConcurrencyToken()
        {
            using (Context context = GetContext())
            {

                Product product = context.Set<Product>().Find(3);

                product.Price = 3.2f;
                DetectChanges(context);
                bool saved = false;
                while (!saved)
                {
                    try
                    {
                        context.SaveChanges();
                        saved = true;
                    }
                    catch (DbUpdateConcurrencyException e)
                    {
                        foreach (Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry entry in e.Entries)
                        {
                            if (entry.Entity is Product)
                            {
                                //Wartości jakie my wprowadziliśmy do encji (stan jaki próbowaliśmy zapisać)
                                Microsoft.EntityFrameworkCore.ChangeTracking.PropertyValues currentValues = entry.CurrentValues;
                                //Pobieramy wartości jakie są aktualnie w bazie danych
                                Microsoft.EntityFrameworkCore.ChangeTracking.PropertyValues databaseValues = entry.GetDatabaseValues();

                                foreach (Microsoft.EntityFrameworkCore.Metadata.IProperty property in currentValues.Properties)
                                {
                                    object currentValue = currentValues[property];
                                    object databaseValue = databaseValues[property];

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
            using (Context context = GetContext())
            {
                context.Database.EnsureDeleted();
                context.Database.Migrate();

                for (int i = 0; i < 5; i++)
                {
                    Order order = new Order
                    {
                        DateTime = DateTime.Now
                    };

                    context.Add(order);

                    for (int ii = 0; ii < 5; ii++)
                    {
                        Product product = new Product();
                            product.Name = $"Product {i}";

                        order.Products.Add(product);
                    }

                }

                DetectChanges(context);

                context.SaveChanges();

                DetectChanges(context);

                Order localOrder = context.Set<Order>().Local.First();
                localOrder.Products.ToList().ForEach(x => x.Price = 2.2f);

                foreach (Product localOrderProduct in localOrder.Products.Take(2))
                {
                    Console.WriteLine($"{localOrderProduct.Name} - {localOrderProduct.Price}");

                    context.Entry(localOrderProduct).State = EntityState.Unchanged;

                    Console.WriteLine($"{localOrderProduct.Name} - {localOrderProduct.Price}");

                }

                foreach (Product localOrderProduct in localOrder.Products.Skip(2).Take(1))
                {
                    localOrderProduct.Name = $"{localOrderProduct.Name} - {localOrderProduct.Price}";

                    context.Entry(localOrderProduct).Member(nameof(localOrderProduct.Price)).IsModified = false;
                }

                DetectChanges(context);
                //context.ChangeTracker.Clear();

                context.SaveChanges();
            }

            using (Context context = GetContext())
            {
                System.Collections.Generic.List<Product> products = context.Set<Product>().AsNoTracking().Take(3).ToList();

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
            return new Context(new DbContextOptionsBuilder()
                //LazyLoading - włączenie proxy
                //.UseLazyLoadingProxies()
                .UseSqlServer(Context.ConnectionString).Options);
        }
    }
}
