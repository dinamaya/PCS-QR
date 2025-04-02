using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CCIMS.Web.App_Code._Globals.Enums;
using CCIMS.Web.Models.Entities.Main;
using System.Reflection.Emit;

namespace CCIMS.Web.Context
{
	public class MainDbContext(DbContextOptions<MainDbContext> options) : DbContext(options)
	{
		public virtual DbSet<Case> Cases { get; set; }
		public virtual DbSet<Customer> Customers { get; set; }
		public virtual DbSet<NonWorkingDate> NonWorkingDates { get; set; }
		public virtual DbSet<QRCode> QRCodes { get; set; }
    public virtual DbSet<ServicePartner> ServicePartners { get; set; }
		public virtual DbSet<Status> Statuses { get; set; }
		public virtual DbSet<Transaction> Transactions { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Case>()
				.HasOne(a => a.QRCode)
				.WithMany()
				.HasForeignKey(a => a.QRCodeId);

			base.OnModelCreating(modelBuilder);
		}
	}
}
