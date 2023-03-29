using System;
using System.Collections.Generic;

namespace kurs11135.Models
{
    public partial class OrderProduct
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public string Count { get; set; } = null!;
        public int CategoryId { get; set; }
        public int Id { get; set; }

        public virtual ProductCategory Category { get; set; } = null!;
        public virtual Order Order { get; set; } = null!;
        public virtual Product Product { get; set; } = null!;
    }
}
