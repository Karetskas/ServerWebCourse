using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Academits.Karetskas.WorkUnit.Model;
using Academits.Karetskas.WorkUnit.Repositories.Interfaces;

namespace Academits.Karetskas.WorkUnit.Repositories
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

        public IEnumerable<Tuple<string, int>> GetPopularProduct()
        {
            var orderItems = _dbContext.Set<OrderItem>().AsQueryable();

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
                .Select(popularProduct => Tuple.Create(popularProduct.Name, popularProduct.Count))
                .ToList();
        }
    }
}
