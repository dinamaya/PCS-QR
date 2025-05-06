using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CCIMS.Web.App_Code._Globals.Enums;
using CCIMS.Web.Models.Entities.Main;
using System.Reflection.Emit;
using CCIMS.Web.Models.SQLViews.Main;
using CCIMS.Web.Models.Entities.HangFire;

namespace CCIMS.Web.Context
{
	public class MainDbContext(DbContextOptions<MainDbContext> options) : DbContext(options)
	{
		public virtual DbSet<Case> Cases { get; set; }
		public virtual DbSet<Customer> Customers { get; set; }
		public virtual DbSet<QRCode> QRCodes { get; set; }
		public virtual DbSet<ServicePartner> ServicePartners { get; set; }
		public virtual DbSet<Status> Statuses { get; set; }
		public virtual DbSet<Transaction> Transactions { get; set; }

		public virtual DbSet<CaseDetailsV> CaseDetailsVs { get; set; }
		public virtual DbSet<LatestCasesV> LatestCasesVs { get; set; }
		public virtual DbSet<ServicePartnersV> ServicePartnersVs { get; set; }
		public virtual DbSet<StatusesV> StatusesVs { get; set; }
		public virtual DbSet<TransactionsV> TransactionsVs { get; set; }
		public virtual DbSet<AgingCasesV> AgingCasesVs { get; set; }
        public virtual DbSet<ClosedCasesV> ClosedCasesVs { get; set; }
        public virtual DbSet<CasesCreatedTodayV> CasesCreatedTodayVs { get; set; }
        public virtual DbSet<WeeklyCasesChartV> WeeklyCasesChartVs { get; set; }
        public virtual DbSet<WeeklyAgingCasesChartV> WeeklyAgingCasesChartVs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
            modelBuilder.Entity<WeeklyAgingCasesChartV>(entity =>
            {
                entity
                    .HasNoKey()
                    .ToView("WeeklyAgingCasesChart_v");

                entity.Property(e => e.DayOfWeek).HasMaxLength(30);
            });

            modelBuilder.Entity<WeeklyCasesChartV>(entity =>
            {
                entity
                    .HasNoKey()
                    .ToView("WeeklyCasesChart_v");

                entity.Property(e => e.DayOfWeek).HasMaxLength(30);
            });

            modelBuilder.Entity<CasesCreatedTodayV>(entity =>
            {
                entity
                    .HasNoKey()
                    .ToView("CasesCreatedToday_v");

                entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.QrcodeId)
                    .HasMaxLength(450)
                    .HasColumnName("QRCodeId");
            });

            modelBuilder.Entity<ClosedCasesV>(entity =>
            {
                entity
                    .HasNoKey()
                    .ToView("ClosedCases_v");

                entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
                entity.Property(e => e.ServicePartnerId).HasMaxLength(450);
                entity.Property(e => e.StatusId).HasMaxLength(450);
            });

            modelBuilder.Entity<AgingCasesV>(entity =>
			{
				entity
					.HasNoKey()
					.ToView("AgingCases_v");
			});

			modelBuilder.Entity<Case>()
				.HasOne(a => a.QRCode)
				.WithMany()
				.HasForeignKey(a => a.QRCodeId);

			modelBuilder.Entity<QRCode>()
				.HasOne(a => a.ServicePartner)
				.WithMany()
				.HasForeignKey(a => a.ServicePartnerId);

			modelBuilder.Entity<Transaction>(entity =>
			{
				entity
			  .HasOne(a => a.Case)
						  .WithMany()
						  .HasForeignKey(a => a.CaseID);

				entity
			  .HasOne(a => a.Status)
						  .WithMany()
						  .HasForeignKey(a => a.StatusId);
			});

			modelBuilder.Entity<CaseDetailsV>(entity =>
			{
				entity
						.HasNoKey()
						.ToView("CaseDetails_v");

				entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
			});

			modelBuilder.Entity<LatestCasesV>(entity =>
	  {
		  entity
			  .HasNoKey()
			  .ToView("LatestCases_v");

		  entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
		  entity.Property(e => e.ServicePartnerId).HasMaxLength(450);
		  entity.Property(e => e.StatusId).HasMaxLength(450);
	  });

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

			modelBuilder.Entity<TransactionsV>(entity =>
			{
				entity
			  .HasNoKey()
			  .ToView("Transactions_v");

        entity.Property(e => e.CaseId).HasColumnName("CaseID");
        entity.Property(e => e.StatusId).HasMaxLength(450);
      });

			base.OnModelCreating(modelBuilder);
		}
	}
}
