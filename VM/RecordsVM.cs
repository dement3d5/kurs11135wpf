using kurs11135.DB;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace kurs11135.VM
{
    public class RecordsVM
    {
        public SeriesCollection SeriesCollection { get; set; }
        public Func<double, string> Formatter { get; set; }
        public List<string> DateLabels { get; set; }

        public RecordsVM()
        {
            SeriesCollection = new SeriesCollection();
            Formatter = value => value.ToString("C");

            using (var dbContext = new user1Context())
            {
                var orders = dbContext.Orders.Include(o => o.OrderProducts).ToList();
                var products = dbContext.Products.ToList();

                CalculateRevenueAndExpenses(orders, products);

                DateLabels = orders
                    .Select(o => o.CreateAt.Value.Date.ToString("yyyy-MM-dd"))
                    .Distinct()
                    .OrderBy(d => d)
                    .ToList();

                AddDataToSeriesCollection("Выручка", orders.Select(o => o.CreateAt.Value.Date).Distinct().OrderBy(d => d),
    date => orders.Where(o => o.CreateAt.Value.Date == date).Sum(o => o.Cost ?? 0));

                AddDataToSeriesCollection("Затраты", orders.Select(o => o.CreateAt.Value.Date).Distinct().OrderBy(d => d),
                    date => orders.Where(o => o.CreateAt.Value.Date == date).Sum(o => CalculateExpenses(o, products)));
            }
        }

        private decimal CalculateExpenses(Order order, List<Product> products)
        {
            decimal totalExpenses = 0;

            foreach (var orderProduct in order.OrderProducts)
            {
                var product = products.FirstOrDefault(p => p.Id == orderProduct.ProductId);
                if (product != null)
                {
                    if (decimal.TryParse(orderProduct.Count, out decimal count))
                    {
                        decimal postavPrice = product.PostavPriсе.Value;
                        totalExpenses += count * postavPrice;
                    }
                }
            }

            return totalExpenses;
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

        private void CalculateRevenueAndExpenses(List<Order> orders, List<Product> products)
        {
            foreach (var order in orders)
            {
                decimal totalRevenue = 0;
                decimal totalExpenses = 0;

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
                            totalExpenses += count * postavPrice;
                        }
                    }
                }
            }
        }
    }
}
