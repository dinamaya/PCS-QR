using CCIMS.Web.Models.ViewModels;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CCIMS.Web.App_Code._Globals.Extensions
{
	public static class HtmlHelperExtensions
	{
		public static async Task<IHtmlContent> RenderAsync(this IHtmlHelper htmlHelper, BaseElementViewModel element)
		{
			string path = element.GetPartialViewPath();
			return await htmlHelper.PartialAsync(path, element);
		}
	}
}
