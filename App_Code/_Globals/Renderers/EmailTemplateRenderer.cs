using RazorLight;

namespace CCIMS.Web.App_Code._Globals.Renderers
{
  public class EmailTemplateRenderer
  {
    private readonly RazorLightEngine _engine;

    public EmailTemplateRenderer()
    {
      _engine = new RazorLightEngineBuilder()
        .UseFileSystemProject(Path.Combine(Directory.GetCurrentDirectory(), "Views"))
        .UseMemoryCachingProvider()
        .Build();
    }

    public async Task<string> RenderTemplateAsync<T>(string templatePath, T model) => await _engine.CompileRenderAsync(templatePath, model);
  }
}
