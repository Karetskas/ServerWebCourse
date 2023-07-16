using Academits.Karetskas.WorkUnit.Model;
using Academits.Karetskas.WorkUnit.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Academits.Karetskas.WorkUnit.Repositories
{
    public class CustomerRepository : BaseEfRepository<Customer>, ICustomerRepository
    {
        public CustomerRepository(DbContext context) : base(context) { }
    }
}
