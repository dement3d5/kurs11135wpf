using System;
using System.Windows;
using System.Windows.Controls;

namespace kurs11135.Tools
{
    public class MainVM : BaseVM
    {
        public Page currentPage;
        public CommandVM Order { get; set; }
        public CommandVM Product { get; set; }
        public CommandVM Exit { get; set; }

        public Page CurrentPage
        {
            get => currentPage;
            set
            {
                currentPage = value;
                Signal();
            }
        }

        public MainVM()
        {
            CurrentPage = new hiPage(this);
            Order = new CommandVM(() =>
                {
                    CurrentPage = new Order1();

                });
             


            Product = new CommandVM(() =>
            {
                CurrentPage = new Product1();
            });

            Exit = new CommandVM(() =>
            {
                MessageBoxResult Result = MessageBox.Show("Закрыть приложение ?", "Уведомление", MessageBoxButton.YesNo);
                if (Result == MessageBoxResult.Yes)
                {
                    Application.Current.Shutdown();

                }
                else if (Result == MessageBoxResult.No)
                {
                    MessageBox.Show("Ну привет еще раз! :D");
                }

            });

         
        }

    }
}
