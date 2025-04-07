using CCIMS.Web.App_Code._Globals.Constants;
using CCIMS.Web.Context;
using CCIMS.Web.Models.DTOs;
using CCIMS.Web.Models.Entities.Auth;
using CCIMS.Web.Models.ViewModels;
using CCIMS.Web.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CCIMS.Web.Repositories.Implementations
{
	public class AccountRepository : IAccountRepository
	{
		private readonly AuthDbContext _authDb;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly UserManager<Account> _userManager;

		public AccountRepository(AuthDbContext authDb, RoleManager<IdentityRole> roleManager, UserManager<Account> userManager)
		{
			_authDb = authDb;
			_roleManager = roleManager;
			_userManager = userManager;
		}

		public async Task CreateAsync(AccountCreationRequestDto creationRequest, string createdBy)
		{
			var date = DateTime.UtcNow;

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
			await _userManager.AddToRoleAsync(account, creationRequest.Type);
			await _authDb.SaveChangesAsync();
		}

		public async Task<IEnumerable<AccountRowViewModel>> GetAll() =>
			await _authDb.AccountsVs
			.AsNoTracking()
			.Select(a => new AccountRowViewModel()
			{
				Id = a.AccountId,
				FirstName = a.FirstName,
				LastName = a.LastName,
				Email = a.Email,
				Username = a.UserName,
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
	}
}
