using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBot.Models.DbSets
{
    internal class Item
    {
        public Item(string color, string name, string type,  string photoUrl)
        {
            Color = color;
            Name = name;
            Type = type;
            CreationDate = CreationDate = DateTime.Now;
            PhotoUrl = photoUrl;                      
        }

        public int Id { get; set; }        
        public int Barcode { get; set; }
        public string Color { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public DateTime? CreationDate { get; set; }
        public string PhotoUrl { get; set; }
        public string Size { get; set; }
        public int Leftover { get; set; }
        public Client? Owner { get; set; }
        public Order? Order { get; set; } 
    }
}
