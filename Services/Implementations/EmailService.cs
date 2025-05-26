using CCIMS.Web.App_Code._Globals;
using CCIMS.Web.App_Code._Globals.Constants;
using CCIMS.Web.App_Code._Globals.Renderers;
using CCIMS.Web.Models.Complex;
using CCIMS.Web.Models.DTOs;
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
		private readonly Server _server;
		private readonly EmailCredential _dev;
		private readonly EmailCredential _prod;
		private readonly IEnumerable<string> _testEmails;
		private readonly ILogger<EmailService> _logger;

		public EmailService(IOptions<EmailServiceConfig> config, Server server, ILogger<EmailService> logger)
		{
			_dev = config.Value.Credentials["Dev"];
			_prod = config.Value.Credentials["Prod"];
			_testEmails = config.Value.TestEmails;
			_server = server;
			_logger = logger;
		}

		public async Task TestSendCaseCreationEmailAsync(CustomerEmailDetailsViewModel emailDetails)
		{
			var emailAssets = new CustomerEmailAssetsViewModel(_server, emailDetails);
			var htmlBody = await new EmailTemplateRenderer().RenderTemplateAsync(Routes.Partials.EMAIL.CASE_CREATION, emailAssets);
			await CreateEmailAsync(
			  emailDetails.Email,
			  $"Test CCIMS - Case Filed by {emailDetails.Fullname}",
			  htmlBody,
			  _dev);
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
