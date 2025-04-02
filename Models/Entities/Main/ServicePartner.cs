using CCIMS.Web.Models.Abstracts;
using CCIMS.Web.Models.Interfaces;

namespace CCIMS.Web.Models.Entities.Main
{
	public class ServicePartner : HashedEntity, IAuditableByUser
	{
		public ServicePartner() : base("SVP") {}
    
		public string Name { get; set; }

		public string CompanyName { get; set; }
		public string ContactNumber { get; set; }
		public string Email { get; set; }
		public string ContactPerson { get; set; }

		public string CreatedBy { get; set; }
		public DateTime DateCreated { get; set; }
		public string? ModifiedBy { get; set; }
		public DateTime DateModified { get; set; }
		public bool IsActive { get; set; }
	}
}
