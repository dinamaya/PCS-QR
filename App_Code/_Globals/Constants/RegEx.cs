using CCIMS.Web.Repositories.Implementations;
using CCIMS.Web.Repositories.Interfaces;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace CCIMS.Web.App_Code._Globals.Constants
{
  public sealed class RegEx
  {

    [StringSyntax(StringSyntaxAttribute.Regex)] public const string SERIALNUMBER = @"[A-Z0-9\-]{6,}";
    [StringSyntax(StringSyntaxAttribute.Regex)] public const string NAMES = @"^[\p{L}\s_.,ñÑ]+$";
    [StringSyntax(StringSyntaxAttribute.Regex)] public const string USERNAME = @"^[\p{L}0-9_.]+$";
    [StringSyntax(StringSyntaxAttribute.Regex)] public const string PHONE = @"^\d+$";
    [StringSyntax(StringSyntaxAttribute.Regex)] public const string EMAIL_LOCAL = @"^[a-zA-Z0-9._%+-]+(?=@)";
    [StringSyntax(StringSyntaxAttribute.Regex)] public const string EMAIL_STRICT = @"^[\w\.\-]+@[\w\-]+(\.[\w\-]+)*\.[a-zA-Z]{2,}$";
    [StringSyntax(StringSyntaxAttribute.Regex)] public const string COMMON = @"^[\p{L}\p{N}\.\,""'\-_/]+$";
    [StringSyntax(StringSyntaxAttribute.Regex)] public const string CONTACT_NUMBER = @"^\+?[\d\s\-()]{7,20}$";
    [StringSyntax(StringSyntaxAttribute.Regex)] public const string NON_EMPTY = @"(.|\s)*\S(.|\s)*";

    public static class Characters
    {
      public const string SERIALNUMBER = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789-._";
    }
  }
}
