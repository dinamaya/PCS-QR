using CCIMS.Web.Models.ViewModels;
using CCIMS.Web.Models.ViewModels.Partials;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;

namespace CCIMS.Web.App_Code._Globals.Extensions
{
	public static class HtmlHelperExtensions
	{
		public static async Task<IHtmlContent?> RenderAsync(this IHtmlHelper htmlHelper, BaseElementViewModel element)
		{
			if(element is null)
				return null;

			if(element is RadioGroupButtonViewModel)
			{
				var _element = element as RadioGroupButtonViewModel;
				
				if (_element.Elements == null && _element.Elements.Any()) return null;

				foreach (var el in _element.Elements)
				{
					string _path = el.GetPartialViewPath();
					return await htmlHelper.PartialAsync(_path, element);
				}
			}

			string path = element.GetPartialViewPath();
			return await htmlHelper.PartialAsync(path, element);
		}
	}
}
