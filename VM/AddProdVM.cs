using kurs11135.Tools;

namespace kurs11135.VM
{
    public class AddProdVM : BaseVM
    {
        public CommandVM AddProduct { get; set; }
        public AddProdVM()
        {
            AddProduct = new CommandVM(() =>
            {
                new AddProduct().Show();

            });

        }
    }
}
