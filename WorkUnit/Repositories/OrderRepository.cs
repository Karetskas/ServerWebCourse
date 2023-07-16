using Academits.Karetskas.WorkUnit.Model;
using Academits.Karetskas.WorkUnit.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Academits.Karetskas.WorkUnit.Repositories
{
    public class OrderRepository : BaseEfRepository<Order>, IOrderRepository
    {
        public OrderRepository(DbContext context) : base(context) { }
    }
}
