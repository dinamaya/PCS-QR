
using CCIMS.Web.App_Code._Globals.Constants;
using CCIMS.Web.App_Code._Globals.Enums;
using CCIMS.Web.Models.Interfaces;

namespace CCIMS.Web.Models.ViewModels.Partials
{
	public class InputFieldViewModel : BaseElementViewModel, IInputElement
	{
		public bool IsCompact { get; set; } = false;
		public bool IsDisabled { get; set; } = false;

		public string? Value { get; set; }
    public InputType? Type { get; set; } = InputType.Text;
    public string? InputContainerClassName { get; set; }
    public string? InputClassName { get; set; }

    public override string GetPartialViewPath() => Routes.Partials.Inputs.TEXTBOX;
	}
}
