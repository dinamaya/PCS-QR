using CCIMS.Web.App_Code._Globals.Constants;
using CCIMS.Web.Context;
using CCIMS.Web.Models.DTOs;
using CCIMS.Web.Models.Entities.Main;
using CCIMS.Web.Models.SQLViews.Main;
using CCIMS.Web.Models.ViewModels;
using CCIMS.Web.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Threading.Tasks;

namespace CCIMS.Web.Repositories
{
  public class CaseRepository : ICaseRepository
	{
		private readonly MainDbContext _context;
    private readonly IConfigurationRepository _configRepo;
    private readonly IServicePartnerRepository _spRepo;

    public CaseRepository(MainDbContext context, IConfigurationRepository configRepo, IServicePartnerRepository spRepo)
    {
      _context = context;
      _configRepo = configRepo;
      _spRepo = spRepo;
    }

    public string InsertedId { get; set; }

		public async Task CreateAsync(Case newCase, string createdBy)
		{
			newCase.CaseNumber = GenerateCaseNumber();
			newCase.DateCreated = DateTime.UtcNow.ToLocalTime();
			newCase.IsActive = true;
			newCase.ModifiedBy = string.Empty;
			newCase.Description = string.Empty;

			_context.Cases.Add(newCase);
			await _context.SaveChangesAsync();
		}

		public async Task<IEnumerable<CaseRowViewModel>> GetAll()
		{
			return await _context.LatestCasesVs
				.Select(c => new CaseRowViewModel()
				{
					Id = c.CaseId.ToString(),
					CaseNumber = c.CaseNumber,
          Description = c.Description,
          Status = c.Status,
          Comments = c.Comments,
					CustomerName = c.CustomerLastName + ", " + c.CustomerFirstName,
					ServicePartnerName = c.ServicePartnerName,
					SerialNumber = c.SerialNumber,
					DateCreated = c.DateStatusUpdated.ToString(Database.DateFormat.DISPLAY),
        }).ToListAsync();
    }

    public async Task<IEnumerable<CaseRowViewModel>> GetByCategory(string categoryId, string value)
    {
      return await FilterCasesByCategory(categoryId, value)
        .Select(
          c => new CaseRowViewModel()
          {
            Id = c.CaseId.ToString(),
            CaseNumber = c.CaseNumber,
            Description = c.Description,
            Status = c.Status,
            Comments = c.Comments,
            CustomerName = c.CustomerLastName + ", " + c.CustomerFirstName,
            ServicePartnerName = c.ServicePartnerName,
            SerialNumber = c.SerialNumber,
            DateCreated= c.DateStatusUpdated.ToString(Database.DateFormat.DISPLAY_COMPLETE),
          }
        ).ToListAsync();
    }

    private IQueryable<LatestCasesV> FilterCasesByCategory(string categoryId, string value)
    {
      var categories = _configRepo.GetCategoriesSearcOptions().ToList();
      var selectedCategory = categories.FirstOrDefault(c => c.Value == categoryId);

      if (selectedCategory == null)
        throw new InvalidOperationException(Exceptions.Message.INVALID_CATEGORY);

      return selectedCategory.Label switch
      {
        "Description" => _context.LatestCasesVs.Where(c => c.Description.Contains(value)),
        "Status" => _context.LatestCasesVs.Where(c => c.StatusId == value),
        "Serial Number" => _context.LatestCasesVs.Where(c => c.SerialNumber.Contains(value)),
        "Case Number / ID" => _context.LatestCasesVs.Where(c => c.CaseNumber.Contains(value)),
        "Service Partner Name" => _context.LatestCasesVs.Where(c => c.ServicePartnerName.Contains(value)),
        "Days Aged" => _context.LatestCasesVs.Where(c => 
          c.DateStatusUpdated != null && 
          EF.Functions.DateDiffDay(c.DateStatusUpdated, DateTime.UtcNow.ToLocalTime()) >= int.Parse(value)),
        _ => throw new InvalidOperationException(Exceptions.Message.INVALID_CATEGORY)
      };
    }


