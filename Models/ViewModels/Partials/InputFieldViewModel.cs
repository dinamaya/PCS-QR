
using CCIMS.Web.App_Code._Globals.Enums;

namespace CCIMS.Web.Models.ViewModels.Partials
{
	public class InputFieldViewModel
  {
    /// <summary>
    /// CRUD Name; Add/Create, Fetch/Read, Edit/Update, Remove/Delete
    /// </summary>
    public string OperationName { get; set; }
    public string IdName { get; set; }
    public string Title { get; set; }
    public string? Value { get; set; }
    public string? ClassName { get; set; }
    public InputType? Type { get; set; } = InputType.Text;

		public string Id => $"txt-{OperationName}-{IdName}";
		public string Name => $"txt_{OperationName}_{IdName}";
		public string NotifId => $"notif-{IdName}";
		public string PlaceHolder => $"Enter {Title}";
	}
}
