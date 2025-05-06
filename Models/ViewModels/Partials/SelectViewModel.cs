using CCIMS.Web.App_Code._Globals.Constants;
using CCIMS.Web.Models.Interfaces;

namespace CCIMS.Web.Models.ViewModels.Partials
{
	public class SelectViewModel : BaseElementViewModel, IInputElement
	{
		public IEnumerable<DropdownOptionViewModel> Options { get; set; }

		public override string PlaceHolder => $"Select {base.Title}" ;

		public override string Id => $"sel-{OperationName}-{IdName}";
		public override string Name => $"sel_{OperationName}_{IdName}";

    public string? InputContainerClassName { get; set; }
    public string? InputClassName { get; set; }

    public override string GetPartialViewPath() => Routes.Partials.Inputs.SELECT;
	}
}
