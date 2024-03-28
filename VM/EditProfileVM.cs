using kurs11135.Tools;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace kurs11135.VM
{
    public class EditProfileVM : BaseVM
    {
        private User _editedUser;

        public User EditedUser
        {
            get { return _editedUser; }
            set
            {
                _editedUser = value;
                Signal(nameof(EditedUser));
            }
        }

        public List<User> users { get; set; }
        public User user { get; set; }

        public async Task che()
        {
            string json1 = await Api.Post("Users", null, "get");
            var result1 = Api.Deserialize<List<User>>(json1);
            users = result1;
            Signal(nameof(users));

        }

        public CommandVM SaveProfileCommand { get;  set; }

        public EditProfileVM(User user)
        {
            EditedUser = user;

            SaveProfileCommand = new CommandVM(async ()=>
            {
                var json1 = await Api.Post("Users", new User
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Login = user.Login,
                    Password = user.Password,
                }, "put");
                che();
            });
        }



    }
}
