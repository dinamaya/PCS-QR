using CCIMS.Web.App_Code._Globals.Constants;

namespace CCIMS.Web.Models.ViewModels.Partials
{
	public class CheckBoxViewModel : BaseElementViewModel
	{
		public override string GetPartialViewPath() => Routes.Partials.Inputs.CHECKBOX;

		public bool IsChecked { get; set; }
	}
}
