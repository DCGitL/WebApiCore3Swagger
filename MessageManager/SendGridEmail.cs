using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Threading.Tasks;

namespace MessageManager
{
    public class SendGridEmail : ISendEmail
    {
        private readonly IConfiguration configuration;

        public SendGridEmail(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public async Task SendMail(string mailbody, EnumEmailType emailtype)
        {
            var apiKey = configuration.GetSection("SendGridApiKey").Value;
            var client = new SendGridClient(apiKey);

            var from = new EmailAddress("tony.peart62@gmail.com", "tony test");
            var to = new EmailAddress("davidl5a@hotmail.com");
            var subject = "This is a test";
            if(emailtype == EnumEmailType.html)
            {
                var msg = MailHelper.CreateSingleEmail(from, to, subject,"", mailbody);
                await client.SendEmailAsync(msg);
            }
            else
            {
                var msg = MailHelper.CreateSingleEmail(from, to, subject, mailbody, "");
                await client.SendEmailAsync(msg);
            }

           
        }
    }
}
