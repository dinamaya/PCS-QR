using CCIMS.Web.App_Code._Globals.Constants;

namespace CCIMS.Web.Models.ViewModels.Partials
{
  public class RadioGroupButtonViewModel : BaseElementViewModel
  {
    public RadioGroupButtonViewModel(BaseElementViewModel element)
    {
      IdName = element.IdName;
      Title = element.Title;
      OperationName = element.OperationName;
      Element = element;
		}
		public BaseElementViewModel Element { get; set; }
		public override string Id => $"rd-btn-{OperationName}-{IdName}";
    public override string Name => $"rd-btn_{OperationName}_{IdName}";

    public override string GetPartialViewPath() => Element.GetPartialViewPath();

	}
}
