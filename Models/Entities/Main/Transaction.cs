using CCIMS.Web.Models.Abstracts;
using CCIMS.Web.Models.Interfaces;

namespace CCIMS.Web.Models.Entities.Main
{
	public class Transaction : BigEntity, IModifiableByUser, ICreatable, IActivatable
	{
		public string CaseNumber { get; set; }
		public string? Comments { get; set; }
		public string StatusId { get; set; }

		public string CreatedBy { get; set; }
		public DateTime DateCreated { get; set; }
		public string? ModifiedBy { get; set; }
		public DateTime DateModified { get; set; }
		public bool IsActive { get; set; }
	}
}
