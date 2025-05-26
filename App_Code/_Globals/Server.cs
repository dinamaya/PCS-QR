using Microsoft.AspNetCore.Components;

namespace CCIMS.Web.App_Code._Globals
{
  public class Server(IWebHostEnvironment environment, IHttpContextAccessor httpContextAccessor)
	{
    public Uri? BaseUrl {
      get {
        var request = httpContextAccessor.HttpContext?.Request;
        return request == null ? null : new Uri($"{request.Scheme}://{request.Host}");
      }
    }

		public string RootDirectory => environment.WebRootPath;
    public string AttachmentsDirectory => Path.Combine(environment.ContentRootPath, @"Attachments\");
  }
}