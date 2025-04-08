
using CCIMS.Web.App_Code._Globals.Constants;
using CCIMS.Web.App_Code._Globals.Enums;

namespace CCIMS.Web.Models.ViewModels.Partials
{
	public class InputFieldViewModel : BaseElementViewModel
	{
		public string? Value { get; set; }
    public InputType? Type { get; set; } = InputType.Text;
		public override string GetPartialViewPath() => Routes.Partials.Inputs.TEXTBOX;
	}
}
