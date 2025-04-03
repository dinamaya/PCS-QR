namespace CCIMS.Web.App_Code._Globals.Constants
{
	public abstract class Routes
	{
		public sealed class Partials
		{
			public const string ALERT = @"../Shared/Alerts/_AlertPartial";
			public const string OUTLINE_ALERT = @"../Shared/Alerts/_OutlineAlertPartial";
			public const string SIDEBAR = @"../Shared/_SideBarPartial";
			public const string NAVBAR = @"../Shared/_NavBarPartial";

			public static class Inputs
			{
				public const string TEXTBOX = @"../Shared/Inputs/_InputFieldPartial";
			}
			public static class SP
			{
				public const string TROW = @"../Shared/Tables/_SPTableRowPartial";
				public const string MODAL_CREATE = @"../Shared/Modals/_SPModalCreatePartial";
			}
		}
	}
}
