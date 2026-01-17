using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using CoolCBackEnd.Interfaces;

namespace CoolCBackEnd.Service
{
    public class EmailService : IEmailService
    {
        private readonly SmtpClient _smtpClient;
        private readonly string _fromAddress;

        public EmailService(SmtpClient smtpClient, string fromAddress)
        {
            _smtpClient = smtpClient;
            _fromAddress = fromAddress;
        }

        public async Task SendEmailAsync(string toAddress, string subject, string emailBody)
        {
            var message = new MailMessage
            {
                From = new MailAddress(_fromAddress),
                Subject = "Verification Link",
                Body = emailBody,
                IsBodyHtml = true
            };
            message.To.Add(toAddress);

            try
            {
                await _smtpClient.SendMailAsync(message);
            }
            catch (Exception ex)
            {
                // Handle any errors here
                Console.WriteLine($"Error sending email: {ex.Message}");
            }
        }
    }

}