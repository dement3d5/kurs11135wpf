using System;
using System.Collections.Generic;

namespace kurs11135
{
    public partial class Product
    {
        public Product()
        {
            OrderProducts = new HashSet<OrderProduct>();
        }

        public int Id { get; set; }
        public int? CategoryId { get; set; }
        public string? ProductName { get; set; }
        public string? ShortDescription { get; set; }
        public decimal? PostavPriсе { get; set; }
        public int? ImageId { get; set; }
        public decimal? SellPrice { get; set; }
        public double? Markup { get; set; }
        public string? Quantity { get; set; }

        public virtual ProductCategory? Category { get; set; }
        public virtual ProductImage? Image { get; set; }
        public virtual ICollection<OrderProduct> OrderProducts { get; set; }
    }
}
