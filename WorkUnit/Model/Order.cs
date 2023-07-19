using System;
using System.Collections.Generic;

namespace Academits.Karetskas.WorkUnit.Model
{
    public class Order
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public int CustomerId { get; set; }

        public virtual Customer Customer { get; set; } = null!;

        public virtual ICollection<Product> Products { get; set; } = new List<Product>();

        public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}