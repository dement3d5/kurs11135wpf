
using GalaSoft.MvvmLight.CommandWpf;
using kurs11135;
using kurs11135.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using static kurs11135.VM.AddOrdVM;

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

        private string _searchQuery;

        public string SearchQuery
        {
            get { return _searchQuery; }
            set
            {
                if (_searchQuery != value)
                {
                    _searchQuery = value;
                    FilterProducts(); // После изменения поискового запроса перефильтровываем продукты
                    Signal(nameof(SearchQuery));
                }
            }
        }


        public List<ProductCategory> ProductCategories { get; set; }
        public List<Product> products { get; set; }
        public Product product { get; set; }
        public Product SelectedItem { get; set; }
        public List<Product> FilteredProducts { get; set; }
        public byte[]? Image { get => image; set { image = value; Signal(); } }
        public string NameProduct { get; set; }
        public string ShortName { get; set; }

        private async Task LoadProducts()
        {
            string json = await Api.Post("Products", null, "get");
            products = Api.Deserialize<List<Product>>(json);
            FilterProducts(); 
        }

        private void FilterProducts()
        {
            if (!string.IsNullOrEmpty(SearchQuery))
            {
           
                FilteredProducts = products.Where(p =>
                    p.ProductName.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase) ||
                    p.ShortDescription.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            else if (SelectedCategory != null && SelectedCategory.Id != -1)
            {
     
                FilteredProducts = products.Where(p => p.Category?.Id == SelectedCategory.Id).ToList();
            }
            else
            {
           
                FilteredProducts = products.ToList();
            }
            Signal(nameof(FilteredProducts));
        }


        private ProductCategory _selectedCategory;
        public ProductCategory SelectedCategory
        {
            get { return _selectedCategory; }
            set
            {
                if (_selectedCategory != value)
                {
                    _selectedCategory = value;
                    Signal(nameof(SelectedCategory));
                    FilterProducts(); 
                }
            }
        }



        //public void Cleanup()
        //{
        //    Mediator.OnProductQuantityChanged -= UpdateProductQuantity;
        //}

        public CatalogVM()
        {
          
            che();

        }

        private void UpdateProductQuantity(Product updatedProduct)
        {
            var productToUpdate = products.FirstOrDefault(p => p.Id == updatedProduct.Id);
            if (productToUpdate != null)
            {
                productToUpdate.Quantity = updatedProduct.Quantity;
                Signal(nameof(products));
            }

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
         
            }
        }
        private bool CanDecreaseQuantity() => Quantity > 0;

        private void IncreaseQuantityAction()
        {
            if (Quantity < GetMaxQuantity()) 
            {
                Quantity++;

            }
        }
        private bool CanIncreaseQuantity() => Quantity < GetMaxQuantity();

        private int GetMaxQuantity()
        {

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
            ProductCategories = result;



            AllProductCategories = new List<ProductCategory>(result);
            AllProductCategories.Insert(0, new ProductCategory { Id = -1, Name = "Все категории" });

            
            SelectedCategory = AllProductCategories.FirstOrDefault();

            Signal(nameof(AllProductCategories));
        }


        public List<ProductCategory> AllProductCategories { get; set; }

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
