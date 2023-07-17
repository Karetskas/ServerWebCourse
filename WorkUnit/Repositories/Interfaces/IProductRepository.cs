using Academits.Karetskas.WorkUnit.Model;
using System.Collections.Generic;
using System;

namespace Academits.Karetskas.WorkUnit.Repositories.Interfaces
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        IEnumerable<Tuple<string, int>> GetPopularProduct();
    }
}
