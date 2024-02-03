using kurs11135.Models;
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







        //private async void Button_Click(object sender, RoutedEventArgs e)
        //{

        //    string json = await Api.Post("OrderStatus", new OrderStatus { Name = "Новый статус" });
        //    if (!string.IsNullOrEmpty(json))
        //    {
        //        OrderStatus answer = Api.Deserialize<OrderStatus>(json);
        //    }
        //}

    }
}
