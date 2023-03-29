using System.Windows.Controls;

namespace kurs11135.Tools
{
    public class MainVM : BaseVM
    {
        public Page currentPage;
        public CommandVM Order { get; set; }
        public CommandVM Product { get; set; }


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

         
        }

    }
}
