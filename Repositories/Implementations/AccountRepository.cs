using CCIMS.Web.App_Code._Globals.Constants;
using CCIMS.Web.Context;
using CCIMS.Web.Models.DTOs;
using CCIMS.Web.Models.Entities.Auth;
using CCIMS.Web.Models.ViewModels;
using CCIMS.Web.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CCIMS.Web.Repositories.Implementations
{
	public class AccountRepository : IAccountRepository
	{
		private readonly AuthDbContext _authDb;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly UserManager<Account> _userManager;
		private readonly PasswordHasher<Account> _passHasher;

		public AccountRepository(AuthDbContext authDb, RoleManager<IdentityRole> roleManager, UserManager<Account> userManager, PasswordHasher<Account> passHasher)
		{
			_authDb = authDb;
			_roleManager = roleManager;
			_userManager = userManager;
			_passHasher = passHasher;
		}

		public async Task CreateAsync(AccountCreationRequestDto creationRequest, string createdBy)
		{
			var date = DateTime.UtcNow.ToLocalTime();

			var person = new Person
			{
				FirstName = creationRequest.FirstName,
				LastName = creationRequest.LastName,
				CreatedBy = createdBy,
				ModifiedBy = createdBy,
				DateCreated = date,
				DateModified = date,
				IsActive = true
			};

			await _authDb.People.AddAsync(person);
			await _authDb.SaveChangesAsync();

			var account = new Account
			{
				UserName = creationRequest.Username,
				Email = creationRequest.Email,
				PersonID = _authDb.People.FindAsync(person.Id).Result.Id ?? person.Id,
				CreatedBy = createdBy,
				ModifiedBy = createdBy,
				DateCreated = date,
				DateModified = date,
				IsActive = true
			};

			var result = await _userManager.CreateAsync(account, creationRequest.Password);
			
			if (!result.Succeeded)
				throw new Exception($"Failed to create account {account.UserName}: {string.Join(", ", result.Errors.Select(e => e.Description))}");

			await _authDb.SaveChangesAsync();
			await _userManager.AddToRoleAsync(account, creationRequest.AccountType);
			await _authDb.SaveChangesAsync();
		}

		public async Task<IEnumerable<AccountRowViewModel>> GetAll() =>
			await _authDb.AccountsVs
			.AsNoTracking()
			.OrderByDescending(a => a.DateCreated)
			.Select(a => new AccountRowViewModel()
			{
				Id = a.AccountId,
				FirstName = a.FirstName,
				LastName = a.LastName,
				Email = a.Email,
				Username = a.UserName,
				CreatedBy = a.Creator,
				DateCreated = a.DateCreated.ToString(Database.DateFormat.DISPLAY_COMPLETE),
				Type = a.RoleName,
			})
			.ToListAsync();

		public async Task<IEnumerable<DropdownOptionViewModel>> GetAllRoles()
		{
			return await _roleManager
				.Roles
				.Select(r => new DropdownOptionViewModel()
				{
					Label = r.Name,
					Value = r.Name,
				})
				.ToListAsync();
		}

		public async Task<AccountEditResponseDto> GetById(string id)
		{
			return await _authDb.AccountsVs
				.AsNoTracking()
				.Where(a => a.AccountId == id)
				.Select(data => new AccountEditResponseDto()
				{
					FirstName = data.FirstName,
					LastName = data.LastName,
					Email = data.Email,
					Username = data.UserName,
					Type = data.RoleName,
				})
				.FirstOrDefaultAsync() ?? throw new Exception(Exceptions.Message.INVALID_ACCOUNTREFERENCE);
		}

		public async Task EditAsync(AccountEditRequestDto editRequestDto, string modifiedBy)
		{
			var date = DateTime.UtcNow;
			Account account = await _userManager.FindByIdAsync(editRequestDto.Id) ?? throw new Exception(Exceptions.Message.INVALID_ACCOUNTREFERENCE);

      if (!string.Equals(account.UserName, editRequestDto.Username, StringComparison.OrdinalIgnoreCase))
      {
        var setUsernameResult = await _userManager.SetUserNameAsync(account, editRequestDto.Username);
        if (!setUsernameResult.Succeeded)
          throw new InvalidOperationException("Failed to update " + string.Join(", ", setUsernameResult.Errors.Select(e => e.Description)));
      }

      if (!string.Equals(account.Email, editRequestDto.Email, StringComparison.OrdinalIgnoreCase))
      {
        var setEmailResult = await _userManager.SetEmailAsync(account, editRequestDto.Email);
        if (!setEmailResult.Succeeded)
          throw new InvalidOperationException("Failed to update " + string.Join(", ", setEmailResult.Errors.Select(e => e.Description)));
      }


      account.DateModified = date;
			account.ModifiedBy = modifiedBy;

			if (editRequestDto.Password.IsNullOrEmpty())
				account.PasswordHash = _passHasher.HashPassword(account, editRequestDto.Password);
			
			var result = await _userManager.UpdateAsync(account);

			if (!result.Succeeded) throw new Exception(Exceptions.Message.INVALID_ACCOUNT_UPDATE);

			Person person = await _authDb.People.FindAsync(account.PersonID);
			person.FirstName = editRequestDto.FirstName;
			person.LastName = editRequestDto.LastName;
			person.DateModified = date;
			person.ModifiedBy = modifiedBy;

			await _authDb.SaveChangesAsync();

			var currentRoles = await _userManager.GetRolesAsync(account);
			await _userManager.RemoveFromRolesAsync(account, currentRoles);
			await _userManager.AddToRoleAsync(account, editRequestDto.AccountType);
		}

    public async Task ValidateInputs(AccountCreationRequestDto creationRequest)
    {
			var username = creationRequest.Username.ToLower();
			var email = creationRequest.Email.ToLower();

      var isUsernameExist = await _authDb.Accounts.AnyAsync(a => a.UserName.ToLower() == username);
      var isEmailExist = await _authDb.Accounts.AnyAsync(a => a.Email.ToLower() == email);

			if (isUsernameExist)
				throw new InvalidOperationException(Exceptions.Message.INVALID_USERNAME);

			if (isEmailExist)
				throw new InvalidOperationException(Exceptions.Message.INVALID_EMAIL);
    }

    public async Task DeactivateAsync(string id)
    {
      var date = DateTime.Now.ToLocalTime();

      using var transaction = await _authDb.Database.BeginTransactionAsync();
			
      var account = await _authDb.Accounts.FindAsync(id) ?? throw new Exception(Exceptions.Message.INVALID_ACCOUNTREFERENCE);
      var person = await _authDb.People.FindAsync(account.PersonID) ?? throw new Exception(Exceptions.Message.INVALID_PERSONREFERENCE);

			account.DateModified = date;
      account.IsActive = false;

      var result = await _userManager.UpdateAsync(account);
      if (!result.Succeeded)
        throw new Exception(Exceptions.Message.INVALID_ACCOUNT_DELETE);
      
			person.DateModified = date;
      person.IsActive = false;

      _authDb.People.Update(person);
      await _authDb.SaveChangesAsync();
      await transaction.CommitAsync();
    }
  }
}
