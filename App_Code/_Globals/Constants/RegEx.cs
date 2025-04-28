using System.Diagnostics.CodeAnalysis;

namespace CCIMS.Web.App_Code._Globals.Constants
{
  public sealed class RegEx
  {
    [StringSyntax(StringSyntaxAttribute.Regex)]
    public const  string SERIALNUMBER = @"[A-Z0-9\-]{6,}";

    public static class Characters
    {
      public const string SERIALNUMBER = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789-._";
    }
  }
}
