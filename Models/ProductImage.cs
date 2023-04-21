using System;
using System.Collections.Generic;

namespace kurs11135.Models
{
    public partial class ProductImage
    {
        public ProductImage()
        {
            Products = new HashSet<Product>();
        }

        public int Id { get; set; }
        public byte[]? Image { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
