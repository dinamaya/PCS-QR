namespace CCIMS.Web.Models.ViewModels.Partials
{
	public class SelectViewModel
	{
		public string IdName { get; set; }
		public string Title { get; set; }
		public IEnumerable<DropdownOptionViewModel> Options { get; set; }
		public string? ClassName { get; set; }
		public string? SelectContainerClassName { get; set; }

		public string DefaultName { get; set; } = "Select Something";

		public string Id => $"sel-{IdName}";
		public string Name => $"sel_{IdName}";
		public string NotifId => $"notif-{IdName}";
	}
}
