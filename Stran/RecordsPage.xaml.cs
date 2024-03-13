using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using kurs11135.VM;
using kurs11135.Tools;
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

     

    }
}
