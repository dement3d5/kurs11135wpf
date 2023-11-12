using System;
using System.Collections.Generic;

namespace kurs11135.Models
{
    public partial class Order
    {
        public Order()
        {
            OrderProducts = new HashSet<OrderProduct>();
        }

        public int Id { get; set; }
        public int? StatusId { get; set; }
        public DateTime? CreateAt { get; set; }
        public int? UserId { get; set; }
        public decimal? Cost { get; set; }
        public string? Count { get; set; }
        public int? ProductId { get; set; }

        public virtual Product? Product { get; set; }
        public virtual OrderStatus? Status { get; set; }
        public virtual User? User { get; set; }
        public virtual ICollection<OrderProduct> OrderProducts { get; set; }
    }
}
