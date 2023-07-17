using System;
using System.Collections.Generic;
using Academits.Karetskas.WorkUnit.Model;

namespace Academits.Karetskas.WorkUnit.Repositories.Interfaces
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        IEnumerable<Tuple<string, int>> GetProductsCountByCategory();
    }
}
