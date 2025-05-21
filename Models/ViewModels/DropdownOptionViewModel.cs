using CCIMS.Web.App_Code._Globals.Constants;
using CCIMS.Web.Models.DTOs;

namespace CCIMS.Web.Models.ViewModels
{
  public class DropdownOptionViewModel : DropdownOptionDto
  {
    public static DropdownOptionViewModel Default(string Label) => new()
    {
      Value = "",
      Label = Label
		};

    public static List<DropdownOptionViewModel> InitOptions(string defaultLabel, IEnumerable<DropdownOptionViewModel> options) {

      List<DropdownOptionViewModel> opts = [Default(defaultLabel)];
      opts.AddRange(options);

      return opts;
    }
  }
}
