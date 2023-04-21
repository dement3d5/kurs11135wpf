using kurs11135.Models;
using kurs11135.Tools;
using kurs11135.VM;
using System.Windows;

namespace kurs11135
{
    /// <summary>
    /// Логика взаимодействия для AddProduct.xaml
    /// </summary>
    public partial class AddProduct : Window
    {
        public AddProduct()
        {
            InitializeComponent();
            DataContext = new AddProdVM();
        }

    }
}
