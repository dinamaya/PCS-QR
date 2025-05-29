using CCIMS.Web.Repositories.Interfaces;

namespace CCIMS.Web.Models.ViewModels
{
  public class CaseAgedEmailDetailsViewModel
  {
    public string BaseUrl { get; set; }
    public IEnumerable<AgedCaseViewModel> Cases { get; set; }
  }
}
