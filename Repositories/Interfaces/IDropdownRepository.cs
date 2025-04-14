using CCIMS.Web.Models.ViewModels;

namespace CCIMS.Web.Repositories.Interfaces
{
  public interface IDropdownRepository
  {
    Task<IEnumerable<DropdownOptionViewModel>> GetOptions();
  }
}
