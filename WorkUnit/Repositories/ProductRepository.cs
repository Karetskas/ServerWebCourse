using Academits.Karetskas.WorkUnit.Model;
using Academits.Karetskas.WorkUnit.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Academits.Karetskas.WorkUnit.Repositories
{
    public class ProductRepository : BaseEfRepository<Product>, IProductRepository
    {
        public ProductRepository(DbContext context) : base(context) { }
    }
}
