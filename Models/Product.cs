using kurs11135.Models;
using System;
using System.Collections.Generic;

namespace kurs11135
{
    public partial class Product
    {
        public Product()
        {
            OrderProducts = new HashSet<OrderProduct>();
            Orders = new HashSet<Order>();
        }

        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string ProductName { get; set; } = null!;
        public string ShortDescription { get; set; } = null!;
        public decimal ProductCost { get; set; }
        public int ImageId { get; set; }

        public virtual ProductCategory Category { get; set; } = null!;
        public virtual ProductImage Image { get; set; } = null!;
        public virtual ICollection<OrderProduct> OrderProducts { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
