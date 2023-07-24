using Microsoft.EntityFrameworkCore;
using Academits.Karetskas.UnitOfWorkProject.Model;
using Academits.Karetskas.UnitOfWorkProject.Repositories.Interfaces;

namespace Academits.Karetskas.UnitOfWorkProject.Repositories
{
    public class CategoryProductRepository : BaseEfRepository<CategoryProduct>, ICategoryProductRepository
    {
        public CategoryProductRepository(DbContext dbContext) : base(dbContext) { }
    }
}
