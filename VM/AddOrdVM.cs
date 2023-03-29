using kurs11135.Models;
using kurs11135.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kurs11135.VM
{
    public class AddOrdVM : BaseVM
    {
        public List<OrderStatus> orderStatus { get; set; } 
        public CommandVM AddOrder { get; set; }
        public AddOrdVM()
        {
            AddOrder = new CommandVM(() =>
            {
                new AddOrder().Show();
            });
            Task.Run(async () => { await init(); });
        }
        private async Task init()
        {
            string json = await Api.Post("OrderStatus", null, "get");
            List<OrderStatus> answer = Api.Deserialize<List<OrderStatus>>(json);
            orderStatus = answer;
            Signal(nameof(orderStatus));
        }
    }
}
/*saDSAD*/ 