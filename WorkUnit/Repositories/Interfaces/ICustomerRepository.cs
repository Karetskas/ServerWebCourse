using System;
using System.Linq;
using Academits.Karetskas.WorkUnit.Model;

namespace Academits.Karetskas.WorkUnit.Repositories.Interfaces
{
    public interface ICustomerRepository : IGenericRepository<Customer>
    {
        IQueryable<Tuple<string, string, string?, decimal>> GetExpensesForAllTime();
    }
}
