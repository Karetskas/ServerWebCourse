using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Academits.Karetskas.UnitOfWorkProject.Model;
using Academits.Karetskas.UnitOfWorkProject.QueryResultClasses;
using Academits.Karetskas.UnitOfWorkProject.Repositories.Interfaces;

namespace Academits.Karetskas.UnitOfWorkProject.Repositories
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

        public IEnumerable<CategoryInfo> GetProductsCountByCategory()
        {
            var categoriesProducts = _dbContext.Set<Category>().AsQueryable();

            return categoriesProducts
                .Select(category => new CategoryInfo(
                    category.Name,
                    category.Products
                        .SelectMany(product => product.OrderItems)
                        .Sum(orderItem => orderItem.Count)
                ));
        }
    }
}
