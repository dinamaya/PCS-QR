using CCIMS.Web.Models.DTOs;
using CCIMS.Web.Models.ViewModels;
using System.Threading.Tasks;

namespace CCIMS.Web.Services.Interfaces
{
    public interface IEmailService
    {
        Task<TaskResultDto> SendCustomerRegistrationNotificationAsync(CreateCustomerDto customerDto, string caseNumber, string encryptedCaseNumber, string spEmail);
        Task<TaskResultDto> SendAgedCasesEmailAsync(CaseAgedEmailDetailsViewModel agedCases);
        Task<TaskResultDto> SendCaseClosedNotificationAsync(CustomerEmailDetailsViewModel emailDetails, string spEmail);
        Task<TaskResultDto> SendCaseFeedbackNotificationAsync(CustomerEmailDetailsViewModel emailDetails, string spEmail);
        Task<TaskResultDto> SendCaseFeedbackReminderNotificationAsync(CustomerEmailDetailsViewModel emailDetails, string spEmail);
        Task<TaskResultDto> SendCaseStatusUpdateEmailNotificationAsync(CaseStatusUpdateEmailViewModel emailDetails, string spEmail);
        Task<TaskResultDto> SendQrCodeEmailAsync(CaseQrCodeEmailViewModel emailDetails, byte[] qrCode);
        Task<TaskResultDto> SendSPCreateNotificationAsync(CaseQrCodeEmailViewModel emailDetails, byte[] qrCode);
        Task TestAsync();
    }
}
