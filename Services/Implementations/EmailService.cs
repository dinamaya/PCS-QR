using CCIMS.Web.App_Code._Globals;
using CCIMS.Web.App_Code._Globals.Constants;
using CCIMS.Web.Models.Complex;
using CCIMS.Web.Models.DTOs;
using CCIMS.Web.Models.ViewModels;
using CCIMS.Web.Repositories.Interfaces;
using CCIMS.Web.Services.Interfaces;
using Elfie.Serialization;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using MimeKit;
using NuGet.Protocol.Plugins;
using Scriban;
using Scriban.Syntax;

namespace CCIMS.Web.Services.Implementations
{
	public class EmailService : IEmailService
	{
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


		public async Task TestSendCaseCreationEmailAsync(CustomerEmailDetailsViewModel emailDetails)
		{
			var icons = Path.Combine(_server.RootDirectory, "img", "icons");
			var illus = Path.Combine(_server.RootDirectory, "img", "illustrations");
			var resources = new List<EmailAttachment>()
		{
		  new(){
			Path = Path.Combine(icons, "icon_ccims_lg.svg"),
			ContentId = "web_icon"
		  },
		  new(){
			Path = Path.Combine(icons, "VST-ECS.png"),
			ContentId = "vst_icon"
		  },
		  new(){
			Path = Path.Combine(icons, "circle-check-solid.png"),
			ContentId = "check_icon.png"
		  },
		  new(){
			Path = Path.Combine(illus,"thank-you1.png"),
			ContentId = "character_image"
		  },
		};

			var emailAssets = new CustomerEmailAssetsViewModel(_configRepo, emailDetails);

			string templatePath = Path.Combine("App_Code", "Scriban", "Templates", "CaseCreationCustomerEmail.sbn");
			var htmlBody = await RenderEmailAsync(templatePath, emailAssets);

			await CreateEmailAsync(
			  emailDetails.Email,
			  $"Test CCIMS - Case Filed by {emailDetails.Fullname}",
			  htmlBody,
			  _dev,
			  resources
			);
		}

		public async Task TestSendCaseClosedEmailAsync(CustomerEmailDetailsViewModel emailDetails)
		{
			var icons = Path.Combine(_server.RootDirectory, "img", "icons");
			var illus = Path.Combine(_server.RootDirectory, "img", "illustrations");
			var resources = new List<EmailAttachment>()
		{
		  new(){
			Path = Path.Combine(icons, "icon_ccims_lg.svg"),
			ContentId = "web_icon"
		  },
		  new(){
			Path = Path.Combine(icons, "VST-ECS.png"),
			ContentId = "vst_icon"
		  },
		  new(){
			Path = Path.Combine(icons, "circle-check-solid.png"),
			ContentId = "check_icon.png"
		  },
		  new(){
			Path = Path.Combine(illus,"thank-you1.png"),
			ContentId = "character_image"
		  },
		};

			var emailAssets = new CustomerEmailAssetsViewModel(_configRepo, emailDetails);

			string templatePath = Path.Combine("App_Code", "Scriban", "Templates", "CaseClosedCustomerEmail.sbn");
			var htmlBody = await RenderEmailAsync(templatePath, emailAssets);

			await CreateEmailAsync(
			  emailDetails.Email,
			  $"Test CCIMS - Case Filed by {emailDetails.Fullname}",
			  htmlBody,
			  _dev,
			  resources
			);
		}

		public async Task SendCustomerRegistrationNotificationAsync(CreateCustomerDto customerDto, string caseNumber)
		{
			try
			{
				var emailDetails = new CustomerEmailDetailsViewModel
				{
					Email = customerDto.Email,
					Fullname = $"{customerDto.FirstName} {customerDto.LastName}",
					CaseNumber = caseNumber,
					ServicePartner = customerDto.ServicePartner
				};

				await TestSendCaseCreationEmailAsync(emailDetails).ConfigureAwait(false);
				_logger.LogInformation($"Email notification sent successfully for case: {caseNumber}");
			}
			catch (Exception ex)
			{
				_logger.LogError($"Failed to send email notification for case {caseNumber}: {ex.Message}");

			}
		}

		public async Task SendCaseClosedNotificationAsync(CustomerEmailDetailsViewModel emailDetails)
		{
			try
			{

				await TestSendCaseClosedEmailAsync(emailDetails).ConfigureAwait(false);
			}

			catch (Exception ex)
			{
				_logger.LogError($"Failed to send case closed email notification for case {emailDetails.CaseNumber}: {ex.Message}");
				// Optionally rethrow or handle as per application's error handling strategy
			}
		}

		private async Task CreateEmailAsync(string to, string subject, string htmlBody, EmailCredential credential, IEnumerable<EmailAttachment> attachments)
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

			foreach (var a in attachments)
			{
				if (!File.Exists(a.Path)) continue;

				var resource = bodyBuilder.LinkedResources.Add(a.Path);
				resource.ContentId = a.ContentId;
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
