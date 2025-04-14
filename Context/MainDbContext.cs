using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CCIMS.Web.App_Code._Globals.Enums;
using CCIMS.Web.Models.Entities.Main;
using System.Reflection.Emit;
using CCIMS.Web.Models.SQLViews.Main;

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

    public virtual DbSet<ServicePartnersV> ServicePartnersVs { get; set; }
    public virtual DbSet<StatusesV> StatusesVs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Case>()
				.HasOne(a => a.QRCode)
				.WithMany()
				.HasForeignKey(a => a.QRCodeId);
			
			modelBuilder.Entity<QRCode>()
				.HasOne(a => a.ServicePartner)
				.WithMany()
				.HasForeignKey(a => a.ServicePartnerId);

      modelBuilder.Entity<ServicePartnersV>(entity =>
      {
        entity
            .HasNoKey()
            .ToView("ServicePartners_v");

        entity.Property(e => e.CreatorUsername).HasMaxLength(256);
        entity.Property(e => e.QrId).HasMaxLength(450);
        entity.Property(e => e.SpId).HasMaxLength(450);
      });

      modelBuilder.Entity<StatusesV>(entity =>
      {
        entity
            .HasNoKey()
            .ToView("Statuses_v");

        entity.Property(e => e.Id).HasMaxLength(450);
      });

      base.OnModelCreating(modelBuilder);
		}
	}
}
