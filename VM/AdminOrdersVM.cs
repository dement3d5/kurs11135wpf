using kurs11135;
using kurs11135.okna;
using kurs11135.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace kurs11135.VM
{
    public class AdminOrdersVM : BaseVM
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

        public CommandVM FilterOrdersCommand { get; set; }
        public CommandVM SaveButton { get; set; }
        public CommandVM AddOrder { get; set; }
        public CommandVM DelOrder { get; set; }
        public CommandVM EditOrder { get; }
        public decimal? CostOrder { get; set; }
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
        public async void ExecuteMenuItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ListViewItem item = sender as ListViewItem;
                if (item != null && SelectedItem != null)
                {
                    var result = MessageBox.Show("Вы уверены, что заказ готов к выдаче?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        await UpdateOrderStatus(SelectedItem.Id, 2);
                        await LoadAllOrders();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при обновлении статуса заказа: {ex.Message}");
            }
        }


        public CommandVM SetOrderReadyCommand { get; set; }
        public CommandVM RefreshCommand { get; set; }
        public CommandVM RemoveUserOrder { get; set; }

        public User User { get; private set; }

        public AdminOrdersVM(User currentUser)
        {
            Task.Run(async () => { await LoadAllOrders(); });

            CurrentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));

            AddProductToOrderCommand = new CommandVM(() =>
            {
                OnAddProductToOrder(ListProduct, Quantity);
            });

            FilterOrdersCommand = new CommandVM(async () =>
            {
                await LoadOrdersByDate(CreateAt);
            });

            SetOrderReadyCommand = new CommandVM(async () =>
            {
                if (SelectedItem != null)
                {
                    var result = MessageBox.Show("Вы уверены, что заказ готов к выдаче?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        await UpdateOrderStatus(SelectedItem.Id, 2);
                        await LoadAllOrders();
                    }
                }
            });

            RemoveUserOrder = new CommandVM(async () =>
            {
                if (SelectedItem != null)
                {
                    var confirmResult = MessageBox.Show("Вы уверены, что хотите отменить выбранный заказ?", "Подтверждение отмены заказа", MessageBoxButton.YesNo);
                    if (confirmResult == MessageBoxResult.Yes)
                    {
                        if (SelectedItem.StatusId == 2)
                        {
                            MessageBox.Show("Нельзя удалять товары из заказа со статусом 'Готов к выдаче'.");
                            return;
                        }



                        foreach (var orderProduct in SelectedItem.OrderProducts.ToList())
                        {
                            await Api.Post("OrderProducts", orderProduct.Id, "delete");
                            var product = products.FirstOrDefault(p => p.Id == orderProduct.ProductId);
                            if (product != null)
                            {
                                product.Quantity = (int.Parse(product.Quantity) + int.Parse(orderProduct.Count)).ToString();
                                await Api.Post("Products", product, "put");
                            }
                        }


                        var json = await Api.Post("Orders", SelectedItem.Id, "delete");
                    }


                }

                else
                {
                    MessageBox.Show("Выберите заказ для удаления");
                }
            });


            RefreshCommand = new CommandVM(async () =>
            {
                await LoadAllOrders();
            });



            Task.Run(async () =>
            {
                await LoadAllOrders();
            });
            AddOrder = new CommandVM(() =>
            {
                new AddOrder(currentUser).Show();
                Task.Run(async () =>
                {
                    await LoadAllOrders();
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

        //как блять комитнуть
        private void CalculateSellPrice()
        {

            CostOrder = SelectedProducts.Sum(op => op.Product.SellPrice * int.Parse(op.Count));
            Signal(nameof(CostOrder));
        }
        public async Task UpdateOrderStatus(int orderId, int newStatusId)
        {
             var json = await Api.Put($"Orders/updateStatus/{orderId}", newStatusId, null);
        }






        public async Task LoadAllOrders()
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


        public async Task LoadOrdersByDate(DateTime selectedDate)
        {
            try
            {
                string filterDate = selectedDate.ToString("yyyy-MM-dd");
                string json = await Api.Post("Orders", filterDate, "getByDate");
                var result = Api.Deserialize<List<Order>>(json);
                orders = result;
                Signal(nameof(orders));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке заказов: {ex.Message}");
            }
        }







    }






}
