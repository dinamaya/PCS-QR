using CCIMS.Web.App_Code._Globals.Constants;

namespace CCIMS.Web.Models.ViewModels
{
  public class DropdownOptionViewModel
  {
    public string Value { get; set; }
    public string Label { get; set; }

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
