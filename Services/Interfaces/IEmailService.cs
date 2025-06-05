using CCIMS.Web.Models.DTOs;
using CCIMS.Web.Models.ViewModels;
using System.Threading.Tasks;

namespace CCIMS.Web.Services.Interfaces
{
  public interface IEmailService
  {
    Task TestSendCaseCreationEmailAsync(string email, string caseNumber, string sp, string sn, string customerName);
    Task SendCustomerRegistrationNotificationAsync(CreateCustomerDto customerDto, string caseNumber);
    Task<TaskResultDto> SendAgedCasesEmailAsync(CaseAgedEmailDetailsViewModel agedCases);
    Task SendCaseClosedNotificationAsync(CustomerEmailDetailsViewModel emailDetails);
    Task TestAsync();
  }
}
