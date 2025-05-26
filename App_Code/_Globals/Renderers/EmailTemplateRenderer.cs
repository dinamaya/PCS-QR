using RazorLight;

namespace CCIMS.Web.App_Code._Globals.Renderers
{
  public class EmailTemplateRenderer
  {
    private readonly RazorLightEngine _engine;

    public EmailTemplateRenderer()
    {
      string path = Path.Combine(Directory.GetCurrentDirectory(), "Views");
      _engine = new RazorLightEngineBuilder()
        .UseFileSystemProject(path)
        .UseMemoryCachingProvider()
        .Build();
    }

    public async Task<string> RenderTemplateAsync<T>(string templatePath, T model) => await _engine.CompileRenderAsync(templatePath, model);
  }
}
