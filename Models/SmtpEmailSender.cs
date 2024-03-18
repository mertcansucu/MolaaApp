using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace MolaaApp.Models
{
    //IEmailsender ı implement edecek bir metod,bu metodu içerecek concrete versiyonu yani bu interface in implement edildiği versiyonu
    //EmailSender sınıfı, IEmailSender arayüzünün “concrete” yani somut bir uygulamasını temsil eder.
    public class SmtpEmailSender : IEmailSender
    {
        private string? _host;
        private int _port;
        private bool _enableSSL;
        private string? _username;
        private string? _password;
        public SmtpEmailSender(string? host,int port,bool enableSSL,string? username,string? password)
        {
            _host = host;
            _port = port;
            _enableSSL = enableSSL;
            _username = username;
            _password = password;
        }
        public Task SendEmailAsync(string email, string subject, string message)
        {
            var client = new SmtpClient(_host,_port)
            {
                Credentials = new NetworkCredential(_username, _password),
                EnableSsl = _enableSSL
            };

            return client.SendMailAsync(new MailMessage(_username ?? "", email, subject, message) {IsBodyHtml = true});
        }
    }
}