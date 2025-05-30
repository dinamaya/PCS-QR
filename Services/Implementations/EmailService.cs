using CCIMS.Web.App_Code._Globals;
using CCIMS.Web.App_Code._Globals.Constants;
using CCIMS.Web.Models.Complex;
using CCIMS.Web.Models.DTOs;
using CCIMS.Web.Models.ViewModels;
using CCIMS.Web.Repositories.Interfaces;
using CCIMS.Web.Services.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MimeKit;
using Scriban;
using Scriban.Syntax;

namespace CCIMS.Web.Services.Implementations
{
  public class EmailService : IEmailService
  {
    private readonly string _agedTemplatePath = Path.Combine("App_Code", "Scriban", "Templates", "CaseAgedSpaEmail.sbn");
    private readonly string _custTemplatePath = Path.Combine("App_Code", "Scriban", "Templates", "CaseCreationCustomerEmail.sbn");
    private readonly string _spaTemplatePath = Path.Combine("App_Code", "Scriban", "Templates", "CaseCreationSPAEmail.sbn");

    private readonly EmailCredential _dev;
    private readonly EmailCredential _prod;
    private readonly IEnumerable<string> _testEmails;
    private readonly ILogger<EmailService> _logger;
    private readonly IConfigurationRepository _configRepo;
    private readonly Server _server;


    public EmailService(IOptions<EmailServiceConfig> config, ILogger<EmailService> logger, IConfigurationRepository configRepo, Server server)
    {
      _dev = config.Value.Credentials["Dev"];
      _prod = config.Value.Credentials["Prod"];
      _testEmails = config.Value.TestEmails;
      _logger = logger;
      _server = server;
      _configRepo = configRepo;
    }


    public async Task TestSendCaseCreationEmailAsync(string email, string caseNumber, string sp, string sn, string customerName)
    {
      string baseUrl = _configRepo.GetBaseUrl();
      var icons = Path.Combine(_server.RootDirectory, "img", "icons");
      var illus = Path.Combine(_server.RootDirectory, "img", "illustrations");

      var custModel = new CustomerEmailDetailsViewModel(_configRepo, caseNumber)
      {
        Email = email,
        ServicePartner = sp,
        Fullname = customerName,
      };

      var spaModel = new SpaCaseCreationEmailDetailsViewModel(_configRepo, caseNumber)
      {
        ServicePartner = sp,
        CustomerName = customerName,
        SerialNumber = sn,
      };

      var agedModel = new CaseAgedEmailDetailsViewModel
      {
        BaseUrl = baseUrl,
        Cases = Enumerable.Range(1, 50).Select(i => new AgedCaseViewModel
        {
          CaseTrackingLink = $"{baseUrl}Cases/Tracking?refNo=CC1000{i:D3}",
          CaseNumber = $"CC1000{i:D3}",
          DateLastUpdated = DateTime.UtcNow.AddDays(-i).ToString("yyyy-MM-dd"),
          CustomerName = $"Customer {i}",
          ServicePartner = $"SP Name{i}"
        }).ToList()
      };

      var customerHtmlBody = await RenderEmailAsync(_custTemplatePath, custModel);
      //var spaHtmlBody = await RenderEmailAsync(spaTemplatePath, spaModel);
      var agedHtmlBody = await RenderEmailAsync(_agedTemplatePath, agedModel);

      await CreateEmailAsync(
        custModel.Email,
        $"Test CCIMS - Customer Case Submitted by {custModel.Fullname}",
        customerHtmlBody,
        _dev
      );

      //await CreateEmailAsync(
      //  custModel.Email,
      //  $"Test CCIMS - SPA Copy {custModel.Fullname}",
      //  spaHtmlBody,
      //  _dev
      //);

      await CreateEmailAsync(
        custModel.Email,
        $"Test CCIMS - Aged Copy {custModel.Fullname}",
        agedHtmlBody,
        _dev
      );
    }

    public async Task SendCustomerRegistrationNotificationAsync(CreateCustomerDto customerDto, string caseNumber)
    {
      try
      {
        var emailDetails = new CustomerEmailDetailsViewModel(_configRepo, caseNumber)
        {
          Email = customerDto.Email,
          Fullname = $"{customerDto.FirstName} {customerDto.LastName}",
          ServicePartner = customerDto.ServicePartner
        };

        //await TestSendCaseCreationEmailAsync(emailDetails).ConfigureAwait(false);
        _logger.LogInformation($"Email notification sent successfully for case: {caseNumber}");
      }
      catch (Exception ex)
      {
        _logger.LogError($"Failed to send email notification for case {caseNumber}: {ex.Message}");

      }
    }

    public async Task SendAgedCasesEmailAsync(CaseAgedEmailDetailsViewModel agedCases)
    {
      var agedHtmlBody = await RenderEmailAsync(_agedTemplatePath, agedCases);

      await CreateEmailAsync(
        null,
        $"Test CCIMS - Aged Cases {DateTime.Now.ToLocalTime().ToString(Database.DateFormat.DISPLAY_COMPLETE)}",
        agedHtmlBody,
        _dev
      );
    }


    private async Task CreateEmailAsync(string to, string subject, string htmlBody, EmailCredential credential, IEnumerable<EmailAttachment>? attachments = null)
    {
      var message = new MimeMessage();
      message.From.Add(new MailboxAddress("Customer Carry-In Monitoring System", credential.SenderEmailAddress));

      foreach (var recipient in _testEmails)
        message.Cc.Add(new MailboxAddress("", recipient));

      if(!to.IsNullOrEmpty())
        message.To.Add(new MailboxAddress("", to));

      message.ReplyTo.Add(new MailboxAddress("", credential.ReplyAddress));

      message.Subject = subject;

      var bodyBuilder = new BodyBuilder
      {
        HtmlBody = htmlBody
      };

      if(attachments != null)
      {
        foreach (var a in attachments)
        {
          if (!File.Exists(a.Path)) continue;

          var resource = bodyBuilder.LinkedResources.Add(a.Path);
          resource.ContentId = a.ContentId;
        }
      }

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

    private async Task<string> RenderEmailAsync<T>(string templatePath, T model)
    {
      try
      {
        var templateText = await File.ReadAllTextAsync(templatePath);
        var template = Template.Parse(templateText);

        if (template.HasErrors)
          throw new InvalidOperationException($"Scriban template parse error: {string.Join(", ", template.Messages.Select(m => m.Message))}");
         
        var html = template.Render(model, member => member.Name);
        return html;
      }
      catch (ScriptRuntimeException ex)
      {
        Console.WriteLine("Scriban error: " + ex.Message);
        return "";
      }
    }
  }
}
