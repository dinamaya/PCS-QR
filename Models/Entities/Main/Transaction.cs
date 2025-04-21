using CCIMS.Web.Models.Abstracts;
using CCIMS.Web.Models.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace CCIMS.Web.Models.Entities.Main
{
	public class Transaction : BigEntity, IModifiableByUser, ICreatable, IActivatable
	{
		public long CaseID { get; set; }
		public string? Comments { get; set; }
		public string StatusId { get; set; }

		public string CreatedBy { get; set; }
		public DateTime DateCreated { get; set; }
		public string? ModifiedBy { get; set; }
		public DateTime DateModified { get; set; }
		public bool IsActive { get; set; }

		[ForeignKey(nameof(StatusId))] public virtual Status Status { get; set; }
		[ForeignKey(nameof(CaseID))] public virtual Case Case { get; set; }
	}
}
