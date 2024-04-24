using kurs11135.DB;
using kurs11135.Tools;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace kurs11135.VM
{
    public class ComingProductVM : BaseVM
    {
        public ObservableCollection<OrderProduct> SelectedComingProducts { get; set; } = new ObservableCollection<OrderProduct>();
        public ObservableCollection<Product> Products { get; set; }

        private Product selectedProduct;
        public Product SelectedProduct
        {
            get => selectedProduct;
            set
            {
                selectedProduct = value;
                Signal();
            }
        }
        private string productName;
        public string ProductName
        {
            get => productName;
            set
            {
                productName = value;
                Signal();
            }
        }

        private string quantity;
        public string Quantity
        {
            get => quantity;
            set
            {
                if (int.TryParse(value, out int result) && result >= 0)
                {
                    quantity = value;
                    Signal();
                }
                else
                {
                    quantity = "0";
                    Signal();
                }
            }
        }

        private OrderProduct selectedComingProduct;
        public OrderProduct SelectedComingProduct
        {
            get => selectedComingProduct;
            set
            {
                selectedComingProduct = value;
                Signal();
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


        public ComingProductVM()
        {
            LoadProducts();

            AddProductToComingCommand = new CommandVM(() =>
            {
                if (SelectedProduct == null)
                {
                    MessageBox.Show("Пожалуйста, выберите товар.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (int.TryParse(Quantity, out int quantityValue) && quantityValue > 0)
                {
                    OnAddProductToComing(SelectedProduct, quantityValue);
                }
                else
                {
                    MessageBox.Show("Пожалуйста, введите корректное количество товара.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            });

            SaveButton = new CommandVM(async () =>
            {

                if (SelectedComingProducts.Count == 0)
                {
                    MessageBox.Show("Добавьте хотя бы один товар.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                foreach (var comingProduct in SelectedComingProducts)
                {
                    Product product = Products.FirstOrDefault(p => p.Id == comingProduct.ProductId);
                    if (product != null)
                    {
                        product.Quantity = (int.Parse(product.Quantity) + int.Parse(comingProduct.Count)).ToString();
                        var json = await Api.Post("Products", product, "put");
 
                    }
                }
                MessageBox.Show("Приход товара успешно зарегистрирован.");
                LoadProducts(); 
                CloseWindow();
            });

            RemoveSelectedProductCommand = new CommandVM(() =>
            {
                if (SelectedComingProduct != null)
                {
                    SelectedComingProducts.Remove(SelectedComingProduct);
                }
            });

        }

        private async void LoadProducts()
        {
            string json1 = await Api.Post("Products", null, "get");
            var result2 = Api.Deserialize<List<Product>>(json1);
            Products = new ObservableCollection<Product>(result2);
            Signal(nameof(Products));
        }

        private void OnAddProductToComing(Product selectedProduct, int quantity)
        {
            if (selectedProduct != null && quantity >= 0)
            {
                var existingProduct = SelectedComingProducts.FirstOrDefault(p => p.ProductId == selectedProduct.Id);
                if (existingProduct != null)
                {
                    existingProduct.Count += quantity;
                }
                else
                {
                    SelectedComingProducts.Add(new OrderProduct
                    {
                        ProductId = selectedProduct.Id,
                        Count = quantity.ToString(),
                        Product = selectedProduct
                    });
                }
            }
        }


        public CommandVM SaveButton { get; set; }
        public CommandVM RemoveSelectedProductCommand { get; set; }
        public CommandVM AddProductToComingCommand { get; }
    }
}
