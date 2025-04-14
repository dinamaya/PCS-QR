using CCIMS.Web.App_Code._Globals.Constants;

namespace CCIMS.Web.Models.ViewModels.Partials
{
	public class RadioGroupViewModel : BaseElementViewModel
	{
		public string JavaScript { get; set; }
		public IEnumerable<RadioGroupButtonViewModel> RadioGroupButtons { get; set; }
		public override string Id => $"rd-grp-{OperationName}-{IdName}";
		public override string Name => $"rd-grp_{OperationName}_{IdName}";

		public override string GetPartialViewPath() => Routes.Partials.Inputs.RADIO_GROUP;
	}
}
