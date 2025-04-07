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
				public const string SELECT = @"../Shared/Inputs/_SelectPartial";
			}
			public static class SP
			{
				public const string TROW = @"../Shared/ServicePartnerPage/_SPTableRowPartial";
				public const string MODAL_CREATE = @"../Shared/ServicePartnerPage/_SPModalCreatePartial";
				public const string MODAL_EDIT = @"../Shared/ServicePartnerPage/_SPModalEditPartial";
				public const string MODAL_QR = @"../Shared/ServicePartnerPage/_QRModalViewPartial";
			}
			public static class Account
			{
				public const string TROW = @"../Shared/AccountPage/_TableRowPartial";
				public const string MODAL_CREATE = @"../Shared/AccountPage/_ModalCreatePartial";
				//public const string MODAL_EDIT = @"../Shared/Account/_SPModalEditPartial";
				//public const string MODAL_QR = @"../Shared/Account/_QRModalViewPartial";
			}
		}
	}
}
