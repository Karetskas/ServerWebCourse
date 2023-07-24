using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Academits.Karetskas.UnitOfWorkProject.Model;
using Academits.Karetskas.UnitOfWorkProject.QueryResultClasses;
using Academits.Karetskas.UnitOfWorkProject.Repositories.Interfaces;

namespace Academits.Karetskas.UnitOfWorkProject.Repositories
{
    public class ProductRepository : BaseEfRepository<Product>, IProductRepository
    {
        private readonly DbContext _dbContext;

        public ProductRepository(DbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext is null
                ? throw new ArgumentNullException(nameof(dbContext), $"The argument \"{nameof(dbContext)}\" is null.")
                : dbContext;
        }

        public IEnumerable<PopularProduct> GetPopularProducts()
        {
            var orderItems = _dbContext.Set<OrderItem>();

            return orderItems
                .GroupBy(orderItem => orderItem.Product)
                .Select(orderItemsGroup => new
                {
                    orderItemsGroup.Key.Name,
                    Count = orderItemsGroup.Sum(orderItem => orderItem.Count)
                })
                .Where(product => product.Count == orderItems
                    .GroupBy(orderItem => orderItem.Product)
                    .Select(orderItemsGroup => new
                    {
                        Count = orderItemsGroup.Sum(orderItem => orderItem.Count)
                    })
                    .Max(orderItem => orderItem.Count))
                .Select(popularProduct => new PopularProduct(popularProduct.Name, popularProduct.Count));
        }

        public IEnumerable<Product> FindProducts(string name)
        {
            return _dbContext.Set<Product>()
                .AsQueryable()
                .Where(product => product.Name == name);
        }
    }
}
