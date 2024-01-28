using kurs11135.Models;
using kurs11135.okna;
using kurs11135.Stran;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace kurs11135.Tools
{
    public class MainVM : BaseVM
    {
        public Page currentPage;
        public CommandVM Order { get; set; }
        public CommandVM Product { get; set; }
        public CommandVM Logout { get; set; }
        public CommandVM Invoice { get; set; }
        public CommandVM CartPage1 { get; set; }
        public CommandVM CatalogPage1 { get; set; } 
        public CommandVM OrdersPage1 { get; set; }
        public CommandVM ProfilePage1 { get; set; }
        public Page CurrentPage
        {
            get => currentPage;
            set
            {
                currentPage = value;
                Signal();
            }
        }
        private User user = new User();
        public User User
        {
            get => user;
            set
            {
                user = value;
                Signal("UserName");
                    
                
            }
        }


        public string UserName
        {
            get => $"{User.FirstName} {User.LastName}";
        }

        public MainVM(User user)
        {
            User = user;
            CurrentPage = new hiPage(this);
            Invoice = new CommandVM(() =>
            {
                currentPage = new PrixodPage();
            });
            Order = new CommandVM(() =>
                {
                    CurrentPage = new Order1(User);

                });

            CartPage1 = new CommandVM(() =>
            {
                CurrentPage = new CartPage();
            });

            CatalogPage1 = new CommandVM(() =>
            {
                CurrentPage = new CatalogPage();
            });

            OrdersPage1 = new CommandVM(()  =>
            {
                CurrentPage = new OrdersPage(User);

            });
            ProfilePage1 = new CommandVM(() =>
            {
                CurrentPage = new ProfilePage1(User);
            });


            Product = new CommandVM(() =>
            {
                CurrentPage = new Product1();
            });

            Logout = new CommandVM(() =>
            {
                MessageBoxResult Result = MessageBox.Show("Закрыть приложение ?", "Уведомление", MessageBoxButton.YesNo);
                if (Result == MessageBoxResult.Yes)
                {
                    AuthLog log = new AuthLog();
                    log.Show(); 
                }
                else if (Result == MessageBoxResult.No)
                {
                    MessageBox.Show("Ну привет еще раз! :D");
                }

            });

         
        }

    }
}
