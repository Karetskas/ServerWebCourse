using System;
using System.Linq;
using Academits.Karetskas.ShopEf.Model;
using Microsoft.EntityFrameworkCore;

namespace Academits.Karetskas.ShopEf
{
    internal class Program
    {
        static void Main()
        {
            using var dbContext = new ShopEfContext();

            RecreateDatabase(dbContext);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("1. Найти самый часто покупаемый продукт");
            Console.WriteLine();

            var productsCount = dbContext.OrdersItems
                .Join(dbContext.Products,
                    orderItem => orderItem.ProductId,
                    product => product.Id,
                    (orderItem, product) => new
                    {
                        orderItem.Count,
                        product.Name
                    })
                .GroupBy(orderedGoods => orderedGoods.Name)
                .Select(productsGroup => new
                {
                    Name = productsGroup.Key,
                    Count = productsGroup.Sum(orderedGoods => orderedGoods.Count)
                });

            var popularProduct = productsCount
                .Where(orderedGoods => orderedGoods.Count == productsCount
                    .Max(orderedGoods => orderedGoods.Count));

            Console.WriteLine("Самыми часто покупаемыми продуктами являются:");

            foreach (var product in popularProduct)
            {
                Console.WriteLine($"{product.Name} = {product.Count}");
            }

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("2. Найти сколько каждый клиент потратил денег за все время:");
            Console.WriteLine();

            var eachCustomerTotalCost = dbContext.OrdersItems
                .Join(dbContext.Products,
                    orderItem => orderItem.ProductId,
                    product => product.Id,
                    (orderItem, product) => new
                    {
                        orderItem.OrderId,
                        orderItemsTotalCost = orderItem.Count * product.Price
                    })
                .GroupBy(order => order.OrderId)
                .Select(ordersGroup => new
                {
                    OrderId = ordersGroup.Key,
                    orderTotalSum = ordersGroup.Sum(orderItem => orderItem.orderItemsTotalCost)
                })
                .Join(dbContext.Orders,
                    orderSum => orderSum.OrderId,
                    order => order.Id,
                    (orderSum, order) => new
                    {
                        orderSum.orderTotalSum,
                        order.CustomerId
                    })
                .GroupBy(customer => customer.CustomerId)
                .Select(customer => new
                {
                    CustomerId = customer.Key,
                    eachCustomerTotalSum = customer.Sum(orderCost => orderCost.orderTotalSum)
                })
                .Join(dbContext.Customers,
                    customerOrderCost => customerOrderCost.CustomerId,
                    customer => customer.Id,
                    (customerOrderCost, customer) => new
                    {
                        customer.LastName,
                        customer.FirstName,
                        customer.SecondName,
                        customerOrderCost.eachCustomerTotalSum
                    });

            foreach (var customer in eachCustomerTotalCost)
            {
                Console.WriteLine($"- {customer.LastName} {customer.FirstName} {customer.SecondName} потратил {customer.eachCustomerTotalSum:F2}");
            }

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("Вывести сколько товаров каждой категории купили:");
            Console.WriteLine();

            var productsCountByCategory = dbContext.Categories
                .Include(category => category.Products)
                .Select(category => new
                {
                    category.Name,
                    ProductsCount = category.Products
                        .Join(dbContext.OrdersItems,
                            product => product.Id,
                            orderItem => orderItem.ProductId,
                            (product, orderItem) => new
                            {
                                product.Id,
                                orderItem.Count
                            })
                        .GroupBy(product => product.Id)
                        .Select(productsGroup => new
                        {
                            Count = productsGroup.Sum(product => product.Count)
                        })
                        .Sum(product => product.Count)
                });

            foreach (var category in productsCountByCategory)
            {
                Console.WriteLine($"- категория: {category.Name} = {category.ProductsCount}");
            }
        }

