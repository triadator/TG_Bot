using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramBot.Models.DbSets;

namespace TelegramBot.Models
{
    internal class BotContext : DbContext
    {
        public DbSet<Client> Clients { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<ClientSizeInfo> ClientSizeInfo { get; set; }
        public DbSet<CashReceipt> CashReceipts { get; set; }

        public BotContext()
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
            bool canConnect = Database.CanConnect();
            if (canConnect)
            Console.WriteLine("Произошло соединение с БД.");
            else 
                Console.WriteLine("Ошибка соединения с БД.");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        { optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=TelegramBotDb; Trusted_Connection=True;"); }


    }
}
