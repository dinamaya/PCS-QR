using CCIMS.Web.App_Code._Globals.Constants;

namespace CCIMS.Web.Models.ViewModels.Partials
{
  public class RadioGroupButtonViewModel : BaseElementViewModel
  {
		public RadioGroupButtonViewModel() {}

		public IEnumerable<BaseElementViewModel>? Elements { get; set; }
		public override string Id => $"rd-btn-{OperationName}-{IdName}";
    public override string Name => $"rd-btn_{OperationName}_{IdName}";

    public override string GetPartialViewPath() => throw new NotImplementedException();
	}
}
