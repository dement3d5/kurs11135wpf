using kurs11135.Models;
using kurs11135.okna;
using kurs11135.Tools;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
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
            // Логика вычисления цены продажи по цене поставщика и наценке
            SellPrice = PostavPriсе * (1 + Markup / 100);
            Signal(nameof(SellPrice));
        }


        public AddProdVM()
        {
          


           

            SaveButton = new CommandVM( async () =>
            {
                var json3 = await Api.Post("ProductImages", new ProductImage { Image = Image }, "get");
                var image = Api.Deserialize<ProductImage>(json3);
                CalculateSellPrice();

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
                MessageBox.Show("Скорее всего добавилось");

                Task.Run(async () =>
                {
                    await che();
                });
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
                var json1 = await Api.Post("Products", SelectedItem.Id , "delete");
                che();

            });

            EditProduct = new CommandVM(async () =>
            {
              
               product = SelectedItem; 
                new EditProduct(product).Show();
               
            });


            

               
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
       




    }
}
