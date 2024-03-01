using kurs11135;
using kurs11135.okna;
using kurs11135.VM;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace kurs11135.Stran
{
    /// <summary>
    /// Логика взаимодействия для ProfilePage1.xaml
    /// </summary>
    public partial class ProfilePage1 : Page
    {
        
        public ProfilePage1(User user)
        {
           InitializeComponent();
            DataContext = new ProfileVM(user);
        }


    }
}
