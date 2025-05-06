using CCIMS.Web.App_Code._Globals.Constants;
using CCIMS.Web.App_Code._Globals.Enums;
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

    public static async Task<IHtmlContent> RenderAlertsAsync(this IHtmlHelper htmlHelper,
        string? error = null, 
        string? success = null, 
        string? warning = null, 
        string? info = null, 
        string? primary = null)
    {
      var alerts = new List<Task<IHtmlContent>>();

      if (!string.IsNullOrEmpty(error))
        alerts.Add(htmlHelper.PartialAsync(Routes.Partials.ALERT, new AlertViewModel(error, AlertType.Error)));

      if (!string.IsNullOrEmpty(success))
        alerts.Add(htmlHelper.PartialAsync(Routes.Partials.ALERT, new AlertViewModel(success, AlertType.Success)));

      if (!string.IsNullOrEmpty(warning))
        alerts.Add(htmlHelper.PartialAsync(Routes.Partials.ALERT, new AlertViewModel(warning, AlertType.Warning)));

      if (!string.IsNullOrEmpty(info))
        alerts.Add(htmlHelper.PartialAsync(Routes.Partials.ALERT, new AlertViewModel(info, AlertType.Info)));

      if (!string.IsNullOrEmpty(primary))
        alerts.Add(htmlHelper.PartialAsync(Routes.Partials.ALERT, new AlertViewModel(primary, AlertType.Primary)));

      var renderedAlerts = await Task.WhenAll(alerts);
      var contentBuilder = new HtmlContentBuilder();

      foreach (var alert in renderedAlerts)
      {
        contentBuilder.AppendHtml(alert);
      }

      return contentBuilder;
    }

    public static async Task<IHtmlContent> RenderOutlineAlertsAsync(this IHtmlHelper htmlHelper,
        string? error = null, 
        string? success = null, 
        string? warning = null, 
        string? info = null, 
        string? primary = null)
    {
      var alerts = new List<Task<IHtmlContent>>();

      if (!string.IsNullOrEmpty(error))
        alerts.Add(htmlHelper.PartialAsync(Routes.Partials.OUTLINE_ALERT, new AlertViewModel(error, AlertType.Error)));

      if (!string.IsNullOrEmpty(success))
        alerts.Add(htmlHelper.PartialAsync(Routes.Partials.OUTLINE_ALERT, new AlertViewModel(success, AlertType.Success)));

      if (!string.IsNullOrEmpty(warning))
        alerts.Add(htmlHelper.PartialAsync(Routes.Partials.OUTLINE_ALERT, new AlertViewModel(warning, AlertType.Warning)));

      if (!string.IsNullOrEmpty(info))
        alerts.Add(htmlHelper.PartialAsync(Routes.Partials.OUTLINE_ALERT, new AlertViewModel(info, AlertType.Info)));

      if (!string.IsNullOrEmpty(primary))
        alerts.Add(htmlHelper.PartialAsync(Routes.Partials.OUTLINE_ALERT, new AlertViewModel(primary, AlertType.Primary)));

      var renderedAlerts = await Task.WhenAll(alerts);
      var contentBuilder = new HtmlContentBuilder();

      foreach (var alert in renderedAlerts)
      {
        contentBuilder.AppendHtml(alert);
      }

      return contentBuilder;
    }
  }
}
