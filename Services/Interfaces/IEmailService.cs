using CCIMS.Web.Models.DTOs;
using CCIMS.Web.Models.ViewModels;

namespace CCIMS.Web.Services.Interfaces
{
  public interface IEmailService
	{
		Task TestSendCaseCreationEmailAsync(CustomerEmailDetailsViewModel emailDetails);
		Task TestSendCaseClosedEmailAsync(CustomerEmailDetailsViewModel emailDetails);
		Task SendCustomerRegistrationNotificationAsync(CreateCustomerDto customerDto, string caseNumber);
		Task SendCaseClosedNotificationAsync(CustomerEmailDetailsViewModel emailDetails);
	}
}
