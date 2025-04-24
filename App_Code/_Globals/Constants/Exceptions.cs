using Microsoft.Data.SqlClient;
using System.Reflection;

namespace CCIMS.Web.App_Code._Globals.Constants
{
	public sealed class Exceptions
	{
		public sealed class Message
		{
			public const string INVALID_AUTHENTICATION_CREDENTIALS = "Authentication Failed: Username or password is incorrect";
			public const string INVALID_QRTOKEN = "Invalid Token: Token doesn't exist or is already expired";
			public const string INVALID_QRREFERENCE = "Invalid QR Code: The scanned QR code is not recognized by the system. Please ensure you are using a valid QR code provided by VST ECS.";
			public const string INVALID_SPREFERENCE = "Invalid SP Reference: SP does not exist. Please contact the administrator";
			public const string INVALID_ACCOUNTREFERENCE = "Invalid Account Reference: Account does not exist.";
			public const string INVALID_ACCOUNT_UPDATE = "Account Update Failed. Please Check the logs or contact the Administrator";
			public const string INVALID_STATUS = "Status Not Found. Please Check the logs or contact the Administrator";
			public const string INVALID_CATEGORY= "Category Not Found. Please provide the correct category id or contact the Administrator";
			public const string INVALID_CUSTOMER= "Customer Not Found. Please provide the correct customer id or contact the Administrator";
			public const string INVALID_CASE= "Case Not Found. Please provide the correct case id or contact the Administrator";
			public const string INVALID_FILESIZE = $"File size should not exceed 10 MB.";

			public const string UNRECOGNIZED_SERIALNUMBER = $"Serial number not recognize";


      public static class Config
			{
        public const string INVALID_SYS_SECDETAILS = "Invalid Security Details Please contact the administrator";
				public const string INVALID_APIURL_PSGC = "Incorrect PSGC base URL";
      }
    }

		public static string GetMessage(Exception ex) => ex.Message + (ex.InnerException != null ? "" + ex.InnerException.Message : "");
	}
}
