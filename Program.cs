using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBot.Models;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Telegram.Bot.Extensions.LoginWidget;
using TelegramBot.Models.DbSets;
using MySqlX.XDevAPI.Common;
using TelegramBot;

//using MailKit.Net.Smtp;
using MimeKit;
using System.Net.Mail;
using System.Net;

class Programm
{       
    //@Spb_Soiree_Bot
    static void Main(string[] args)
    {        
        TG_Bot bot = new TG_Bot();
        Console.ReadLine();       
    }
}

