using System;
using System.Collections.Generic;

namespace kurs11135.Models
{
    public partial class User
    {
        public User()
        {
            Orders = new HashSet<Order>();
        }

        public int Id { get; set; }
        public string? Login { get; set; }
        public string? Password { get; set; }
        public string? Organization { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int? StatusId { get; set; }

        public virtual UserPosition? Status { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
