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

    public string InsertedId { get; set; }

    public async Task CreateAsync(StatusDto data, string createdBy)
    {
      var date = DateTime.UtcNow.ToLocalTime();

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

      InsertedId = status.Id;
			await _mainDb.Statuses.AddAsync(status);
      await _mainDb.SaveChangesAsync();
    }

		public async Task EditAsync(StatusEditRequestDto editRequestDto, string modifiedBy)
		{
			var status = await _mainDb.Statuses.FindAsync(editRequestDto.Id) ?? throw new Exception(Exceptions.Message.INVALID_STATUS);

			status.Name = editRequestDto.Name;
      status.IsCommentable = editRequestDto.IsCommentable;
      status.ModifiedBy = modifiedBy;
      status.DateModified = DateTime.UtcNow.ToLocalTime();
     
      await _mainDb.SaveChangesAsync();
		}

		public async Task<IEnumerable<StatusRowViewModel>> GetAllStatus()
    {
      return await _mainDb.Statuses
        .AsNoTracking()
        .OrderByDescending(s => s.DateCreated)
        .Select(s => new StatusRowViewModel()
        {
          Id = s.Id,
          StatusName = s.Name,
          IsCommentable = s.IsCommentable ? "Commentable" : "Not",
          DateCreated = s.DateCreated.ToString(Database.DateFormat.DISPLAY_COMPLETE),
          IsActive = s.IsActive ? "Active" : "Not"
        })
        .ToListAsync();
    }

    public async Task<StatusDto> GetStatusById(string id)
		{
      var status = await _mainDb.Statuses.FindAsync(id) ?? throw new Exception(Exceptions.Message.INVALID_STATUS);
      return new StatusDto()
			{
				Name = status.Name,
				IsCommentable = status.IsCommentable,
			};
    }

    public async Task<IEnumerable<DropdownOptionViewModel>> GetOptions()
    {
      return await _mainDb.Statuses
        .AsNoTracking()
        .OrderBy(s => s.Name)
        .Select(s => new DropdownOptionViewModel()
        {
          Value = s.Id,
          Label= s.Name,
        })
        .ToListAsync();
    }
  }
}
