using kurs11135;
using kurs11135.okna;
using kurs11135.Tools;
using Microsoft.Win32;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace kurs11135.VM
{
    public class EditProdVM : BaseVM
    {
        public CommandVM AddProduct { get; set; }
        public CommandVM EditImage { get; set; }
        public CommandVM SaveButton { get; set; }
        public CommandVM EditProduct { get; set; }
        public Product SelectedItem { get; set; }


        private List<ProductCategory> productCategories;
        public List<ProductCategory> ProductCategories
        {
            get => productCategories; 
            set
            {
                productCategories = value;
                Signal();
            }
        }

        private ProductCategory selectedCategory;

        public ProductCategory SelectedCategory
        {
            get { return selectedCategory; }
            set
            {
                selectedCategory = value;
                Signal();
            }
        }

        public CommandVM DelProduct { get; set; }

        public byte[]? Image
        {
            get => image;
            set
            {
                image = value;
                Signal(nameof(Image));
            }
        }

        public string NameProduct
        {
            get => product?.ProductName;
            set
            {
                if(product != null)
                {
                    product.ProductName = value;
                    Signal();
                }
            }

        }
        public string ShortName
        {
            get => product?.ShortDescription;
            set
            {
                if (product != null)
                {
                    product.ShortDescription = value;
                    Signal();
                }
            }

        }
        public decimal PostavPriсе
        {
            get => (decimal)(product?.PostavPriсе);
            set
            {
                if (product != null)
                {
                    product.PostavPriсе = value;
                    Signal();
                }
            }

        }
        public decimal SellPrice
        {
            get => (decimal)(product?.SellPrice);
            set
            {
                if (product != null)
                {
                    product.SellPrice = value;
                    Signal();
                }
            }

        }

        public string Quantity
        {
            get => product?.Quantity;
            set
            {
                if (product != null)
                {
                    product.Quantity = value;
                    Signal();
                }
            }

        }
        public float Markup
        {
            get => (float)(product?.Markup);
            set
            {
                if (product != null)
                {
                    product.Markup = value;
                    Signal();
                }
            }

        }

      

        public EditProdVM(Product selectedProduct)
        {

            Task.Run(async () =>
            {
                await che();
                SelectedCategory = ProductCategories.FirstOrDefault(category => category.Id == selectedProduct.CategoryId);
            });

            product = selectedProduct;
            Image = selectedProduct.Image?.Image;
           
            SaveButton = new CommandVM(async () =>
             {
                 var json3 = await Api.Post("ProductImages", new ProductImage { Id = selectedProduct.Image.Id, Image = Image }, "put");
                 var json1 = await Api.Post("Products", new Product
                 {
                     Id = selectedProduct.Id,
                     CategoryId = SelectedCategory.Id,
                     ProductName = NameProduct,
                     PostavPriсе = PostavPriсе,
                     Quantity = Quantity,
                     Markup = Markup,
                     SellPrice = SellPrice,
                     ShortDescription = ShortName,
                     ImageId = selectedProduct.ImageId
                 }, "put");
                 che();
             });




            Task.Run(async () =>
            {
                await che();
            });

            AddProduct = new CommandVM(() =>
            {
                new AddProduct().Show();

            });
            EditImage = new CommandVM(async () =>
            {
                OpenFileDialog ofd = new();
                if (ofd.ShowDialog() == true)
                {
                    var bytes = File.ReadAllBytes(ofd.FileName);
                    Image = bytes;
                }
            });
            DelProduct = new CommandVM(async () =>
            {
                var json1 = await Api.Post("Products", SelectedItem.Id, "delete");
                //var result = Api.Deserialize<Product>(json1);
                //UpdateList();

            });
            ProductCategories = new List<ProductCategory>();
        }

     


        private byte[]? image;
        private List<ProductCategory> productCategory;

      


        public async Task che()
        {
            string json1 = await Api.Post("Products", null, "get");
            var result1 = Api.Deserialize<List<Product>>(json1);
            products = result1;
            Signal(nameof(products));
          

            string json = await Api.Post("ProductCategories", null, "get");
            var result = Api.Deserialize<List<ProductCategory>>(json);
            ProductCategories = result;
            Signal(nameof(ProductCategories));





        }
        public List<Product> products { get; set; }
        public Product product { get; set; }




    }
}
