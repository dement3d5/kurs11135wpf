using System;
using System.Collections.Generic;

namespace kurs11135.Models
{
    public partial class ProductCategory
    {
        public ProductCategory()
        {
            OrderProducts = new HashSet<OrderProduct>();
            Products = new HashSet<Product>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<OrderProduct> OrderProducts { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
