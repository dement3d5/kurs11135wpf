
using GalaSoft.MvvmLight.CommandWpf;
using kurs11135.Models;
using kurs11135.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace kurs11135.VM
{
    class CatalogVM : BaseVM
    {

        public Product SelectedItem { get; set; }
        public List<Product> products { get; set; }
        public Product product { get; set; }


       

        public byte[]? Image { get => image; set { image = value; Signal(); } }
        public string NameProduct { get; set; }
        public string ShortName { get; set; }
        public string Quantity { get; set; }


        private void MinButton_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void DecrementButton_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void EqualButton_Click(object sender, RoutedEventArgs e)
        {
            //// Логика присваивания определенного значения количеству
            //// Пример:
            //int newValue = 10; // Присваиваем 10, например
            //Quantity = newValue.ToString();
        }

        private void MaxButton_Click(object sender, RoutedEventArgs e)
        {
            
        }

        public ICommand MinQuantityCommand { get; }
        public ICommand DecreaseQuantityCommand { get; }
        public ICommand IncreaseQuantityCommand { get; }
        public ICommand MaxQuantityCommand { get; }



        private void MinQuantity(Product product)
        {
            // Логика уменьшения количества
            // Пример:
            int currentQuantity = int.Parse(Quantity); // Предполагается, что Quantity может быть преобразован в int
            if (currentQuantity > 0)
            {
                currentQuantity--;
                Quantity = currentQuantity.ToString();
                Signal();
            }
        }

        private void DecreaseQuantity(Product product)
        {
            // Логика уменьшения количества на более произвольное значение
            // Пример:
            int currentQuantity = int.Parse(Quantity); // Предполагается, что Quantity может быть преобразован в int
            int decrementValue = 5; // Уменьшение на 5, например
            if (currentQuantity >= decrementValue)
            {
                currentQuantity -= decrementValue;
                Quantity = currentQuantity.ToString();
                Signal();
            }
            Signal();
        }

        private void IncreaseQuantity(Product product)
        {
            int currentQuantity = int.Parse(Quantity); // Предполагается, что Quantity может быть преобразован в int
            int decrementValue = 5; // Уменьшение на 5, например
            if (currentQuantity <= decrementValue)
            {
                currentQuantity += decrementValue;
                Quantity = currentQuantity.ToString();
                Signal();
            }
            Signal();
        }

        private void MaxQuantity(Product product)
        {
            // Логика установки максимального значения количества
            // Пример:
            int maxValue = 100; // Максимальное значение
            Quantity = maxValue.ToString();
            Signal();
        }







            public CatalogVM()
        {
            MinQuantityCommand = new RelayCommand<Product>(MinQuantity); Signal();
            DecreaseQuantityCommand = new RelayCommand<Product>(DecreaseQuantity); Signal();
            IncreaseQuantityCommand = new RelayCommand<Product>(IncreaseQuantity); Signal();
            MaxQuantityCommand = new RelayCommand<Product>(MaxQuantity); Signal();



            Task.Run(async () =>
            {
                await che();
            });


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

        public List<ProductCategory> productCategories { get; set; }
        private ProductCategory listProductCategory;
        private byte[]? image;


        public ProductCategory ListProductCategory
        {
            get => listProductCategory;
            set
            {
                listProductCategory = value;
                Signal();
            }
        }


        



        

    }





}
