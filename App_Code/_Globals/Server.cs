using Microsoft.AspNetCore.Components;

namespace CCIMS.Web.App_Code._Globals
{
  public class Server(IWebHostEnvironment environment)
	{
		public string RootDirectory => environment.WebRootPath;
    public string AttachmentsDirectory => Path.Combine(environment.ContentRootPath, @"Attachments\");
  }
}