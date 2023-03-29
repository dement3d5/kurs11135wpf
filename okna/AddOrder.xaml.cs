using kurs11135.Tools;
using System;
using System.Text.Json;
using System.Windows;

namespace kurs11135
{
    /// <summary>
    /// Логика взаимодействия для AddOrder.xaml
    /// </summary>
    public partial class AddOrder : Window
    {
        public AddOrder()
        {
            InitializeComponent();
           
        }
        public Order SelectedStatusId;
        public Order SelectedProductId;
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var json = await Api.Post("Order",  new Order
            {
                CreateAt = DateTime.Today,
                Count = TextBox_Count.Text,
                Cost = decimal.Parse(TextBox_Cost.Text),
                StatusId = SelectedStatusId.Id,
                ProductId = SelectedProductId.Id
            }, "SaveOrder");
            Order result = Api.Deserialize<Order>(json);
            MessageBox.Show("ну хз проверь бд");
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
