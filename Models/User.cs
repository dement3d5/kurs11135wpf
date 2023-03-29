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
        public string Login { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Organization { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public int StatusId { get; set; }

        public virtual UserPosition Status { get; set; } = null!;
        public virtual ICollection<Order> Orders { get; set; }
    }
}
