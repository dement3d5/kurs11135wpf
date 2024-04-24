using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using kurs11135.DB;
using kurs11135.Tools;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using Microsoft.EntityFrameworkCore;

namespace kurs11135.VM
{
    public class RecordsVM : BaseVM
    {
        public SeriesCollection SeriesCollection { get; set; }
        public Func<double, string> Formatter { get; set; }
        private List<string> _dateLabels;
        public List<string> DateLabels
        {
            get => _dateLabels;
            set
            {
                _dateLabels = value;
                Signal(nameof(DateLabels));
            }
        }

        private int _totalProductCount;
        public int TotalProductCount
        {
            get => _totalProductCount;
            set
            {
                _totalProductCount = value;
                Signal(nameof(TotalProductCount));
            }
        }

        public decimal TotalRevenue { get; set; }
        public decimal TotalExpenses { get; set; }
        public decimal AllProductPrice { get; set; }

        private decimal _totalProfit;
        public decimal TotalProfit
        {
            get => _totalProfit;
            set
            {
                _totalProfit = value;
                Signal(nameof(TotalProfit));
            }
        }
        public List<string> AvailableMonths { get; set; }
        public string SelectedMonth { get; set; }

        public RecordsVM()
        {
            SeriesCollection = new SeriesCollection();
            Formatter = value => value.ToString("C");
            DateLabels = new List<string>();
            AvailableMonths = new List<string>();
            SelectedMonth = DateTime.Today.ToString("yyyy-MM");
            FillAvailableMonths();
            LoadData();
        }

        private void FillAvailableMonths()
        {
            using (var dbContext = new user1Context())
            {
                AvailableMonths = dbContext.Orders
                    .Select(o => new DateTime(o.CreateAt.Value.Year, o.CreateAt.Value.Month, 1).ToString("yyyy-MM"))
                    .Distinct()
                    .ToList();
            }
        }

        public void CalculateTotalProductPrice(List<Product> products)
        {
            decimal totalProductPrice = 0;

            foreach (var product in products)
            {
                if (int.TryParse(product.Quantity, out int quantity) &&
                    product.PostavPriсе.HasValue)
                {
                    decimal postavPrice = product.PostavPriсе.Value;
                    totalProductPrice += quantity * postavPrice;
                }
            }

            AllProductPrice = totalProductPrice;
        }



        public void LoadData()
        {
            var selectedDate = DateTime.ParseExact(SelectedMonth, "yyyy-MM", CultureInfo.InvariantCulture);
            using (var dbContext = new user1Context())
            {
                var orders = dbContext.Orders
                    .Include(o => o.OrderProducts)
                    .Where(o => o.CreateAt.Value.Year == selectedDate.Year && o.CreateAt.Value.Month == selectedDate.Month)
                    .ToList();
                var products = dbContext.Products.ToList();
                SeriesCollection.Clear();
                TotalRevenue = 0;
                TotalExpenses = 0;
                TotalProfit = 0;
                DateLabels.Clear();
                DateLabels = orders
                    .GroupBy(o => o.CreateAt.Value.Date)
                    .OrderBy(g => g.Key)
                    .Select(g => g.Key.ToString("yyyy-MM-dd"))
                    .ToList();
                CalculateTotalProductPrice(products);
                AddDataToSeriesCollection("Выручка", orders.Select(o => o.CreateAt.Value.Date).Distinct().OrderBy(d => d),
                    date => orders.Where(o => o.CreateAt.Value.Date == date).Sum(o => o.Cost ?? 0));

                AddDataToSeriesCollection("Затраты", orders.Select(o => o.CreateAt.Value.Date).Distinct().OrderBy(d => d),
                    date => orders.Where(o => o.CreateAt.Value.Date == date).Sum(o => CalculateExpenses(o, products)));

                AddDataToSeriesCollection("Прибыль", orders.Select(o => o.CreateAt.Value.Date).Distinct().OrderBy(d => d),
                    date => orders.Where(o => o.CreateAt.Value.Date == date).Sum(o => (o.Cost ?? 0) - CalculateExpenses(o, products)));
                TotalProfit = TotalRevenue - TotalExpenses;
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
                var value = (double)valueSelector(date);
                values.Add(new ObservableValue(value));

                if (title == "Выручка")
                    TotalRevenue += (decimal)value;
                else if (title == "Затраты")
                    TotalExpenses += (decimal)value;
            }

            var series = new LineSeries { Title = title, Values = values, DataLabels = true, LabelPoint = point => $"{point.Y:C}" };
            if (title == "Прибыль")
                series.Stroke = System.Windows.Media.Brushes.Green;

            SeriesCollection.Add(series);
        }


        public void BuildChartByDateRange(DateTime fromDate, DateTime toDate)
        {
            using (var dbContext = new user1Context())
            {
                var orders = dbContext.Orders
                    .Include(o => o.OrderProducts)
                    .Where(o => o.CreateAt.Value.Date >= fromDate.Date && o.CreateAt.Value.Date <= toDate.Date)
                    .ToList();
                var products = dbContext.Products.ToList();

                SeriesCollection.Clear();
                DateLabels.Clear();
                TotalRevenue = 0;
                TotalExpenses = 0;

                DateLabels = orders
                    .GroupBy(o => o.CreateAt.Value.Date)
                    .OrderBy(g => g.Key)
                    .Select(g => g.Key.ToString("yyyy-MM-dd"))
                    .ToList();

                CalculateTotalProductPrice(products);
                AddDataToSeriesCollection("Выручка", orders.Select(o => o.CreateAt.Value.Date).Distinct().OrderBy(d => d),
                    date => orders.Where(o => o.CreateAt.Value.Date == date).Sum(o => o.Cost ?? 0));

                AddDataToSeriesCollection("Затраты", orders.Select(o => o.CreateAt.Value.Date).Distinct().OrderBy(d => d),
                    date => orders.Where(o => o.CreateAt.Value.Date == date).Sum(o => CalculateExpenses(o, products)));

                AddDataToSeriesCollection("Прибыль", orders.Select(o => o.CreateAt.Value.Date).Distinct().OrderBy(d => d),
                    date => orders.Where(o => o.CreateAt.Value.Date == date).Sum(o => (o.Cost ?? 0) - CalculateExpenses(o, products)));

                TotalProfit = TotalRevenue - TotalExpenses;
            }
        }



        //private void CalculateRevenueAndExpenses(List<Order> orders, List<Product> products)
        //{
        //    foreach (var order in orders)
        //    {
        //        decimal totalRevenue = 0;
        //        decimal totalExpenses = 0;

        //        foreach (var orderProduct in order.OrderProducts)
        //        {
        //            var product = products.FirstOrDefault(p => p.Id == orderProduct.ProductId);
        //            if (product != null)
        //            {
        //                if (decimal.TryParse(orderProduct.Count, out decimal count) &&
        //                    product.SellPrice.HasValue && product.PostavPriсе.HasValue)
        //                {
        //                    decimal postavPrice = product.PostavPriсе.Value;
        //                    decimal markupPercent = (decimal)(product.Markup ?? 0);
        //                    decimal markedUpPrice = (decimal)(postavPrice * (1 + markupPercent / 100));
        //                    decimal sellPrice = product.SellPrice.Value;
        //                    totalRevenue += count * sellPrice;
        //                    totalExpenses += count * markedUpPrice;
        //                }
        //            }
        //        }

        //        TotalRevenue += totalRevenue;
        //        TotalExpenses += totalExpenses;
        //    }

        //    TotalProfit = TotalRevenue - TotalExpenses;
        //}
    }
}
