using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Controls;
using System.Windows.Media;

namespace kurs11135.Converters
{
    public class AlternatingBackgroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ListViewItem listViewItem = value as ListViewItem;
            if (listViewItem != null)
            {
                ListView listView = ItemsControl.ItemsControlFromItemContainer(listViewItem) as ListView;
                if (listView != null)
                {
                    int index = listView.ItemContainerGenerator.IndexFromContainer(listViewItem);
                    return index % 2 == 0 ? Brushes.White : new SolidColorBrush(Color.FromRgb(230, 230, 230));
                }
            }
            return Brushes.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
