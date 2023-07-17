using System;
using Microsoft.EntityFrameworkCore;
using Academits.Karetskas.WorkUnit.Model;
using Academits.Karetskas.WorkUnit.UnitOfWork;
using Academits.Karetskas.WorkUnit.Repositories.Interfaces;

namespace Academits.Karetskas.WorkUnit
{
    internal class Program
    {
        static void Main()
        {
            using var dbContext = new WorkUnitContext();

            using var workUnit = new UnitOfWorkGroceryStore(dbContext);

            RecreateDatabase(workUnit, dbContext);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("1. Найти самый часто покупаемый продукт");
            Console.WriteLine();

            try
            {
                workUnit.BeginTransaction();

                var popularProduct = workUnit.GetRepository<IProductRepository>().GetPopularProduct();

                workUnit.CommitTransaction();

                Console.WriteLine("Самыми часто покупаемыми продуктами являются:");

                foreach (var product in popularProduct)
                {
                    Console.WriteLine($"{product.Item1} = {product.Item2}");
                }
            }
            catch (Exception)
            {
                workUnit.RollbackTransaction();
            }

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("2. Найти сколько каждый клиент потратил денег за все время:");
            Console.WriteLine();

            var eachCustomerTotalCost = workUnit.GetRepository<ICustomerRepository>().GetExpensesForAllTime();

            foreach (var customer in eachCustomerTotalCost)
            {
                Console.WriteLine($"- {customer.Item1} {customer.Item2} {customer.Item3} потратил {customer.Item4:F2}");
            }

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("3. Вывести сколько товаров каждой категории купили:");
            Console.WriteLine();

            var productsCountByCategory = workUnit.GetRepository<ICategoryRepository>().GetProductsCountByCategory();

            foreach (var category in productsCountByCategory)
            {
                Console.WriteLine($"- категория: {category.Item1} = {category.Item2}");
            }
        }

        private static void RecreateDatabase(UnitOfWorkGroceryStore workUnit, DbContext context)
        {
            context.Database.EnsureDeleted();

            context.Database.EnsureCreated();

            var dairyProducts = new Category
            {
                Name = "Dairy products"
            };

            var meatAndPoultry = new Category
            {
                Name = "Meat and poultry"
            };

            var fruitsAndVegetables = new Category
            {
                Name = "Fruits and vegetables"
            };

            var grainsAndBakeryProducts = new Category
            {
                Name = "Grains and bakery products"
            };

            var fishAndSeafood = new Category
            {
                Name = "Fish and seafood"
            };


            workUnit.GetRepository<ICategoryRepository>().AddRange(dairyProducts,
                meatAndPoultry,
                fruitsAndVegetables,
                grainsAndBakeryProducts,
                fishAndSeafood);

            var yogurt = new Product
            {
                Name = "Yogurt",
                Price = 35.67m
            };

            var chickenBreast = new Product
            {
                Name = "Chicken breast",
                Price = 142.04m
            };

            var buckwheat = new Product
            {
                Name = "Buckwheat",
                Price = 38.00m
            };

            var tuna = new Product
            {
                Name = "Tuna",
                Price = 71.71m
            };

            var cheese = new Product
            {
                Name = "Cheese",
                Price = 59.18m
            };

            workUnit.GetRepository<IProductRepository>().AddRange(yogurt,
                chickenBreast,
                buckwheat,
                tuna,
                cheese);

            chickenBreast.Categories.Add(meatAndPoultry);
            buckwheat.Categories.Add(grainsAndBakeryProducts);
            yogurt.Categories.Add(fruitsAndVegetables);
            yogurt.Categories.Add(dairyProducts);
            tuna.Categories.Add(fishAndSeafood);
            cheese.Categories.Add(dairyProducts);

            var ivanovI = new Customer
            {
                FirstName = "Ivan",
                LastName = "Ivanov",
                SecondName = "Ivanovich",
                PhoneNumber = "89080808435",
                Email = "ivanovII@mail.ru"
            };

            var petrovP = new Customer
            {
                FirstName = "Petr",
                LastName = "Petrov",
                SecondName = "Petrovich",
                PhoneNumber = "2-23-48",
                Email = "petrov@yandex.ru"
            };

            var michailovM = new Customer
            {
                FirstName = "Michail",
                LastName = "Michailov",
                SecondName = "Michailovich",
                PhoneNumber = "8(908)123-34-45",
                Email = "micha@rambler.ru"
            };

            var ogurovO = new Customer
            {
                FirstName = "Oleg",
                LastName = "Ogurov",
                SecondName = "Olegovich",
                PhoneNumber = "2-22-22",
                Email = "ogura@mail.ru"
            };

            workUnit.GetRepository<ICustomerRepository>().AddRange(ivanovI, petrovP, michailovM, ogurovO);

            var order1 = new Order
            {
                Date = new DateTime(2015, 08, 19),
                Customer = petrovP
            };

            var order2 = new Order
            {
                Date = new DateTime(2015, 08, 07),
                Customer = ivanovI
            };

            var order3 = new Order
            {
                Date = new DateTime(2015, 08, 05),
                Customer = petrovP
            };

            var order4 = new Order
            {
                Date = new DateTime(2015, 08, 19),
                Customer = michailovM
            };

            var order5 = new Order
            {
                Date = new DateTime(2015, 08, 24),
                Customer = petrovP
            };

            workUnit.GetRepository<IOrderRepository>().AddRange(order1, order2, order3, order4, order5);

            order1.OrderItems.Add(new OrderItem
            {
                Product = buckwheat,
                Count = 1
            });

            order1.OrderItems.Add(new OrderItem
            {
                Product = chickenBreast,
                Count = 4
            });

            order1.OrderItems.Add(new OrderItem
            {
                Product = chickenBreast,
                Count = 3
            });

            order2.OrderItems.Add(new OrderItem
            {
                Product = tuna,
                Count = 2
            });

            order2.OrderItems.Add(new OrderItem
            {
                Product = buckwheat,
                Count = 1
            });

            order3.OrderItems.Add(new OrderItem
            {
                Product = chickenBreast,
                Count = 2
            });

            order3.OrderItems.Add(new OrderItem
            {
                Product = yogurt,
                Count = 1
            });

            order4.OrderItems.Add(new OrderItem
            {
                Product = tuna,
                Count = 5
            });

            order4.OrderItems.Add(new OrderItem
            {
                Product = cheese,
                Count = 3
            });

            order5.OrderItems.Add(new OrderItem
            {
                Product = yogurt,
                Count = 2
            });

            workUnit.Save();
        }
    }
}