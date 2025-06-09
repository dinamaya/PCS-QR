using CCIMS.Web.Models.DTOs;
using CCIMS.Web.Models.Entities.Main;
using CCIMS.Web.Models.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace CCIMS.Web.Repositories.Interfaces
{
	public interface ICaseRepository : 
		IReadOnlyRepository<CaseRowViewModel, CaseDetailsViewModel>, 
		ICreateRepository<Case>, 
		IEditRepository<CaseEditRequestDto>
	{
		Task<IEnumerable<CaseRowViewModel>> GetByCategory(string categoryId, string value);
		Task<IEnumerable<AgedCaseViewModel>> GetAgedCases();
		Task<string> GetCurrentStatus(long caseId);
		Task<CaseDetailsViewModel> GetByCaseNumber(string caseNumber);
		string GenerateCaseNumber();
    Task<IEnumerable<CaseRowViewModel>> GetDateRangeFilteredCasesByCategory(string categoryId, string value, DateTime? startDate = null, DateTime? endDate = null);
		Task<IEnumerable<CaseRowViewModel>> GetDataAged3DaysByServicePartner(string spName);
  }
}
