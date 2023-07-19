using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Academits.Karetskas.WorkUnit.Model;
using Academits.Karetskas.WorkUnit.Repositories.Interfaces;

namespace Academits.Karetskas.WorkUnit.Repositories
{
    public class CategoryRepository : BaseEfRepository<Category>, ICategoryRepository
    {
        private readonly DbContext _dbContext;

        public CategoryRepository(DbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext is null
                ? throw new ArgumentNullException(nameof(dbContext), $"The argument \"{nameof(dbContext)}\" is null.")
                : dbContext;
        }

        public IEnumerable<Tuple<string, int>> GetProductsCountByCategory()
        {
            var categoriesProducts = _dbContext.Set<Category>().AsQueryable();

            return categoriesProducts
                .Select(category =>
                    Tuple.Create(category.Name,
                        category.Products
                            .SelectMany(product => product.OrderItems)
                            .Sum(orderItem => orderItem.Count)));
        }
    }
}
