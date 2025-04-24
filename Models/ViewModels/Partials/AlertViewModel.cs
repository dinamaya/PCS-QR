using CCIMS.Web.App_Code._Globals.Enums;

namespace CCIMS.Web.Models.ViewModels.Partials
{
	public class AlertViewModel(string message, AlertType type = AlertType.Error)
	{
		public string Message { get; } = message;
		public AlertType Type { get; } = type;
	}
}
