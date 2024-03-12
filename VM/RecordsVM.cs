using System;
using System.Collections.Generic;
using System.Linq;
using kurs11135;
using kurs11135.Tools;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using LiveCharts;
using kurs11135.DB;

namespace kurs11135.VM
{
    public class RecordsVM : BaseVM
    {
        public SeriesCollection SeriesCollection { get; set; }
        public Func<double, string> Formatter { get; set; }

        public RecordsVM()
        {
            SeriesCollection = new SeriesCollection();
            Formatter = value => value.ToString("C");

            using (var dbContext = new user1Context()) 
            {
                var orders = dbContext.Orders.ToList();
                var products = dbContext.Products.ToList();

        
                CalculateRevenueAndProfit(orders, products);

         
                AddDataToSeriesCollection("Выручка", orders.Select(o => o.CreateAt.Value.Date).Distinct().OrderBy(d => d),
                    date => orders.Where(o => o.CreateAt.Value.Date == date).Sum(o => o.Cost ?? 0));

                AddDataToSeriesCollection("Прибыль", orders.Select(o => o.CreateAt.Value.Date).Distinct().OrderBy(d => d),
                    date => orders.Where(o => o.CreateAt.Value.Date == date).Sum(o => CalculateProfit(o, products)));
            }
        }




        private decimal CalculateProfit(Order order, List<Product> products)
        {
            decimal totalProfit = 0;

            foreach (var orderProduct in order.OrderProducts)
            {
                var product = products.FirstOrDefault(p => p.Id == orderProduct.ProductId);
                if (product != null)
                {

                    if (decimal.TryParse(orderProduct.Count, System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.CultureInfo.InvariantCulture, out decimal count))
                    {
                        decimal postavPrice = product.PostavPriсе.Value;
                        totalProfit += count * (product.SellPrice.Value - postavPrice);
                        Signal();
                    }
                }
            }

            return totalProfit;
            Signal();
        }




        private void AddDataToSeriesCollection(string title, IEnumerable<DateTime> dates, Func<DateTime, decimal> valueSelector)
        {
            var values = new ChartValues<ObservableValue>();

            foreach (var date in dates)
            {
                values.Add(new ObservableValue((double)valueSelector(date)));
            }

            SeriesCollection.Add(new LineSeries { Title = title, Values = values, DataLabels = true, LabelPoint = point => $"{point.Y:C}" });
        }

        private void CalculateRevenueAndProfit(List<Order> orders, List<Product> products)
        {
            foreach (var order in orders)
            {
                decimal totalRevenue = 0;
                decimal totalProfit = 0;

                foreach (var orderProduct in order.OrderProducts)
                {
                    var product = products.FirstOrDefault(p => p.Id == orderProduct.ProductId);
                    if (product != null)
                    {

                        if (decimal.TryParse(orderProduct.Count, out decimal count) &&
                            product.SellPrice.HasValue && product.PostavPriсе.HasValue)
                        {
                            decimal postavPrice = product.PostavPriсе.Value;
                            totalRevenue += count * product.SellPrice.Value;
                            totalProfit += count * (product.SellPrice.Value - postavPrice);
                        }
                    }
                }

            }
        }

    }
}
