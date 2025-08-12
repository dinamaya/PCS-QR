using CCIMS.Web.App_Code._Globals;
using CCIMS.Web.App_Code._Globals.Constants;
using CCIMS.Web.Models.Complex;
using CCIMS.Web.Models.DTOs;
using CCIMS.Web.Models.ViewModels;
using CCIMS.Web.Repositories.Interfaces;
using CCIMS.Web.Services.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
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
        private readonly string _closedtemplatePath = Path.Combine("App_Code", "Scriban", "Templates", "CaseClosedCustomerEmail.sbn");
        private readonly string _qrCodeTemplatePath = Path.Combine("App_Code", "Scriban", "Templates", "QrCodeEmail.sbn");

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

        public async Task<TaskResultDto> SendCustomerRegistrationNotificationAsync(CreateCustomerDto customerDto, string caseNumber, string encryptedCaseNumber)
        {
            try
            {
                string customer = customerDto.LastName + ", " + customerDto.FirstName;
                var icons = Path.Combine(_server.RootDirectory, "img", "icons");
                var illus = Path.Combine(_server.RootDirectory, "img", "illustrations");

                var custModel = new CustomerEmailDetailsViewModel(_configRepo, caseNumber, encryptedCaseNumber)
                {
                    Email = customerDto.Email,
                    ServicePartner = customerDto.ServicePartner,
                    Fullname = customer,
                };

                var htmlBody = await RenderEmailAsync(_custTemplatePath, custModel);

                await CreateEmailAsync(
                  customerDto.Email,
                  $"CCI Monitoring System - Case Registered",
                  htmlBody,
                  _dev,
                  null
                );

                return TaskResultDto.Success("Email Sent Successfully");
            }

            catch (Exception ex)
            {
                return TaskResultDto.Fail(Exceptions.GetMessage(ex));
            }
        }

        public async Task<TaskResultDto> SendAgedCasesEmailAsync(CaseAgedEmailDetailsViewModel agedCases)
        {
            var task = new TaskResultDto();
            try
            {
                var agedHtmlBody = await RenderEmailAsync(_agedTemplatePath, agedCases);

                await CreateEmailAsync(
                  null,
                  $"CCI Monitoring System - Aged Cases {DateTime.Now.ToLocalTime().ToString(Database.DateFormat.DISPLAY_COMPLETE)}",
                  agedHtmlBody,
                  _dev
                );

                return TaskResultDto.Success("Email Sent Successfully");
            }
            catch (Exception ex)
            {
                return TaskResultDto.Fail(Exceptions.GetMessage(ex));
            }
        }

        public async Task<TaskResultDto> SendCaseClosedNotificationAsync(CustomerEmailDetailsViewModel emailDetails)
        {
            try
            {
                var icons = Path.Combine(_server.RootDirectory, "img", "icons");
                var illus = Path.Combine(_server.RootDirectory, "img", "illustrations");

                var htmlBody = await RenderEmailAsync(_closedtemplatePath, emailDetails);

                await CreateEmailAsync(
                  emailDetails.Email,
                  $"CCI Monitoring System - Case Closed",
                  htmlBody,
                  _dev,
                  null
                );

                return TaskResultDto.Success("Email Sent Successfully");
            }

            catch (Exception ex)
            {
                return TaskResultDto.Fail(Exceptions.GetMessage(ex));
            }
        }

        public async Task<TaskResultDto> SendQrCodeEmailAsync(CaseQrCodeEmailViewModel emailDetails, byte[] qrCode)
        {
            try
            {
                var contentId = Guid.NewGuid().ToString();
                emailDetails.QrCodeContentId = contentId;

                var tempFilePath = Path.GetTempFileName();
                await File.WriteAllBytesAsync(tempFilePath, qrCode);

                var htmlBody = await RenderEmailAsync(_qrCodeTemplatePath, emailDetails);

                var attachments = new List<EmailAttachment>
                {
                    new EmailAttachment
                    {
                        Path = tempFilePath,
                        ContentId = contentId
                    }
                };

                await CreateEmailAsync(
                    emailDetails.Email,
                    "Your Service Partner QR Code",
                    htmlBody,
                    _dev,
                    attachments
                );

                File.Delete(tempFilePath);

                return TaskResultDto.Success("Email Sent Successfully");
            }
            catch (Exception ex)
            {
                return TaskResultDto.Fail(Exceptions.GetMessage(ex));
            }
        }

        private async Task CreateEmailAsync(string to, string subject, string htmlBody, EmailCredential credential, IEnumerable<EmailAttachment>? attachments = null)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Customer Carry-In Monitoring System", credential.SenderEmailAddress));

            foreach (var recipient in _testEmails)
                message.Cc.Add(new MailboxAddress("", recipient));

            if (!to.IsNullOrEmpty())
                message.To.Add(new MailboxAddress("", to));

            message.ReplyTo.Add(new MailboxAddress("", credential.ReplyAddress));

            message.Subject = subject;

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = htmlBody
            };

            if (attachments != null)
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

        public async Task TestAsync()
        {
            string url = _configRepo.GetBaseUrl();
            _logger.LogInformation("Test Email Service");
            _logger.LogInformation("Base URL EmailService Call: " + url);
        }
    }
}
