using kurs11135.Models;
using kurs11135.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Windows;

namespace kurs11135.VM
{
    public class AddOrdVM : BaseVM
    {

        private DateTime createAt;


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

        public List<Order> orders { get; set; }
        private Order listOrder;
        public Order ListOrder
        {
            get => listOrder;
            set
            {
                listOrder = value;
                Signal();
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

            }
        }
        public CommandVM SaveButton { get; set; }
        public CommandVM AddOrder { get; set; }
        public CommandVM DelOrder { get; set; }
        public decimal CostOrder { get; set; }
        public string CountOrder { get; set; }

        public AddOrdVM()
        {
            SaveButton = new CommandVM(async () =>
            {
                var json = await Api.Post("Orders", new Order
                {
                    CreateAt = CreateAt,
                    Count = CountOrder,
                    Cost = CostOrder,
                    StatusId = ListOrderStatus.Id,
                    Status = ListOrderStatus,
                    ProductId = ListProduct.Id,
                    UserId = ListUser.Id,
                    User = ListUser
                }, "SaveOrder");
                Order result = Api.Deserialize<Order>(json);


            });
                Task.Run(async () =>
            {
                await che();
            });
            AddOrder = new CommandVM(() =>
            {
                new AddOrder().Show();
            });
            DelOrder = new CommandVM(async () =>
            {
                var json1 = await Api.Post("Orders", SelectedItem.Id, "delete");
            });
        }
        public async Task che()
        {
            var json = await Api.Post("OrderStatus", null, "get");
            var result = Api.Deserialize<List<OrderStatus>>(json);
            orderStatuses = result;
            Signal(nameof(orderStatuses));

            var json4 = await Api.Post("Orders", null, "get");
            var result4 = Api.Deserialize<List<Order>>(json4);
            orders = result4;
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


        //public void UpdateList()
        //{
        //    var json = await Api.Post("OrderStatus", null, "get");
        //    var result = Api.Deserialize<List<OrderStatus>>(json);
        //    orderStatuses = result;
        //    Signal(nameof(orderStatuses));

        //    string json1 = await Api.Post("Products", null, "get");
        //    var result2 = Api.Deserialize<List<Product>>(json1);
        //    products = result2;
        //    Signal(nameof(products));
        //}
    } 
}
