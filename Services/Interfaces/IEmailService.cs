using CCIMS.Web.Models.ViewModels;

namespace CCIMS.Web.Services.Interfaces
{
  public interface IEmailService
  {
    Task TestSendCaseCreationEmailAsync(CustomerEmailDetailsViewModel emailDetails);
  }
}
