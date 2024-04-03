using kurs11135;
using kurs11135.okna;
using kurs11135.Tools;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace kurs11135.VM
{
    public class AddProdVM : BaseVM
    {
        public CommandVM AddProduct { get; set; }
        public CommandVM AddImage { get; set; }
        public CommandVM SaveButton { get; set; }
        public CommandVM ComingProduct { get; set; }
        public CommandVM EditProduct { get; set; }
        public Product SelectedItem { get; set; }
        public CommandVM DelProduct { get; set; }

        public List<Product> products { get; set; }
        public Product product { get; set; }

        public byte[]? Image { get => image; set { image = value; Signal(); } }
        public string NameProduct { get; set; }

      
        public string ShortName { get; set; }
        public string Quantity { get; set; }


        public decimal PostavPriсе
        {
            get { return _postavPrice; }
            set
            {
                _postavPrice = value;
                CalculateSellPrice();
                Signal(nameof(PostavPriсе));
            }
        }
        private decimal _sellPrice;
        public decimal SellPrice
        {
            get { return _sellPrice; }
            set
            {
                if (_sellPrice != value)
                {
                    _sellPrice = value;
                    Signal(nameof(SellPrice));
                }
            }
        }


        public decimal Markup
        {
            get { return _markup; }
            set
            {
                _markup = value;
                CalculateSellPrice();
                Signal(nameof(Markup));
            }
        }

        private void CalculateSellPrice()
        {

            SellPrice = PostavPriсе * (1 + Markup / 100);
            Signal(nameof(SellPrice));
        }
        public CommandVM RefreshCommand { get; set; }

        public AddProdVM()
        {


            RefreshCommand = new CommandVM(async () =>
            {
                await che();
            });

            SaveButton = new CommandVM(async () =>
            {

                if (ListProductCategory == null || string.IsNullOrWhiteSpace(NameProduct) || string.IsNullOrWhiteSpace(ShortName) || string.IsNullOrWhiteSpace(Quantity) || string.IsNullOrWhiteSpace(PostavPriсе.ToString()) || string.IsNullOrWhiteSpace(Markup.ToString()))
                {
                    MessageBox.Show("Пожалуйста, заполните все данные.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                
                if (!int.TryParse(Quantity, out int parsedQuantity) || parsedQuantity <= 0)
                {
                    MessageBox.Show("Некорректное количество товара. Пожалуйста, введите положительное целое число.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

               
                if (!decimal.TryParse(PostavPriсе.ToString(), out decimal parsedPostavPrice) || parsedPostavPrice <= 0)
                {
                    MessageBox.Show("Некорректная цена поставщика. Пожалуйста, введите положительное числовое значение.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

              
                if (!IsDecimalValid(PostavPriсе.ToString()))
                {
                    MessageBox.Show("Некорректный формат цены поставщика. Пожалуйста, введите число с использованием цифр и десятичного разделителя.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

               
                if (!decimal.TryParse(Markup.ToString(), out decimal parsedMarkup) || parsedMarkup < 0)
                {
                    MessageBox.Show("Некорректная наценка. Пожалуйста, введите положительное числовое значение или 0.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                
                if (!IsDecimalValid(Markup.ToString()))
                {
                    MessageBox.Show("Некорректный формат наценки. Пожалуйста, введите число с использованием цифр и десятичного разделителя.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

               
                if (Image == null)
                {
                    MessageBox.Show("Пожалуйста, выберите изображение товара.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

              
                if (products.Any(p => p.ProductName.Equals(NameProduct, StringComparison.OrdinalIgnoreCase)))
                {
                    MessageBox.Show("Товар с таким названием уже существует. Пожалуйста, выберите другое название.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                
                CalculateSellPrice();

               
                var json3 = await Api.Post("ProductImages", new ProductImage { Image = Image }, "get");
                var image = Api.Deserialize<ProductImage>(json3);
                var json1 = await Api.Post("Products", new Product
                {
                    CategoryId = ListProductCategory.Id,
                    ProductName = NameProduct,
                    PostavPriсе = PostavPriсе,
                    SellPrice = SellPrice,
                    ShortDescription = ShortName,
                    Markup = (double)Markup,
                    Quantity = Quantity,
                    ImageId = image.Id
                }, "SaveProduct");
                Product result1 = Api.Deserialize<Product>(json1);

                MessageBox.Show("Товар успешно добавлен.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

              
                CloseWindow();
            });

          
           







            Task.Run(async () =>
            {
                await che();
            });

            AddProduct = new CommandVM(() =>
            {
                new AddProduct().Show();
                Task.Run(async () =>
                {
                    await che();
                });

            });
            AddImage = new CommandVM( async() =>
            {
                OpenFileDialog ofd = new();
                if(ofd.ShowDialog() == true)
                {
                    var bytes = File.ReadAllBytes(ofd.FileName);
                    Image = bytes;
                }
            });
            DelProduct = new CommandVM(async () =>
            {
                
                if (SelectedItem == null)
                {
                    MessageBox.Show("Пожалуйста, выберите товар для удаления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                 var confirmResult = MessageBox.Show("Вы уверены, что хотите удалить выбранный товар?", "Подтверждение удаления товара", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (confirmResult == MessageBoxResult.Yes)
                {
                    var jsonOrders = await Api.Post("Orders", null, "get");
                    var orders = Api.Deserialize<List<Order>>(jsonOrders);
                    var acceptedOrders = orders.Where(o => o.OrderProducts.Any(op => op.ProductId == SelectedItem.Id && o.StatusId == 1)).ToList();
                    if (acceptedOrders.Any())
                    {
                        MessageBox.Show("Невозможно удалить товар, так как он используется в активных заказах.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    var jsonDeleteProduct = await Api.Post("Products", SelectedItem.Id, "delete");
                    await che();
                }


               
            });

            EditProduct = new CommandVM(async () =>
            {
              
                if (SelectedItem == null)
                {
                    MessageBox.Show("Пожалуйста, выберите товар для редактирования.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                product = SelectedItem;
                new EditProduct(product).Show();
            });


            ComingProduct = new CommandVM(() =>
            {

                ComingProductWin cp = new ComingProductWin();
                cp.Show();
            });
            

               
        }

        private bool IsDecimalValid(string value)
        {
            foreach (char c in value)
            {
               
                if (!char.IsDigit(c) && c != '.' && c != ',' && c != '-')
                {
                    return false;
                }
            }

           
            if (value.Count(c => c == '.' || c == ',') > 1)
            {
                return false;
            }

            return true;
        }





        public List<ProductCategory> productCategories { get; set; }
        private ProductCategory listProductCategory;
        private byte[]? image;
        private decimal _postavPrice;
        private decimal _markup;

        public ProductCategory ListProductCategory
        {
            get => listProductCategory;
            set
            {
                listProductCategory = value;
                Signal();
            }
        }

    

        public async Task che()
        {
            string json1 = await Api.Post("Products", null, "get");
            var result1 = Api.Deserialize<List<Product>>(json1);
            products = result1;
            Signal(nameof(products));

            string json = await Api.Post("ProductCategories", null, "get");
            var result = Api.Deserialize<List<ProductCategory>>(json);
            productCategories = result;
            Signal(nameof(productCategories));

            


        }
        //ко всем потом надо!
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




    }
}
