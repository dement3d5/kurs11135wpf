using kurs11135.Tools;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace kurs11135.VM
{
    public class AddCategoryProductVM : BaseVM
    {
        public CommandVM SaveButton { get; set; }
        private string _categoryName;
        public string CategoryName
        {
            get { return _categoryName; }
            set
            {
                _categoryName = value;
                Signal(nameof(CategoryName));
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


        public AddCategoryProductVM()
        {
            SaveButton = new CommandVM(async() =>
            {

                var json1 = await Api.Post("ProductCategories", new ProductCategory
                {
                    Name = CategoryName

                }, "SaveCategory");
                ProductCategory result1 = Api.Deserialize<ProductCategory>(json1);

                MessageBox.Show("Товар успешно добавлен.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);


                CloseWindow();


            });

            


        }




    }
}
