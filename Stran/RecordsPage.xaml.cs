using kurs11135.VM;
using System.Windows.Controls;

namespace kurs11135.Stran
{
    public partial class RecordsPage : Page
    {
        public RecordsPage()
        {
            InitializeComponent();
            DataContext = new RecordsVM();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataContext is RecordsVM viewModel)
            {
                viewModel.LoadData();
            }
        }
    }
}
