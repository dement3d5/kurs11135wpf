using kurs11135.Models;
using kurs11135.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace kurs11135.okna
{
    /// <summary>
    /// Логика взаимодействия для AuthLog.xaml
    /// </summary>
    public partial class AuthLog : Window
    {
        public AuthLog()
        {
            InitializeComponent();
        }

        private async void Log(object sender, RoutedEventArgs e)
        {
            string login = txt_Login.Text;
            string password = txt_Password.Text;
            var json = await Api.Post("Users", new User { Login = login, Password = password }, "Auth");
            var user= Api.Deserialize<User>(json);
            bool isLogin = false, isEditUser = false;
            {
                if (user.Id == 0)
                {
                    
                    MessageBox.Show("Такого пользователя не существует!!", $"ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                { 
                    isLogin = true;
                }
                if (isLogin)
                {
                    if (isEditUser)
           
                    MessageBox.Show("Вы успешно вошли!", "Успешно!", MessageBoxButton.OK, MessageBoxImage.Information);
                    MainWindow main = new MainWindow();
                    Close();
                    main.Show();
                }
                else 
                {
                    MessageBox.Show("Ошибка!", $"Неверный логин или пароль!", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }
        private void Reg(object sender, RoutedEventArgs e)
        {
            Auth m = new Auth();
            m.Show();
            this.Close();

        }
    }
}
