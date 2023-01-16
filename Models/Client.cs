using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBot.Models.DbSets
{
    internal class Client
    {
        public int Id { get; set; }
        public long TelegramId { get; set; }
        [Required]
        public string? Name { get; set; }
        public string? Lastname { get; set; }
        public string? MiddleName { get; set; }
        public string? Phone { get; set; }
        [Required]
        public string? Email { get; set; }
        [Required]
        public ClientSizeInfo SizeInfo { get; set; }
        public List<Order>? Orders { get; set; }
        public List<Item>? Wardrobe { get; set; }
    }
}
