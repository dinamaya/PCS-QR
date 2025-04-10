using CCIMS.Web.Models.ViewModels;
using CCIMS.Web.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CCIMS.Web.Controllers
{
	public class OperationController : Controller
	{
		private readonly IOperationsRepository _opsRepo;

    public OperationController(IOperationsRepository opsRepo)
    {
      _opsRepo = opsRepo;
    }

    public async Task<IActionResult> Status()
		{
			try
			{
				var stats = await _opsRepo.GetAllStatus();
				return View(stats);
      }
			catch (Exception ex) 
			{
				return View(Enumerable.Empty<StatusRowViewModel>());
      }
    }
	}
}
