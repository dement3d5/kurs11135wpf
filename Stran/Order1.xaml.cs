using kurs11135;
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

namespace kurs11135
{
   //fdsffd

    /// <summary>
    /// Логика взаимодействия для Order.xaml
    /// </summary>
    public partial class Order1 : Page
    {
        public Order1(User currentUser)
        {
            InitializeComponent();
            DataContext = new AdminOrdersVM(currentUser);

        }

    }
}
