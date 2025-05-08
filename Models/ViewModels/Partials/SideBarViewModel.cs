namespace CCIMS.Web.Models.ViewModels.Partials
{
	public class SideBarViewModel
	{
		public readonly bool isAuthenticated;
		public readonly string controller;
		public readonly string action;

    public SideBarViewModel(bool isAuthenticated, string controller, string action)
    {
      this.isAuthenticated = isAuthenticated;
      this.controller = controller;
      this.action = action;
    }
  }
}
