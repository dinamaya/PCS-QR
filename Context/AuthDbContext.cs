using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using CCIMS.Web.Models.Entities.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.Identity.Client;
using CCIMS.Web.App_Code._Globals.Enums;
using CCIMS.Web.Models.SQLViews.Auth;

namespace CCIMS.Web.Context
{
	public class AuthDbContext(DbContextOptions<AuthDbContext> options) : IdentityDbContext<Account>(options)
	{
		public virtual DbSet<Account> Accounts { get; set; }
		public virtual DbSet<Person> People { get; set; }
		public virtual DbSet<AccountsV> AccountsVs { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			modelBuilder.Entity<AccountsV>(entity =>
			{
				entity
						.HasNoKey()
						.ToView("Accounts_v");

				entity.Property(e => e.AccountId).HasMaxLength(450);
				entity.Property(e => e.Email).HasMaxLength(256);
				entity.Property(e => e.RoleName).HasMaxLength(256);
				entity.Property(e => e.PersonId).HasColumnName("PersonID");
				entity.Property(e => e.RoleId).HasMaxLength(450);
				entity.Property(e => e.UserName).HasMaxLength(256);
			});

			modelBuilder.Entity<Account>()
				.Ignore(c => c.EmailConfirmed)
				.Ignore(c => c.PhoneNumber)
				.Ignore(c => c.PhoneNumberConfirmed)
				.Ignore(c => c.TwoFactorEnabled)
				.Ignore(c => c.LockoutEnd)
				.Ignore(c => c.AccessFailedCount)
				.Ignore(c => c.AccessFailedCount)
				.Ignore(c => c.SecurityStamp)
				.Ignore(c => c.ConcurrencyStamp)
				.Ignore(c => c.LockoutEnabled)
        .Property(u => u.Id)
           .HasMaxLength(250);

			//modelBuilder.Entity<Account>()
			//	.HasOne(a => a.Person)
			//	.WithMany()
			//	.HasForeignKey(a => a.PersonID);


      modelBuilder.Entity<IdentityRole>()
				.Ignore(c => c.ConcurrencyStamp)
        .Property(u => u.Id)
           .HasMaxLength(250);


      foreach (var accountType in Enum.GetValues(typeof(AccountType)))
			{
				var roleName = accountType.ToString();
				modelBuilder.Entity<IdentityRole>()
					.HasData(new IdentityRole { Name = roleName, NormalizedName = roleName.ToUpper() });
			}
		}
	}
}
