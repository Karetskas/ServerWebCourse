using Academits.Karetskas.WorkUnit.Model;
using Academits.Karetskas.WorkUnit.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Academits.Karetskas.WorkUnit.Repositories
{
    public class OrderItemRepository : BaseEfRepository<OrderItem>, IOrderItemRepository
    {
        public OrderItemRepository(DbContext context) : base(context) { }
    }
}
