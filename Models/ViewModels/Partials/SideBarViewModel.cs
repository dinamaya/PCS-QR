namespace CCIMS.Web.Models.ViewModels.Partials
{
	public class SideBarViewModel
	{
		public readonly bool isAuthenticated;

		public SideBarViewModel(bool isAuthenticated)
		{
			this.isAuthenticated = isAuthenticated;
		}
	}
}
