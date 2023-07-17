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
            var categoriesProducts = _dbContext.Set<CategoryProduct>().AsQueryable();

            return categoriesProducts
                .GroupBy(categoryProduct => categoryProduct.CategoryId)
                .Select(categoriesProductsGroup =>
                    Tuple.Create(categoriesProductsGroup.Single().Category.Name,
                        categoriesProductsGroup
                            .SelectMany(categoryProduct => categoryProduct.Product.OrderItems)
                            .Sum(orderItem => orderItem.Count)
                        ));
        }
    }
}
