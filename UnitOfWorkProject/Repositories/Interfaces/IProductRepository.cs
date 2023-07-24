using System.Collections.Generic;
using Academits.Karetskas.UnitOfWorkProject.Model;
using Academits.Karetskas.UnitOfWorkProject.QueryResultClasses;

namespace Academits.Karetskas.UnitOfWorkProject.Repositories.Interfaces
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        IEnumerable<PopularProduct> GetPopularProducts();

        IEnumerable<Product> FindProducts(string name);
    }
}
