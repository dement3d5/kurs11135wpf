﻿using kurs11135;
using kurs11135.okna;
using kurs11135.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace kurs11135
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
   
        public MainWindow(User user)
        {
            InitializeComponent();
            DataContext = new MainVM(user);

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AuthLog al = new AuthLog();
            al.Show();
            this.Close();
        }
    }
}
