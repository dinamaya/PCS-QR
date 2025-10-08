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
        private readonly string _feedbackTemplatePath = Path.Combine("App_Code", "Scriban", "Templates", "CaseFeedbackCustomerEmail.sbn");
        private readonly string _qrCodeTemplatePath = Path.Combine("App_Code", "Scriban", "Templates", "CaseQrCodeEmail.sbn");

        private readonly EmailCredential _dev;
        private readonly EmailCredential _prod;
        private readonly IEnumerable<string> _testEmails;
        private readonly IEnumerable<string> _bcc;
        private readonly ILogger<EmailService> _logger;
        private readonly IConfigurationRepository _configRepo;
        private readonly Server _server;


        public EmailService(IOptions<EmailServiceConfig> config, ILogger<EmailService> logger, IConfigurationRepository configRepo, Server server)
        {
            _dev = config.Value.Credentials["Dev"];
            _prod = config.Value.Credentials["Prod"];
            _testEmails = config.Value.TestEmails;
            _bcc = config.Value.Bcc;

            _logger = logger;
            _server = server;
            _configRepo = configRepo;
        }

        public async Task<TaskResultDto> SendCustomerRegistrationNotificationAsync(CreateCustomerDto customerDto, string caseNumber, string encryptedCaseNumber, string spEmail)
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
                    SerialNumber = customerDto.SerialNumber,
                };

                var htmlBody = await RenderEmailAsync(_custTemplatePath, custModel);

                await CreateEmailAsync(
                  customerDto.Email,
                            $"CCI Monitoring System - Case Registered ({custModel.CaseNumber}) | {custModel.ServicePartner} | {custModel.SerialNumber}",
                  htmlBody,
                  _dev,
                  null,
                  cc: [spEmail]
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
                  $"CCI Monitoring System - Aged Cases ({DateTime.Now.ToLocalTime().ToString(Database.DateFormat.DISPLAY_COMPLETE)})",
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

        public async Task<TaskResultDto> SendCaseClosedNotificationAsync(CustomerEmailDetailsViewModel emailDetails, string spEmail)
        {
            try
            {
                var icons = Path.Combine(_server.RootDirectory, "img", "icons");
                var illus = Path.Combine(_server.RootDirectory, "img", "illustrations");

                var htmlBody = await RenderEmailAsync(_closedtemplatePath, emailDetails);

                await CreateEmailAsync(
                  emailDetails.Email,
                  $"CCI Monitoring System - Case Closed ({emailDetails.CaseNumber}) | {emailDetails.ServicePartner} | {emailDetails.SerialNumber}",
                  htmlBody,
                  _dev,
                  null,
                  cc: [spEmail]
                );

                return TaskResultDto.Success("Email Sent Successfully");
            }

            catch (Exception ex)
            {
                return TaskResultDto.Fail(Exceptions.GetMessage(ex));
            }
        }

        public async Task<TaskResultDto> SendCaseFeedbackNotificationAsync(CustomerEmailDetailsViewModel emailDetails, string spEmail)
        {
            try
            {
                var icons = Path.Combine(_server.RootDirectory, "img", "icons");
                var illus = Path.Combine(_server.RootDirectory, "img", "illustrations");

                var htmlBody = await RenderEmailAsync(_feedbackTemplatePath, emailDetails);

                await CreateEmailAsync(
                  emailDetails.Email,
                  $"CCI Monitoring System - We Value Your Feedback! ({emailDetails.CaseNumber}) | {emailDetails.ServicePartner} | {emailDetails.SerialNumber}",
                  htmlBody,
                  _dev,
                  null,
                  cc: [spEmail]
                );

                return TaskResultDto.Success("Feedback Email Sent Successfully");
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
                var tempFilePath = Path.GetTempFileName();
                await File.WriteAllBytesAsync(tempFilePath, qrCode);

                var htmlBody = await RenderEmailAsync(_qrCodeTemplatePath, emailDetails);

                var attachments = new List<EmailAttachment>
                {
                    new EmailAttachment
                    {
                        Name = "QRCode.png",
                        Path = tempFilePath
                    }
                };

                await CreateEmailAsync(
                    emailDetails.Email,
                    $"CCI Monitoring System - QR Code | {emailDetails.Spname}",
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

        public async Task<TaskResultDto> SendSPCreateNotificationAsync(CaseQrCodeEmailViewModel emailDetails, byte[] qrCode)
        {
            try
            {
                var tempFilePath = Path.GetTempFileName();
                await File.WriteAllBytesAsync(tempFilePath, qrCode);

                var htmlBody = await RenderEmailAsync(_qrCodeTemplatePath, emailDetails);

                var attachments = new List<EmailAttachment>
                {
                    new EmailAttachment
                    {
                        Name = "QRCode.png",
                        Path = tempFilePath
                    }
                };

                await CreateEmailAsync(
                    emailDetails.Email,
                    $"CCI Monitoring System - {emailDetails.Spname} Added as a Service Partner ",
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

        private async Task CreateEmailAsync(string to, string subject, string htmlBody, EmailCredential credential, IEnumerable<EmailAttachment>? attachments = null, IEnumerable<string>? cc = null)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Customer Carry-In Monitoring System", credential.SenderEmailAddress));

            foreach (var recipient in _testEmails)
                message.Cc.Add(new MailboxAddress("", recipient));

            foreach (var recipient in _bcc)
                message.Bcc.Add(new MailboxAddress("", recipient));
            
            if (cc != null)
            {
                foreach (var recipient in cc)
                    message.Cc.Add(new MailboxAddress("", recipient));
            }

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

                    var attachment = new MimePart("image", "png")
                    {
                        Content = new MimeContent(File.OpenRead(a.Path), ContentEncoding.Default),
                        ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                        ContentTransferEncoding = ContentEncoding.Base64,
                        FileName = a.Name
                    };
                    bodyBuilder.Attachments.Add(attachment);
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
