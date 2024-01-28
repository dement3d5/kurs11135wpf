﻿using kurs11135.Models;
using kurs11135.okna;
using kurs11135.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace kurs11135.VM
{
    public class AddOrderPageVM : BaseVM
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



        public AddOrderPageVM(User user)
        {
            AddProductToOrderCommand = new CommandVM(() =>
            {
                OnAddProductToOrder(ListProduct, Quantity);
            });



            SaveButton = new CommandVM(async () =>
            {
                if (SelectedProducts != null && SelectedProducts.Any())
                {
                    CalculateSellPrice();


                    var json = await Api.Post("Orders", new Order
                    {

                        CreateAt = CreateAt,
                        Cost = CostOrder,
                        OrderProducts = SelectedProducts.ToList(),
                        StatusId = 1,
                        //UserId =

                    }, "SaveOrder");
                    var result = Api.Deserialize<Order>(json);

                    if (result != null)
                    {
                        MessageBox.Show("Заказ успешно оформлен.");
                    }
                    else
                    {
                        MessageBox.Show("Ошибка при сохранении заказа.");
                    }
                }
                else
                {
                    MessageBox.Show("Не выбраны товары для заказа.");
                }
            });



            //SaveButton = new CommandVM(async () =>
            //{

            //    if (SelectedProducts != null && SelectedProducts.Count > 0)
            //    {
            //        var json = await Api.Post("Orders", new Order
            //        {
            //            CreateAt = CreateAt,
            //            Cost = CostOrder,
            //            ProductId = ListProduct.Id,
            //            Count = CountOrder,
            //            OrderProducts = new List<OrderProduct>(SelectedProducts)
            //        }, "SaveOrder");
            //        Order result = Api.Deserialize<Order>(json);
            //    }

            //});

            Task.Run(async () =>
            {
                await che();
            });
            //AddOrder = new CommandVM(() =>
            //{
            //    new AddOrder().Show();
            //    Task.Run(async () =>
            //    {
            //        await che();
            //    });
            //});
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

        //как блять комитнуть
        private void CalculateSellPrice()
        {

            CostOrder = SelectedProducts.Sum(op => op.Product.SellPrice * int.Parse(op.Count));
            Signal(nameof(CostOrder));
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
        //как блять комитнуть

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
    
