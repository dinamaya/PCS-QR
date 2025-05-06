namespace CCIMS.Web.Models.ViewModels.Partials
{
	public class NavBarViewModel
	{
		public readonly bool IsAuthenticated;
		public readonly string Username;
		public readonly string RoleName;
		public readonly string AvatarInitials;
		public readonly string FullName;

		public NavBarViewModel(bool isAuthenticated, string username, string roleName, string firstName, string lastName)
		{
			IsAuthenticated = isAuthenticated;
			Username = IsAuthenticated ? username : "";
			RoleName = IsAuthenticated ? roleName : "";
			AvatarInitials = IsAuthenticated ? firstName.ElementAt(0).ToString().ToUpper() + lastName.ElementAt(0).ToString().ToUpper() : "";
			FullName = IsAuthenticated ? lastName + ", " + firstName : "";
		}
	}
}
