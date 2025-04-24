using CCIMS.Web.Repositories.Interfaces;
using System.Text.RegularExpressions;
using Tesseract;

namespace CCIMS.Web.Repositories.Implementations
{
  public class CameraRepository : ICameraRepository
  {
    private readonly string _tessDataPath;
    private readonly IConfigurationRepository _configRepo;

    public CameraRepository(IWebHostEnvironment env, IConfigurationRepository configRepo)
    {
      _configRepo = configRepo;
      _tessDataPath = Path.Combine(env.ContentRootPath, _configRepo.GetTesseractTrainingDataPath());
    }

    public async Task<string> GetResult(string filePath)
    {
      return await Task.Run(() =>
      {
        using var engine = new TesseractEngine(_tessDataPath, "eng", EngineMode.Default);
        engine.SetVariable("tessedit_char_whitelist", "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789-._=/#");

        using var img = Pix.LoadFromFile(filePath);
        using var page = engine.Process(img);

        var ocrText = page.GetText();
        var match = Regex.Match(ocrText, @"[A-Z0-9]{6,}");
        return match.Success ? match.Value : "Serial number not found";
      });
    }
  }
}
