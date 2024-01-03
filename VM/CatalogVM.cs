
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


        private int _quantity;

        public int Quantity
        {
            get => _quantity;
            set
            {
                if (_quantity != value)
                {
                    _quantity = value;
                    Signal(nameof(Quantity));
                }
            }
        }

        public Product SelectedItem { get; set; }
        public List<Product> products { get; set; }
        public Product product { get; set; }


       

        public byte[]? Image { get => image; set { image = value; Signal(); } }
        public string NameProduct { get; set; }
        public string ShortName { get; set; }




        public ICommand MinQuantityCommand { get; set; }
        public ICommand DecreaseQuantityCommand { get; set; }
        public ICommand IncreaseQuantityCommand { get; set; }
        public ICommand MaxQuantityCommand { get; set; }

        private void SetMinQuantity(Product product)
        {
            Quantity = 1;
            Task.Run(async () =>
            {
                await che();
            });
        }
        private void DecreaseQuantity(Product product)
        {
            if (Quantity > 1)
            {
                Quantity--;
                Task.Run(async () =>
                {
                    await che();
                });
            }
            else
            {
                MessageBox.Show("Ошибка: Количество не может быть меньше 1");
                Task.Run(async () =>
                {
                    await che();
                });
            }
        }
        private void IncreaseQuantity(Product product)
        {

            Quantity++;
            Task.Run(async () =>
            {
                await che();
            });
        }
        private void SetMaxQuantity(Product product)
        {
            // Предположим, что у вас есть переменная maxQuantity, содержащая максимальное значение Quantity из базы данных
            int maxQuantity = 100; // Пример максимального значения из базы данных

            if (Quantity < maxQuantity)
            {
                Quantity = maxQuantity;
                Task.Run(async () =>
                {
                    await che();
                });
            }
            else
            {
                MessageBox.Show($"Ошибка: На складе имеется максимальное количество товара ({Quantity})");
                Task.Run(async () =>
                {
                    await che();
                });
            }
            Task.Run(async () =>
            {
                await che();
            });
        }

        public CatalogVM()
        {
            MinQuantityCommand = new RelayCommand<Product>(SetMinQuantity);
            DecreaseQuantityCommand = new RelayCommand<Product>(DecreaseQuantity);
            IncreaseQuantityCommand = new RelayCommand<Product>(IncreaseQuantity);
            MaxQuantityCommand = new RelayCommand<Product>(SetMaxQuantity);

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
