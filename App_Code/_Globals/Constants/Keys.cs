namespace CCIMS.Web.App_Code._Globals.Constants
{
	public static class Keys
	{
		public sealed class ViewData
		{
			public const string TITLE = "PageTitle";
			public const string ERROR = "ErrorNotif";
			public const string SUCCESS = "SuccessNotif";
			public const string SIGN_OUT = "SignOutNotif";
			public const string ORIGIN = "OriginType";

			public static class Types
			{
				public const string WARRANTY = "AllWarrantyTypes";
				public const string STATUS = "AllStatusTypes";
			}

			public static class Search
			{
				public const string COUNT = "SearchCount";
				public const string VALUE = "SearchValue";
				public const string CATEGORY = "SearchCategory";
				public const string DATE_RANGE = "SearchDateRange";
				public const string CASE_ID = "SearchCaseID";
			}

		}
	}
}
