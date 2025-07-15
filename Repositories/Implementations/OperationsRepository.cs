using CCIMS.Web.App_Code._Globals.Constants;
using CCIMS.Web.Context;
using CCIMS.Web.Models.DTOs;
using CCIMS.Web.Models.Entities.Main;
using CCIMS.Web.Models.ViewModels;
using CCIMS.Web.Repositories.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
      string statName = data.Name.Trim();
      var isExist = await _mainDb.Statuses.AnyAsync(s => EF.Functions.Like(s.Name, statName) && s.IsActive);

      if (isExist)
        throw new InvalidOperationException(Exceptions.Message.INVALID_STATUS_EXIST);

      var status = new Status()
      {
        Name = statName,
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

      string statName = editRequestDto.Name.Trim();
      var isExist = await _mainDb.Statuses.AnyAsync(s => EF.Functions.Like(s.Name, statName) && s.Id != editRequestDto.Id && s.IsActive);

      if (isExist)
        throw new InvalidOperationException(Exceptions.Message.INVALID_STATUS_EXIST);

      if (statName.Equals("Closed", StringComparison.OrdinalIgnoreCase) || statName.Equals("On-Queue", StringComparison.OrdinalIgnoreCase))
        throw new Exception(statName + " " + Exceptions.Message.INVALID_STATUS_EDIT_1);

			status.Name = statName;
      status.IsCommentable = editRequestDto.IsCommentable;
      status.ModifiedBy = modifiedBy;
      status.DateModified = DateTime.UtcNow.ToLocalTime();
     
      await _mainDb.SaveChangesAsync();
		}

		public async Task<IEnumerable<StatusRowViewModel>> GetAllStatus()
    {
      return await _mainDb.Statuses
        .AsNoTracking()
        .Where(s => s.IsActive)
        .OrderByDescending(s => s.DateCreated)
        .Select(s => new StatusRowViewModel()
        {
          Id = s.Id,
          StatusName = s.Name,
          IsCommentable = s.IsCommentable ? "Commentable" : "Not",
          DateCreated = s.DateCreated.ToString(Database.DateFormat.DISPLAY_COMPLETE),
          IsEditable = !(s.Name.Equals("Closed") || s.Name.Equals("On-Queue"))
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
        .Where(s => s.IsActive)
        .OrderBy(s => s.Name)
        .Select(s => new DropdownOptionViewModel()
        {
          Value = s.Id,
                  Label = s.Name,
        })
        .ToListAsync();
    }

		public async Task<bool> IsStatusCommentable(string id)
		{
      var result = await _mainDb.Statuses.FindAsync(id) ?? throw new Exception(Exceptions.Message.INVALID_STATUS);
      return result.IsCommentable;
		}

    public async Task DeactivateAsync(string id)
    {
      var date = DateTime.Now.ToLocalTime();

      var status = await _mainDb.Statuses.FindAsync(id) ?? throw new Exception(Exceptions.Message.INVALID_STATUS);

      if (status.Name.Equals("Closed") || status.Name.Equals("On-Queue"))
        throw new Exception(status.Name + " " + Exceptions.Message.INVALID_STATUS_EDIT_1);

            var transactionsToDeactivate = await _mainDb.Transactions
                .Where(t => t.StatusId == id && t.IsActive)
                .ToListAsync();

            foreach (var transaction in transactionsToDeactivate)
            {
                var previousTransaction = await _mainDb.Transactions
                    .Where(t => t.CaseID == transaction.CaseID && t.DateCreated < transaction.DateCreated)
                    .OrderByDescending(t => t.DateCreated)
                    .FirstOrDefaultAsync();

                if (previousTransaction != null)
                {
                    previousTransaction.IsActive = true;
                }
                transaction.IsActive = false;
            }

      status.IsActive = false;
      status.DateModified = date;

      await _mainDb.SaveChangesAsync();
    }
  }
}
