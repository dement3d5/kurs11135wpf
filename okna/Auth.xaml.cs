using kurs11135;
using kurs11135.okna;
using kurs11135.Tools;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
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


        private async Task<bool> CheckExistingUser(string login)
        {
            var json = await Api.Post("Users", login, "CheckExistingUser");
            return Api.Deserialize<bool>(json);
        }

        private async Task<bool> CheckExistingPhone(string phone)
        {
            var json = await Api.Post("Users", phone, "CheckExistingPhone");
            return Api.Deserialize<bool>(json);
        }

        private bool IsValidPhoneNumber(string phoneNumber)
        {
            string pattern = @"^(?:\+7|8)(?:\(\d{3}\)|\d{3})\d{3}(?:-?\d{2}){2}$";
            return Regex.IsMatch(phoneNumber, pattern);
        }




        private async void Reg(object sender, RoutedEventArgs e)
        {

            if (string.IsNullOrWhiteSpace(txt_Login.Text) || string.IsNullOrWhiteSpace(txt_Password.Text) || string.IsNullOrWhiteSpace(txt_Org.Text) || string.IsNullOrWhiteSpace(txt_FirstName.Text) || string.IsNullOrWhiteSpace(txt_LastName.Text))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.");
                return;
            }

            var existingUser = await CheckExistingUser(txt_Login.Text);
            if (existingUser)
            {
                MessageBox.Show("Пользователь с таким логином уже существует!");
                return;
            }

            if (!IsValidPhoneNumber(txt_Org.Text))
            {
                MessageBox.Show("Некорректный номер телефона!");
                return;
            }

            var existingPhone = await CheckExistingPhone(txt_Org.Text);
            if (existingPhone)
            {
                MessageBox.Show("Пользователь с таким номером телефона уже существует!");
                return;
            }


            var json = await Api.Post("Users", new User
            {
                Login = txt_Login.Text,
                Password = txt_Password.Text,
                Organization = txt_Org.Text,
                FirstName = txt_FirstName.Text,
                LastName = txt_LastName.Text
            }, "SaveUser");

            User result = Api.Deserialize<User>(json);
            MessageBox.Show("Найс!");

            AuthLog m = new AuthLog();
            m.Show();
            this.Close();
        }

      

    }

}
