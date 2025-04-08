using CCIMS.Web.App_Code._Globals.Constants;

namespace CCIMS.Web.Models.ViewModels.Partials
{
	public class SelectViewModel : BaseElementViewModel
	{
		public IEnumerable<DropdownOptionViewModel> Options { get; set; }
		public string? SelectContainerClassName { get; set; }

		public override string PlaceHolder => "Select Something";

		public override string Id => $"sel-{OperationName}-{IdName}";
		public override string Name => $"sel_{OperationName}_{IdName}";

		public override string GetPartialViewPath() => Routes.Partials.Inputs.SELECT;
	}
}
