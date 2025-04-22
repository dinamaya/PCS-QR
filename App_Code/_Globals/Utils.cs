using System;
using Humanizer;

namespace CCIMS.Web.App_Code._Globals
{
	public static class Utils
	{
		public static class Urls
		{
			public static string Combine(string baseUrl, string relativePath)
			{
				var baseUri = new Uri(baseUrl, UriKind.Absolute);
				var combinedUri = new Uri(baseUri, relativePath);
				return combinedUri.ToString();
			}

		}

		public static class Security
		{
      public static string GenerateExtendedGuid(string prefix, int count = 4)
      {

				if (count > 6)
					throw new Exception("Failed to generate ID. Maximum of 6");

        var result = prefix;
        
				for (int i = 0; i < count; i++)
          result += Guid.NewGuid().ToString("N");

        return result;
      }

			public static byte[] ConvertHexStringToBytes(string hex)
			{
				if (hex.Length % 2 != 0)
					throw new ArgumentException("Invalid hex string length.");

				byte[] bytes = new byte[hex.Length / 2];
				for (int i = 0; i < hex.Length; i += 2)
					bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
				
				return bytes;
			}
		}

    public static string GetTimestamp(string message, DateTime dateTime)
    {
      var now = DateTime.Now;
      var timeDiff = now - dateTime;

      string humanizedTime = timeDiff.Humanize(precision: 1, maxUnit: Humanizer.Localisation.TimeUnit.Day);

      if (timeDiff.TotalDays < 1)
      {
        if (timeDiff.TotalHours < 1)
          return $"{message} {humanizedTime} ({dateTime:h:mmtt})";
        else
          return $"{message} today at {dateTime:h:mmtt}";
      }
      else if (timeDiff.TotalDays < 365)
        return $"{message} {humanizedTime} ago ({dateTime:MMM d, yyyy, h:mmtt})";
      else
      {
        int years = (int)(timeDiff.TotalDays / 365);
        return $"{message} {years} year/s ago ({dateTime:MMM d, yyyy, h:mmtt})";
      }
    }
  }
}
