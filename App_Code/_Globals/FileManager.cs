using CCIMS.Web.App_Code._Globals.Constants;
using System.IO;
using System.Runtime.InteropServices;

namespace CCIMS.Web.App_Code._Globals
{
	public class FileManager
	{
		private readonly Server _server;

		public FileManager(Server server)
		{
			_server = server;
		}

		public async Task<string> UploadAttachmentAsync(IFormFile file, [Optional] Action? uploadCallBack)
		{
			string directory = Path.Combine(_server.AttachmentsDirectory, "SN");

			if (!Directory.Exists(directory))
				Directory.CreateDirectory(directory);

			string fileName = GetUniqueFileName(file);
			string filePath = Path.Combine(directory, fileName);

			await UploadAsync(file, filePath, uploadCallBack);

			return filePath;
		}

		private async Task UploadAsync(IFormFile file, string filePath, Action? uploadCallBack)
		{
			if (file.Length > Database.File.MAX_SIZE)
				throw new ArgumentOutOfRangeException(nameof(file), Exceptions.Message.INVALID_FILESIZE);

			await using var original = file.OpenReadStream();
			await using var target = new FileStream(filePath, FileMode.Create);

			await original.CopyToAsync(target);
			uploadCallBack?.Invoke();
		}

		public string GetUniqueFileName(IFormFile file)
		{
			string ext = Path.GetExtension(file.FileName);
			string fn = DateTime.Now.ToLocalTime().ToString(Database.DateFormat.CONUMBER) + "-"+ Utils.Security.GenerateExtendedGuid("SN",2);
			return fn + ext;
		}
	}
}
