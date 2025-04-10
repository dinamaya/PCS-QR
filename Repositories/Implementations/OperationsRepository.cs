using CCIMS.Web.App_Code._Globals.Constants;
using CCIMS.Web.Context;
using CCIMS.Web.Models.DTOs;
using CCIMS.Web.Models.Entities.Main;
using CCIMS.Web.Models.ViewModels;
using CCIMS.Web.Repositories.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace CCIMS.Web.Repositories.Implementations
{
  public class OperationsRepository : IOperationsRepository
  {
    private readonly MainDbContext _mainDb;

    public OperationsRepository(MainDbContext mainDb)
    {
      _mainDb = mainDb;
    }

    public string InsertedId { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public async Task CreateAsync(StatusCreationRequestDto data, string createdBy)
    {
      var date = DateTime.UtcNow;

      var status = new Status()
      {
        Name = data.Name,
        IsCommentable = data.IsCommentable,
        CreatedBy = createdBy,
        DateCreated = date,
        ModifiedBy = createdBy,
        DateModified = date,
        IsActive = true
      };

      await _mainDb.Statuses.AddAsync(status);
      await _mainDb.SaveChangesAsync();
    }

    public async Task<IEnumerable<StatusRowViewModel>> GetAllStatus()
    {
      return await _mainDb.Statuses
        .Select(s => new StatusRowViewModel()
        {
          Id = s.Id,
          StatusName = s.Name,
          IsCommentable = s.IsCommentable ? "Commentable" : "Not",
          //CreatedBy = s.CreatedBy,
          DateCreated = s.DateCreated.ToString(Database.DateFormat.DISPLAY_COMPLETE),
          IsActive = s.IsActive ? "Active" : "Not"
        })
        .ToListAsync();
    }
  }
}
