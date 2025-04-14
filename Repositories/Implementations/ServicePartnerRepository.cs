using CCIMS.Web.App_Code._Globals.Constants;
using CCIMS.Web.Context;
using CCIMS.Web.Models.DTOs;
using CCIMS.Web.Models.Entities.Main;
using CCIMS.Web.Models.ViewModels;
using CCIMS.Web.Repositories.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CCIMS.Web.Repositories.Implementations
{
  public class ServicePartnerRepository : IServicePartnerRepository
  {
    private readonly MainDbContext _mainDb;

    public ServicePartnerRepository(MainDbContext mainDb)
    {
      _mainDb = mainDb;
    }

    public string InsertedId { get; set; }

    public async Task CreateAsync(SPCreationRequestDto data, string createdBy)
    {
      var date = DateTime.Now;

      var sp = new ServicePartner()
      {
        Name = data.Name,
        CompanyName = data.CompanyName,
        ContactNumber = data.ContactNumber,
        Email = data.Email,
        ContactPerson = data.ContactPerson,
        CreatedBy = createdBy,
        DateCreated = date,
        DateModified = date,
        IsActive = true,
      };

      await _mainDb.ServicePartners.AddAsync(sp);
      await _mainDb.SaveChangesAsync();
    }

		public async Task<IEnumerable<ServicePartnerRowViewModel>> GetAll()
    {
      return await _mainDb.ServicePartnersVs
        .AsNoTracking()
				.Select(s => new ServicePartnerRowViewModel()
        {
          SpId = s.SpId,
          QrId = s.QrId,
          ServicePartnerName = s.SpName,
          CompanyName = s.CompanyName,
          ContactNumber = s.ContactNumber,
          ContactEmail = s.Email,
          ContactPerson = s.ContactPerson,
          CreatedBy = s.CreatorLastName.IsNullOrEmpty() || s.CreatorFirstName.IsNullOrEmpty() ? "" : s.CreatorLastName + ", " + s.CreatorFirstName,
          SpDateCreated = s.SpDateCreated.ToString(Database.DateFormat.DISPLAY_COMPLETE),
          QrDateCreated = s.QrDateCreated.ToString(),
        })
				.OrderByDescending(s => s.SpDateCreated)
				.ToListAsync();
    }

    public async Task<SPEditResponseDto?> GetById(string id)
    {
      return await _mainDb.ServicePartners
				.Where(s => s.Id == id)
        .AsNoTracking()
				.Select(s => new SPEditResponseDto()
        {
          Name = s.Name,
          CompanyName = s.CompanyName,
          ContactNumber = s.ContactNumber,
          Email = s.Email,
          ContactPerson = s.ContactPerson,
        })
        .FirstOrDefaultAsync();
    }

		public async Task<ServicePartner?> Get(string id)
		{
			return await _mainDb.ServicePartners
				.Where(s => s.Id == id)
				.FirstOrDefaultAsync();
		}


		public async Task EditAsync(SPEditRequestDto editRequestDto, string modifiedBy)
		{
      var data = await Get(editRequestDto.ID) ?? throw new Exception(Exceptions.Message.INVALID_SPREFERENCE);

			data.Name = editRequestDto.Name;
      data.CompanyName = editRequestDto.CompanyName;
      data.ContactNumber = editRequestDto.ContactNumber;
      data.Email = editRequestDto.Email;
      data.ContactPerson = editRequestDto.ContactPerson;
      data.ModifiedBy = modifiedBy;
      data.DateModified = DateTime.UtcNow;

			await _mainDb.SaveChangesAsync();
		}
	}
}
