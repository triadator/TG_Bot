using System;
using System.Net;
using System.Net.Mail;


namespace TelegramBot
{
    internal class EmailHelper
    {
        public static void WriteErrorLog(Exception ex)
        {
          
            string path = @"C:\Users\Виктория\source\repos\TelegramBot";
            using (StreamWriter sw = (File.Exists(path)) ? File.AppendText(path) : File.CreateText(path))
            {
                sw.WriteLine(DateTime.Now.ToString() + ": " + ex.Source.ToString().Trim() + "; " + ex.Message.ToString().Trim());
                sw.Flush();
                sw.Close();
            }
          
        }

        public static void WriteMessageLog(string Message)
        {
            try
            {
                string path = @"C:\Users\Виктория\source\repos\TelegramBot.txt";
                using (StreamWriter sw = (File.Exists(path)) ? File.AppendText(path) : File.CreateText(path))
                {
                    sw.WriteLine(DateTime.Now.ToString() + ": " + Message);
                    sw.Flush();
                    sw.Close();
                }
            }
            catch (Exception ex)
            {
                WriteErrorLog(ex);
            }
        }

        public static void SendEmail(string body,string subject)
        {
            try
            {
                using (var mail = new MailMessage())
                {
                    mail.From = new MailAddress("triadator1@mail.ru"); 
                    mail.To.Add(new MailAddress("triadator1@mail.ru")); 
                    mail.Subject = subject;
                    mail.Body = body;

                    SmtpClient client = new SmtpClient("smtp.mail.ru", 587);
                    client.EnableSsl = true;
                    client.Credentials = new NetworkCredential("triadator1@mail.ru", "z5rmmnH78drVPQKcq9XH");
                    client.Send(mail);
                }
            }
            catch (Exception ex)
            {
                WriteErrorLog(ex);
            }
        } 
    }
}