    public async Task<CaseDetailsViewModel> GetById(string id)
		{
      long _id = long.Parse(id);
      return await _context.CaseDetailsVs
        .Where(c => c.Id == _id)
        .Select(_case => new CaseDetailsViewModel()
        {
          Id = _case.Id.ToString(),
          CustomerId = _case.CustomerId,
          DateCreated = _case.DateCreated,
          CaseNumber = _case.CaseNumber,
          ServicePartner = _case.SpName,
          SerialNumber = _case.SerialNumber,
          Description = _case.Description
        })
        .FirstOrDefaultAsync() ?? throw new Exception(Exceptions.Message.INVALID_CASE);
    }

    public async Task<string> GetCurrentStatus(long caseId)
    {
      return await _context.LatestCasesVs
        .AsNoTracking()
        .Where(c => c.CaseId == caseId)
        .Select(c => c.Status)
        .FirstOrDefaultAsync() ?? throw new Exception(Exceptions.Message.INVALID_CASE);
    }

    public async Task<CaseDetailsViewModel> GetByCaseNumber(string caseNumber)
    {
      return await _context.CaseDetailsVs
        .Where(c => c.CaseNumber == caseNumber)
        .Select(_case => new CaseDetailsViewModel()
        {
          Id = _case.Id.ToString(),
          CustomerId = _case.CustomerId,
          DateCreated = _case.DateCreated,
          CaseNumber = _case.CaseNumber,
          ServicePartner = _case.SpName,
          SerialNumber = _case.SerialNumber,
          Description = _case.Description
        })
        .FirstOrDefaultAsync() ?? throw new Exception(Exceptions.Message.INVALID_CASE);
    }

    public string GenerateCaseNumber() => "CC"+Guid.NewGuid().ToString("N")[..4].ToUpper() + DateTime.Now.ToLocalTime().ToString(Database.DateFormat.CASEID);

    public async Task EditAsync(CaseEditRequestDto editRequestDto, string modifiedBy)
    {
      var date = DateTime.Now.ToLocalTime();
      long _caseId = long.Parse(editRequestDto.Id);
      var _case = await _context.Cases.FindAsync(_caseId) ?? throw new Exception(Exceptions.Message.INVALID_CASE);
      string _qrId = await _spRepo.GetQrIdByName(editRequestDto.ServicePartner);

      _case.ModifiedBy = modifiedBy;
      _case.DateModified = date;
      _case.SerialNumber = editRequestDto.SerialNumber;
      _case.QRCodeId = _qrId;

      await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<CaseRowViewModel>> GetDataAged5DaysByServicePartner(string spName)
    {
      return await _context.ServicePartnersWithAgingCasesVs
        .Where(s => s.ServicePartnerName == spName)
        .Select(
          c => new CaseRowViewModel()
          {
            Id = c.Id.ToString(),
            CaseNumber = c.CaseNumber,
            Description = c.Description,
            Status = c.Status,
            Comments = c.Comments,
            CustomerName = c.CustomerName,
            ServicePartnerName = c.ServicePartnerName,
            SerialNumber = c.SerialNumber,
            DateCreated = c.DateStatusUpdated.ToString(Database.DateFormat.DISPLAY_COMPLETE),
          })
        .ToListAsync();
    }

    public async Task<IEnumerable<AgedCaseViewModel>> GetAgedCases()
    {
      var baseUrl = new Uri(_configRepo.GetBaseUrl());

      return await _context.TopAgingCasesAllVs.Select(c => new AgedCaseViewModel()
      {
        CaseTrackingLink = new Uri(baseUrl, $"Cases/Tracking?refNo={c.CaseNumber}").AbsoluteUri,
        CaseNumber = c.CaseNumber,
        DateLastUpdated = c.DateLastUpdated.ToString(Database.DateFormat.DISPLAY_COMPLETE),
        CustomerName = c.LastName + ", " + c.FirstName,
        ServicePartner = c.ServicePartnerName
      }).ToListAsync();
    }
  }
}