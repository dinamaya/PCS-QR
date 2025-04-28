using CCIMS.Web.App_Code._Globals.Constants;
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

    public async Task<string> ExtractText(string filePath)
    {
      return await Task.Run(() =>
      {
        using var engine = new TesseractEngine(_tessDataPath, "eng", EngineMode.TesseractAndLstm);
        engine.SetVariable("tessedit_char_whitelist", RegEx.Characters.SERIALNUMBER);
        engine.DefaultPageSegMode = PageSegMode.SingleLine;

        using var img = Pix.LoadFromFile(filePath);
        using var page = engine.Process(img);

        var ocrText = page.GetText();
        var match = Regex.Match(ocrText, RegEx.SERIALNUMBER);
        return match.Success ? match.Value : Exceptions.Message.UNRECOGNIZED_SERIALNUMBER;
      });
    }
  }
}
