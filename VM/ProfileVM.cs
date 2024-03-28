using kurs11135;
using kurs11135.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kurs11135.VM
{
    public class ProfileVM : BaseVM
    {
        public User User { get; private set; }
      
        public CommandVM EditProfile { get; set; }


        public ProfileVM(User user) 
        {
            User = user;


            EditProfile = new CommandVM(() =>
            {

                EditProfile ep = new EditProfile(user);
                ep.Show();

            });



        }



        

    }
}
