using System;
using Microsoft.EntityFrameworkCore;
using Academits.Karetskas.UnitOfWorkProject.Model;
using Academits.Karetskas.UnitOfWorkProject.UnitOfWork;
using Academits.Karetskas.UnitOfWorkProject.Repositories.Interfaces;

namespace Academits.Karetskas.UnitOfWorkProject
{
    internal class Program
    {
        static void Main()
        {
            using var dbContext = new UnitOfWorkContext();

            using var unitOfWork = new UnitOfWorkGroceryStore(dbContext);

            RecreateDatabase(unitOfWork, dbContext);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("1. Найти самый часто покупаемый продукт");
            Console.WriteLine();

            var popularProduct = unitOfWork.GetRepository<IProductRepository>().GetPopularProducts();

            Console.WriteLine("Самыми часто покупаемыми продуктами являются:");

            foreach (var product in popularProduct)
            {
                Console.WriteLine($"{product.Name} = {product.Count}");
            }

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("2. Найти сколько каждый клиент потратил денег за все время:");
            Console.WriteLine();

            var eachCustomerTotalCost = unitOfWork.GetRepository<ICustomerRepository>().GetExpensesForAllTime();

            foreach (var customer in eachCustomerTotalCost)
            {
                Console.WriteLine($"- {customer.LastName} {customer.FirstName} {customer.SecondName} потратил {customer.Cost:F2}");
            }

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("3. Вывести сколько товаров каждой категории купили:");
            Console.WriteLine();

            var productsCountByCategory = unitOfWork.GetRepository<ICategoryRepository>().GetProductsCountByCategory();

            foreach (var category in productsCountByCategory)
            {
                Console.WriteLine($"- категория: {category.Name} = {category.ProductsCount}");
            }

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("Изменить цену продукта Tuna на цену 54.07 рубля, а продукта Yogurt на цену 28.22 рубля.");
            Console.WriteLine();

            try
            {
                unitOfWork.BeginTransaction();

                UpdateProduct("Tuna", 54.07m, unitOfWork);
                UpdateProduct("Yogurt", 28.22m, unitOfWork);
                unitOfWork.Save();

                unitOfWork.CommitTransaction();

                Console.WriteLine("Изменена цена продукта Tuna и Yogurt.");
            }
            catch (Exception)
            {
                unitOfWork.RollbackTransaction();
            }
        }

        private static void UpdateProduct(string name, decimal price, UnitOfWorkGroceryStore unitOfWork)
        {
            if (unitOfWork is null)
            {
                throw new ArgumentNullException(nameof(unitOfWork), $"The argument \"{nameof(unitOfWork)}\" is null.");
            }

            var productRepository = unitOfWork.GetRepository<IProductRepository>();
            var products = productRepository.FindProducts(name);

            foreach (var product in products)
            {
                product.Price = price;

                productRepository.Update(product);
            }
        }

        private static void RecreateDatabase(UnitOfWorkGroceryStore unitOfWork, DbContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            try
            {
                unitOfWork.BeginTransaction();

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

                unitOfWork.GetRepository<ICategoryRepository>().AddRange(dairyProducts,
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

                unitOfWork.GetRepository<IProductRepository>().AddRange(yogurt,
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

                unitOfWork.GetRepository<ICustomerRepository>().AddRange(ivanovI, petrovP, michailovM, ogurovO);

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

                unitOfWork.GetRepository<IOrderRepository>().AddRange(order1, order2, order3, order4, order5);

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

                unitOfWork.Save();

                unitOfWork.CommitTransaction();
            }
            catch (Exception)
            {
                unitOfWork.RollbackTransaction();
            }
        }
    }
}