using CCIMS.Web.App_Code._Globals.Constants;
using CCIMS.Web.App_Code._Globals.Renderers;
using CCIMS.Web.Models.Complex;
using CCIMS.Web.Models.ViewModels;
using CCIMS.Web.Services.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using MimeKit;
using NuGet.Protocol.Plugins;

namespace CCIMS.Web.Services.Implementations
{
  public class EmailService : IEmailService
  {
    private readonly EmailCredential _dev;
    private readonly EmailCredential _prod;
    private readonly IEnumerable<string> _testEmails;

    public EmailService(IOptions<EmailServiceConfig> config)
    {
      _dev = config.Value.Credentials["Dev"];
      _prod = config.Value.Credentials["Prod"];
      _testEmails = config.Value.TestEmails;
    }

    public async Task TestSendCaseCreationEmailAsync(CustomerEmailDetailsViewModel emailDetails)
    {
      var htmlBody = await new EmailTemplateRenderer().RenderTemplateAsync(Routes.Partials.EMAIL.TEST, emailDetails);
      await CreateEmailAsync(
        emailDetails.Email, 
        $"Test CCIMS - Case Filed by {emailDetails.Fullname}", 
        htmlBody, 
        _dev);
    }

    private async Task CreateEmailAsync(string to, string subject, string htmlBody, EmailCredential credential)
    {
      var message = new MimeMessage();
      message.From.Add(new MailboxAddress("Customer Carry-In Monitoring System", credential.SenderEmailAddress));

      foreach (var recipient in _testEmails)
        message.Cc.Add(new MailboxAddress("", recipient));

      message.To.Add(new MailboxAddress("", to));

      message.ReplyTo.Add(new MailboxAddress("", credential.ReplyAddress));

      message.Subject = subject;

      var bodyBuilder = new BodyBuilder
      {
        HtmlBody = htmlBody
      };

      message.Body = bodyBuilder.ToMessageBody();
      message.Priority = MessagePriority.Urgent;

      await SendEmailAsync(message, credential);
    }

    private async Task SendEmailAsync(MimeMessage message, EmailCredential credential)
    {
      using var client = new SmtpClient();
      await client.ConnectAsync(credential.Host, credential.Port, SecureSocketOptions.StartTls);
      await client.AuthenticateAsync(credential.Username, credential.Password);
      await client.SendAsync(message);
      await client.DisconnectAsync(true);
    }

  }
}
