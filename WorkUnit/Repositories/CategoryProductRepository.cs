using Academits.Karetskas.WorkUnit.Model;
using Academits.Karetskas.WorkUnit.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Academits.Karetskas.WorkUnit.Repositories
{
    public class CategoryProductRepository : BaseEfRepository<CategoryProduct>, ICategoryProductRepository
    {
        public CategoryProductRepository(DbContext context) : base(context) { }
    }
}
