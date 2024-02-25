using kurs11135.Models;
using kurs11135.okna;
using kurs11135.Tools;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace kurs11135.VM
{
    public class UserOrdersVM : BaseVM
    {

        private DateTime createAt = DateTime.Now;

        public List<User> users { get; set; }
        private User listUser;
        public User ListUser
        {
            get => listUser;
            set
            {
                listUser = value;
                Signal();
            }
        }

        private User currentUser;
        public User CurrentUser
        {
            get => currentUser;
            set
            {
                currentUser = value;
                Signal(nameof(CurrentUser));
            }
        }


        public List<Order> orders { get; set; }
        private Order listOrder;
        public Order ListOrder
        {
            get => listOrder;
            set
            {
                listOrder = value;
                Signal(nameof(ListOrder));
            }
        }

        public List<Product> products { get; set; }
        private Product listProduct;
        public Product ListProduct
        {
            get => listProduct;
            set
            {
                listProduct = value;
                Signal();
            }
        }
        public Order order { get; set; }

        public Order SelectedItem { get; set; }
        public List<OrderStatus> orderStatuses { get; set; }
        private OrderStatus listOrderStatus;
        public OrderStatus ListOrderStatus
        {
            get => listOrderStatus;
            set
            {
                listOrderStatus = value;
                Signal();
            }
        }
        public DateTime CreateAt
        {
            get => createAt;
            set
            {
                createAt = value;
                Signal();
            }
        }
        public CommandVM SaveButton { get; set; }
        public CommandVM AddOrder { get; set; }
        public CommandVM DelOrder { get; set; }
        public CommandVM EditOrder { get; }
        public decimal CostOrder { get; set; }
        public string Count { get; set; }

        public CommandVM AddProductToOrderCommand { get; }
        public ObservableCollection<OrderProduct> SelectedProducts { get; set; } = new ObservableCollection<OrderProduct>();


        private void OnAddProductToOrder(Product selectedProduct, string quantity)
        {
            if (selectedProduct != null && int.TryParse(quantity, out int count))
            {
                SelectedProducts.Add(new OrderProduct
                {
                    ProductId = selectedProduct.Id,
                    Count = quantity,
                    Product = selectedProduct
                });

                CalculateSellPrice();
            }
        }

        private string quantity;

        public string Quantity
        {
            get => quantity;
            set
            {
                quantity = value;
                Signal();
            }
        }


        public User User { get; private set; }





        public UserOrdersVM(User currentUser)
        {

            Task.Run(async () => { await LoadOrders(); });


            CurrentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));

            AddProductToOrderCommand = new CommandVM(() =>
            {
                OnAddProductToOrder(ListProduct, Quantity);
            });


            Task.Run(async () =>
            {
                await LoadOrders();
            });
            AddOrder = new CommandVM(() =>
            {
                new AddOrder(currentUser).Show();
                Task.Run(async () =>
                {
                    await LoadOrders();
                });
            });
            DelOrder = new CommandVM(async () =>
            {
                var json1 = await Api.Post("Orders", SelectedItem.Id, "delete");
            });
            EditOrder = new CommandVM(async () =>
            {
                order = SelectedItem;
                new EditOrder(order).Show();
            });
        }


        private void CalculateSellPrice()
        {
            CostOrder = SelectedProducts.Sum(op => op.Product.SellPrice * int.Parse(op.Count));
            Signal(nameof(CostOrder));
        }

        public async Task LoadOrders()
        {

            var json = await Api.Post("OrderStatus", null, "get");
            var result = Api.Deserialize<List<OrderStatus>>(json);
            orderStatuses = result;
            Signal(nameof(orderStatuses));

            var ordersJson = await Api.Post("Orders", null , "get");
            var allOrders = Api.Deserialize<List<Order>>(ordersJson);
            orders = allOrders.Where(o => o.UserId == CurrentUser.Id).ToList();
            Signal(nameof(orders));

            string json1 = await Api.Post("Products", null, "get");
            var result2 = Api.Deserialize<List<Product>>(json1);
            products = result2;
            Signal(nameof(products));

            string json3 = await Api.Post("Users", null, "get");
            var result3 = Api.Deserialize<List<User>>(json3);
            users = result3;
            Signal(nameof(users));
        }
    }
}
