using kurs11135.VM;
using System;
using System.Data.SqlTypes;
using System.Windows;
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

        private void BuildRangeChart_Click(object sender, System.Windows.RoutedEventArgs e)
        {

            if (FromDatePicker.SelectedDate == null || ToDatePicker.SelectedDate == null)
            {
                MessageBox.Show("Пожалуйста, выберите даты \"От\" и \"До\" для построения графика.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            DateTime fromDate = FromDatePicker.SelectedDate.Value;
            DateTime toDate = ToDatePicker.SelectedDate.Value;

            // Проверяем, что дата не слишком старая
            if (fromDate < SqlDateTime.MinValue.Value || toDate < SqlDateTime.MinValue.Value)
            {
                MessageBox.Show("Выбранная дата слишком старая. Пожалуйста, выберите более поздние даты.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }


            if (DataContext is RecordsVM viewModel)
            {

                viewModel.BuildChartByDateRange(fromDate, toDate);
            }
        }

    }
}
