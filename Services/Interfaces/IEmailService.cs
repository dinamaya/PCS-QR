using CCIMS.Web.Models.DTOs;
using CCIMS.Web.Models.ViewModels;
using System.Threading.Tasks;

namespace CCIMS.Web.Services.Interfaces
{
  public interface IEmailService
  {
    Task<TaskResultDto> SendCustomerRegistrationNotificationAsync(CreateCustomerDto customerDto, string caseNumber);
    Task<TaskResultDto> SendAgedCasesEmailAsync(CaseAgedEmailDetailsViewModel agedCases);
    Task<TaskResultDto> SendCaseClosedNotificationAsync(CustomerEmailDetailsViewModel emailDetails);
    Task TestAsync();
  }
}
