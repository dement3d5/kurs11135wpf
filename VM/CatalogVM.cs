
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

    
        public List<Product> products { get; set; }
        public Product product { get; set; }
        public Product SelectedItem { get; set; }

        public byte[]? Image { get => image; set { image = value; Signal(); } }
        public string NameProduct { get; set; }
        public string ShortName { get; set; }




        public CatalogVM()
        {
            Quantity = 0;
            che();
       

        }
        private ICommand _decreaseQuantityCommand;
        public ICommand DecreaseQuantityCommand => _decreaseQuantityCommand ??
            (_decreaseQuantityCommand = new RelayCommand(DecreaseQuantityAction, CanDecreaseQuantity));

        private ICommand _increaseQuantityCommand;
        public ICommand IncreaseQuantityCommand => _increaseQuantityCommand ??
            (_increaseQuantityCommand = new RelayCommand(IncreaseQuantityAction, CanIncreaseQuantity));
        private void DecreaseQuantityAction()
        {
            if (Quantity > 0)
            {
                Quantity--;
                // Здесь должна быть логика обновления количества товара в базе данных
            }
        }
        private bool CanDecreaseQuantity() => Quantity > 0;

        private void IncreaseQuantityAction()
        {
            if (Quantity < GetMaxQuantity()) // Функция GetMaxQuantity() должна возвращать макс. кол-во товара из БД
            {
                Quantity++;
                // Здесь должна быть логика обновления количества товара в базе данных
            }
        }
        private bool CanIncreaseQuantity() => Quantity < GetMaxQuantity();

        private int GetMaxQuantity()
        {
            // Этот метод должен получать максимальное количество товара из БД для SelectedProduct
            // В этом примере предполагается, что метод Quantity из модели Product возвращает integer.

            // Здесь временный код, вам нужно реализовать логику получения данных из БД.
            // Если поле Quantity модели Product - это строка, вам нужно будет её преобразовать в int
            return SelectedItem != null && int.TryParse(SelectedItem.Quantity, out int maxQuantity) ? maxQuantity : 0;
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
