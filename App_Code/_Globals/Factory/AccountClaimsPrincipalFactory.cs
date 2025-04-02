using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using CCIMS.Web.App_Code._Globals.Constants;
using CCIMS.Web.Context;
using CCIMS.Web.Models.Entities.Auth;
using System.Security.Claims;

namespace CCIMS.Web.App_Code._Globals.Factory
{
	public class AccountClaimsPrincipalFactory : UserClaimsPrincipalFactory<Account>
	{
		private readonly AuthDbContext _authDb;
		public AccountClaimsPrincipalFactory(
				UserManager<Account> userManager,
				IOptions<IdentityOptions> optionsAccessor,
				AuthDbContext authDb) : base(userManager, optionsAccessor)
		{
			_authDb = authDb;
		}

		protected override async Task<ClaimsIdentity> GenerateClaimsAsync(Account account)
		{
			var roles = await UserManager.GetRolesAsync(account);
			var identity = await base.GenerateClaimsAsync(account);
			var currAccount = await _authDb.AccountsVs.FirstOrDefaultAsync(a => a.AccountId == account.Id);

			identity.AddClaim(new(ClaimTypes.Role, roles.FirstOrDefault()));
			identity.AddClaim(new(ClaimTypes.Name, account.UserName));
			identity.AddClaim(new(ClaimTypes.NameIdentifier, account.Id));
			identity.AddClaim(new(AuthClaims.EMAIL, account.Email ?? ""));
			identity.AddClaim(new(AuthClaims.FIRSTNAME, currAccount.FirstName));
			identity.AddClaim(new(AuthClaims.LASTNAME, currAccount.LastName));

			identity.AddClaim(new(AuthClaims.DATE_CREATED, account.DateCreated.ToString()));
			identity.AddClaim(new(AuthClaims.DATE_MODIFIED, account.DateModified.ToString()));

			identity.AddClaim(new(AuthClaims.ACCOUNT_ID, account.Id));
			identity.AddClaim(new(AuthClaims.PERSON_ID, currAccount.PersonId.ToString()));
			identity.AddClaim(new(AuthClaims.ROLE, currAccount.RoleName));

			return identity;
		}
	}
}
