namespace Academits.Karetskas.Migrations.Model
{
    public class CategoryProduct
    {
        public int Id { get; set; }

        public int CategoryId { get; set; }

        public virtual Category Category { get; set; } = null!;

        public int ProductId { get; set; }

        public virtual Product Product { get; set; } = null!;
    }
}
