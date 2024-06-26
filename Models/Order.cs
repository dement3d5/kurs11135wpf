﻿using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace kurs11135
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

        public virtual OrderStatus? Status { get; set; }
        public virtual User? User { get; set; }
        public virtual ICollection<OrderProduct> OrderProducts { get; set; }
    }
}