        private static void RecreateDatabase(ShopEfContext context)
        {
            context.Database.EnsureDeleted();

            context.Database.EnsureCreated();

            var dairyProducts = new Category()
            {
                Name = "Dairy products"
            };

            var meatAndPoultry = new Category()
            {
                Name = "Meat and poultry"
            };

            var fruitsAndVegetables = new Category()
            {
                Name = "Fruits and vegetables"
            };

            var grainsAndBakeryProducts = new Category()
            {
                Name = "Grains and bakery products"
            };

            var fishAndSeafood = new Category()
            {
                Name = "Fish and seafood"
            };

            context.Categories.AddRange(dairyProducts,
                meatAndPoultry,
                fruitsAndVegetables,
                grainsAndBakeryProducts,
                fishAndSeafood);

            var yogurt = new Product()
            {
                Name = "Yogurt",
                Price = 35.67m
            };

            var chickenBreast = new Product()
            {
                Name = "Chicken breast",
                Price = 142.04m
            };

            var buckwheat = new Product()
            {
                Name = "Buckwheat",
                Price = 38.00m
            };

            var tuna = new Product()
            {
                Name = "Tuna",
                Price = 71.71m
            };

            var cheese = new Product()
            {
                Name = "Cheese",
                Price = 59.18m
            };

            context.Products.AddRange(yogurt,
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

            var ivanovI = new Customer()
            {
                FirstName = "Ivan",
                LastName = "Ivanov",
                SecondName = "Ivanovich",
                PhoneNumber = "89080808435",
                Email = "ivanovII@mail.ru"
            };

            var petrovP = new Customer()
            {
                FirstName = "Petr",
                LastName = "Petrov",
                SecondName = "Petrovich",
                PhoneNumber = "2-23-48",
                Email = "petrov@yandex.ru"
            };

            var michailovM = new Customer()
            {
                FirstName = "Michail",
                LastName = "Michailov",
                SecondName = "Michailovich",
                PhoneNumber = "8(908)123-34-45",
                Email = "micha@rambler.ru"
            };

            var ogurovO = new Customer()
            {
                FirstName = "Oleg",
                LastName = "Ogurov",
                SecondName = "Olegovich",
                PhoneNumber = "2-22-22",
                Email = "ogura@mail.ru"
            };

            context.Customers.AddRange(ivanovI, petrovP, michailovM, ogurovO);

            var order1 = new Order()
            {
                Date = new DateTime(2015, 08, 19),
                Customer = petrovP
            };

            var order2 = new Order()
            {
                Date = new DateTime(2015, 08, 07),
                Customer = ivanovI
            };

            var order3 = new Order()
            {
                Date = new DateTime(2015, 08, 05),
                Customer = petrovP
            };

            var order4 = new Order()
            {
                Date = new DateTime(2015, 08, 19),
                Customer = michailovM
            };

            var order5 = new Order()
            {
                Date = new DateTime(2015, 08, 24),
                Customer = petrovP
            };

            context.Orders.AddRange(order1, order2, order3, order4, order5);

            order1.OrdersItems.Add(new OrderItem()
            {
                Product = buckwheat,
                Count = 1
            });

            order1.OrdersItems.Add(new OrderItem()
            {
                Product = chickenBreast,
                Count = 4
            });

            order1.OrdersItems.Add(new OrderItem()
            {
                Product = chickenBreast,
                Count = 3
            });

            order2.OrdersItems.Add(new OrderItem()
            {
                Product = tuna,
                Count = 2
            });

            order2.OrdersItems.Add(new OrderItem()
            {
                Product = buckwheat,
                Count = 1
            });

            order3.OrdersItems.Add(new OrderItem()
            {
                Product = chickenBreast,
                Count = 2
            });

            order3.OrdersItems.Add(new OrderItem()
            {
                Product = yogurt,
                Count = 1
            });

            order4.OrdersItems.Add(new OrderItem()
            {
                Product = tuna,
                Count = 5
            });

            order4.OrdersItems.Add(new OrderItem()
            {
                Product = cheese,
                Count = 3
            });

            order5.OrdersItems.Add(new OrderItem()
            {
                Product = yogurt,
                Count = 2
            });

            context.SaveChanges();
        }
    }
}