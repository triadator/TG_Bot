using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBot.Models.DbSets
{
    internal class ClientSizeInfo
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        
        public string? OverDressSize { get; set; }
        public string? DressSize { get; set; }
        public string? T_ShortSize { get; set; }
        public string? TrousersSize { get; set; }
        
    }
}
