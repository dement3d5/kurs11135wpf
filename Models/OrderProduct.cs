using System;
using System.Collections.Generic;

namespace kurs11135.Models
{
    public partial class OrderProduct
    {
        public int Id { get; set; }
        public int? OrderId { get; set; }
        public int? ProductId { get; set; }
        public string? Count { get; set; }
        public int? CategoryId { get; set; }

        public virtual Order? Order { get; set; }
        public virtual Product? Product { get; set; }
    }
}
