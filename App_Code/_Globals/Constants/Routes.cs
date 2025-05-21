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
			public const string FOOTER = @"../Shared/_FooterPartial";
			public const string TIMELINE_TRANSACTIONS = @"../Shared/Timeline/_TransactionsPartial";

			public static class Inputs
			{
				public const string TEXTBOX = @"../Shared/Inputs/_InputFieldPartial";
				public const string CHECKBOX = @"../Shared/Inputs/_CheckBoxPartial";
				public const string SELECT = @"../Shared/Inputs/_SelectPartial";
				public const string RADIO_GROUP = @"../Shared/Inputs/_RadioGroupPartial";
			}

			public static class Customer
			{
				public const string MODAL_SCAN = @"../Shared/RegistrationPage/_ImageModalEditPartial";
				public const string MODAL_POLICY = @"../Shared/RegistrationPage/_PolicyModalPartial";
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
				public const string MODAL_EDIT = @"../Shared/AccountPage/_ModalEditPartial";
			}
			
			public static class Cases
			{
				public const string TROW = @"../Shared/CasePage/_TableRowPartial";
				public const string TRANS_HIST = @"../Shared/CasePage/_TransactionsHistoryPartial";
				public const string CASE_DETAILS = @"../Shared/CasePage/_CaseDetailsPartial";
				public const string SEARCH = @"../Shared/CasePage/_SearchBarPartial";
				public const string STAT_UPDATE_MODAL = @"../Shared/CasePage/_ModalStatusUpdatePartial";
				public const string CUSTOMER_UPDATE_MODAL = @"../Shared/CasePage/_CustomerModalEditPartial";
				public const string CASE_UPDATE_MODAL = @"../Shared/CasePage/_CaseModalEditPartial";
				public const string TRANSACTION_UPDATE_MODAL = @"../Shared/CasePage/_TransactionRemarksModalEditPartial";
			}

			public static class Operation
			{
				public const string STAT_TROW = @"../Shared/OperationPage/_StatusTableRowPartial";
				public const string STAT_CREATE_MODAL = @"../Shared/OperationPage/_StatusModalCreatePartial";
				public const string STAT_EDIT_MODAL = @"../Shared/OperationPage/_StatusModalEditPartial";
			}
		}
	}
}
