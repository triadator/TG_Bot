using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBot.Models.DbSets
{
    internal class Order
    {
        public int Id { get; set; }
        public int? ClientId { get; set; }
        public Client? ClientName { get; set; }
        public CashReceipt? CashReceipt { get; set; }
        public DateTime OrderDate { get; set; }
        public List<Item>? Items { get; set; }
    }
}
