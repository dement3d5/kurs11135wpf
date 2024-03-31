using kurs11135;
using kurs11135.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Windows;
using kurs11135.okna;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Reflection.Metadata;
using System.Text.Json;
using System.Xml.Linq;

namespace kurs11135.VM
{
    public class AddOrdVM : BaseVM
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

        private OrderProduct selectedProduct;
        public OrderProduct SelectedProduct
        {
            get { return selectedProduct; }
            set
            {
                selectedProduct = value;
                Signal();
            }
        }

        public CommandVM SaveButton { get; set; }
        public CommandVM AddOrder { get; set; }
        public CommandVM DelOrder { get; set; }
        public CommandVM RemoveProductFromOrderCommand { get; set; }
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

        private ObservableCollection<Product> _products;
        public ObservableCollection<Product> Products
        {
            get { return _products; }
            set
            {
                _products = value;
                FilteredProducts = new ObservableCollection<Product>(_products.Where(p => int.Parse(p.Quantity) > 0));
                Signal(nameof(FilteredProducts));
            }
        }

        private ObservableCollection<Product> _filteredProducts;
        public ObservableCollection<Product> FilteredProducts
        {
            get { return _filteredProducts; }
            set
            {
                _filteredProducts = value;
                Signal(nameof(FilteredProducts));
            }
        }


        private async void CloseWindow()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                foreach (Window window in Application.Current.Windows)
                {
                    if (window.DataContext == this)
                    {
                        window.Close();
                        break;
                    }
                }
            });
        }




        private int inputQuantity;


        public User User { get; private set; }

        public AddOrdVM(User currentUser)
        {

            CurrentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));

            AddProductToOrderCommand = new CommandVM(() =>
            {
                if (ListProduct == null)
                {
                    MessageBox.Show("Пожалуйста, выберите товар.");
                    return;
                }

                if (!int.TryParse(Quantity, out int inputQuantity) || inputQuantity <= 0)
                {
                    MessageBox.Show("Некорректное количество товара. Пожалуйста, введите положительное целое число.");
                    return;
                }

                if (inputQuantity > int.Parse(ListProduct.Quantity))
                {
                    MessageBox.Show($"Невозможно добавить {inputQuantity} единиц товара {ListProduct.ProductName}, так как на складе осталось только {ListProduct.Quantity} единиц.");
                    return;
                }

                OnAddProductToOrder(ListProduct, Quantity);
            });

            RemoveProductFromOrderCommand = new CommandVM(() =>
            {

                if (SelectedProduct != null)
                {
                    SelectedProducts.Remove(SelectedProduct);
                    CalculateSellPrice();
                }

            });




            SaveButton = new CommandVM(async () =>
            {
                if (SelectedProducts != null && SelectedProducts.Any())
                {
                    CalculateSellPrice();

                    foreach (var orderProduct in SelectedProducts)
                    {
                        Product product = Products.FirstOrDefault(p => p.Id == orderProduct.ProductId);
                        if (product != null)
                        {
                            int newQuantity = Convert.ToInt32(product.Quantity) - Convert.ToInt32(orderProduct.Count);

                            if (newQuantity >= 0)
                            {
                                product.Quantity = newQuantity.ToString();

                                var json1 = await Api.Post("Products", product, "put");

                            }
                            else
                            {
                                MessageBox.Show($"Невозможно оформить заказ. Недостаточно товара {product.ProductName} на складе.");
                                return;
                            }
                        }
                    }

                    var order = new Order
                    {
                        CreateAt = CreateAt,
                        Cost = CostOrder,
                        OrderProducts = SelectedProducts.ToList(),
                        StatusId = 1,
                        UserId = CurrentUser.Id
                    };
                    var json = await Api.Post("Orders", order, "SaveOrder");
                    var result = Api.Deserialize<Order>(json);
                        await che();

                        MessageBox.Show("Заказ успешно оформлен.");

                    CloseWindow();
                }
               
            });




            //SaveButton = new CommandVM(async () =>
            //{
            //    if (SelectedProducts != null && SelectedProducts.Any())
            //    {
            //        CalculateSellPrice();


            //        var order = new Order
            //        {
            //            CreateAt = CreateAt,
            //            Cost = CostOrder,
            //            OrderProducts = SelectedProducts.ToList(),
            //            StatusId = 1,
            //            UserId = CurrentUser.Id
            //        };
            //        var json = await Api.Post("Orders", order, "SaveOrder");
            //        var result = Api.Deserialize<Order>(json);

            //        if (result != null)
            //        {

            //            foreach (var orderProduct in SelectedProducts)
            //            {
            //                Product product = products.FirstOrDefault(p => p.Id == orderProduct.ProductId);
            //                if (product != null)
            //                {
            //                    int newQuantity = Convert.ToInt32(product.Quantity) - Convert.ToInt32(orderProduct.Count);

            //                    if (newQuantity >= 0)
            //                    {
            //                        product.Quantity = newQuantity.ToString();

            //                        json = await Api.Post("Products", product, "put");
                        
            //                    }
            //                }
            //            }

                     
            //            await che();

            //            MessageBox.Show("Заказ успешно оформлен.");
            //        }
            //        else
            //        {
            //            MessageBox.Show("Ошибка при сохранении заказа.");
            //        }
            //    }
            //    else
            //    {
            //        MessageBox.Show("Не выбраны товары для заказа.");
            //    }
            //});




            //foreach (var orderProduct in SelectedProducts)
            //{
            //    Product product = products.FirstOrDefault(p => p.Id == orderProduct.ProductId);
            //    if (product != null)
            //    {
            //        int currentQuantity = Convert.ToInt32(product.Quantity);
            //        int orderQuantity = Convert.ToInt32(orderProduct.Count);

            //        if (currentQuantity >= orderQuantity)
            //        {
            //            Signal(nameof(ListProduct));

            //        }
            //    }
            //}
            //che();

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
            AddOrder = new CommandVM(() =>
            {
                new AddOrder(currentUser).Show();
                Task.Run(async () =>
                {
                    await che();
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
            Products = new ObservableCollection<Product>(result2); 
            Signal(nameof(Products));

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