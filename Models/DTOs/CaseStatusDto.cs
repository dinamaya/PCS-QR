using CCIMS.Web.Models.ViewModels;

namespace CCIMS.Web.Models.DTOs
{
  public class CaseStatusDto
  {
    public string CurrentStatus { get; set; }
    public IEnumerable<DropdownOptionViewModel> AvailableStatus { get; set;}
  }
}
