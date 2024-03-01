using System;
using System.Collections.Generic;

namespace kurs11135
{
    public partial class UserPosition
    {
        public UserPosition()
        {
            Users = new HashSet<User>();
        }

        public int Id { get; set; }
        public string? Title { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
