using kurs11135.Models;
using kurs11135.okna;
using kurs11135.Tools;
using System.Windows;

namespace kurs11135
{
    /// <summary>
    /// Логика взаимодействия для Auth.xaml
    /// </summary>
    public partial class Auth : Window
    {
        public Auth()
        {
            InitializeComponent();
        }

        private async void Reg(object sender, RoutedEventArgs e)
        {
            var json = await Api.Post("Users", new User { Login = txt_Login.Text,
                Password = txt_Password.Text, 
                Organization = txt_Org.Text,
                FirstName = txt_FirstName.Text, 
                LastName = txt_LastName.Text }, "SaveUser");
            User result = Api.Deserialize<User>(json);
            MessageBox.Show("Найс!");

            AuthLog m = new AuthLog();
            m.Show();
            this.Close();
        }
    }
}
