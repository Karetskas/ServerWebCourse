using System.Collections.Generic;

namespace Academits.Karetskas.Migrations.Model
{
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public decimal Price { get; set; }

        public int? SupplierId { get; set; }

        public virtual ICollection<Category> Categories { get; set; } = new List<Category>();

        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

        public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

        public virtual Supplier? Supplier { get; set; } = new Supplier();
    }
}
