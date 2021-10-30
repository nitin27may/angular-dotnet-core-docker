using Application.DTOs.Email;
using Application.Exceptions;
using Application.Interfaces;
using Domain.Settings;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;
using SendGridSettings = Domain.Settings.SendGridSettings;

namespace Infrastructure.Shared.Services
{
    public class EmailService : IEmailService
    {
        public SendGridSettings _sendGridSettings { get; }
        public ILogger<EmailService> _logger { get; }

        public EmailService(IOptions<Domain.Settings.SendGridSettings> sendGridSettings,ILogger<EmailService> logger)
        {
            _sendGridSettings = sendGridSettings.Value;
            _logger = logger;
        }

        public async Task SendAsync(EmailRequest request)
        {
            try
            {


                var apiKey = _sendGridSettings.Key;
                var client = new SendGridClient(apiKey);
                var from = new EmailAddress(_sendGridSettings.EmailFrom, _sendGridSettings.DisplayName);
                var subject = request.Subject;
                var to = new EmailAddress(request.To);
               // var plainTextContent = "and easy to do anywhere, even with C#";
                var htmlContent = request.Body;
                var msg = MailHelper.CreateSingleEmail(from, to, subject, "", htmlContent);
                var response = await client.SendEmailAsync(msg);
                if (response.IsSuccessStatusCode != true)
                {
                    throw new ApiException($"Email not sent with status code { response.StatusCode }");
                }
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new ApiException(ex.Message);
            }
        }
    }
}
