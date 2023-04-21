using System;
using System.Collections.Generic;

namespace kurs11135.Models
{
    public partial class Product
    {
        public Product()
        {
            OrderProducts = new HashSet<OrderProduct>();
            Orders = new HashSet<Order>();
        }

        public int Id { get; set; }
        public int? CategoryId { get; set; }
        public string? ProductName { get; set; }
        public string? ShortDescription { get; set; }
        public decimal? ProductCost { get; set; }
        public int? ImageId { get; set; }

        public virtual ProductCategory? Category { get; set; }
        public virtual ProductImage? Image { get; set; }
        public virtual ICollection<OrderProduct> OrderProducts { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
