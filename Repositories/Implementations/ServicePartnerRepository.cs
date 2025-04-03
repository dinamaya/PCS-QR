using CCIMS.Web.App_Code._Globals.Constants;
using CCIMS.Web.Context;
using CCIMS.Web.Models.Entities.Main;
using CCIMS.Web.Models.ViewModels;
using CCIMS.Web.Repositories.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

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

    public async Task CreateAsync(ServicePartner data)
    {
      await _mainDb.ServicePartners.AddAsync(data);
      await _mainDb.SaveChangesAsync();
    }

    public async Task<IEnumerable<ServicePartnerRowViewModel>> GetAll()
    {
      return await _mainDb.ServicePartnersVs
        .Select(s => new ServicePartnerRowViewModel()
        {
          SpId = s.SpId,
          QrId = s.QrId,
          ServicePartnerName = s.SpName,
          CompanyName = s.CompanyName,
          ContactNumber = s.ContactNumber,
          ContactEmail = s.Email,
          ContactPerson = s.ContactPerson,
          QRDescription = s.QrDescription,
          CreatedBy = s.CreatorLastName.IsNullOrEmpty() || s.CreatorFirstName.IsNullOrEmpty() ? "" : s.CreatorLastName + ", " + s.CreatorFirstName,
          SpDateCreated = s.SpDateCreated.ToString(Database.DateFormat.DISPLAY_COMPLETE),
          QrDateCreated = s.QrDateCreated.ToString(),
        })
        .ToListAsync();
    }

    public Task<string> GetId()
    {
      throw new NotImplementedException();
    }
  }
}
