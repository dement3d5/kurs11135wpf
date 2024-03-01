using kurs11135;
using kurs11135.Tools;
using kurs11135.VM;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace kurs11135
{
    /// <summary>
    /// Логика взаимодействия для AddOrder.xaml
    /// </summary>
    public partial class AddOrder : Window
    {
        public AddOrder(User currentUser)
        {
            InitializeComponent();
            DataContext = new AddOrdVM(currentUser);
        }
    }
}
