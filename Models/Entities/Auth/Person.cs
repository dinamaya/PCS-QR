using CCIMS.Web.Models.Abstracts;
using CCIMS.Web.Models.Interfaces;

namespace CCIMS.Web.Models.Entities.Auth
{
	public class Person : HashedEntity, IAuditableByUser
	{
		public Person() : base(string.Empty) {}

    public string FirstName { get; set; }
		public string LastName { get; set; }

		public string CreatedBy { get; set; }
		public DateTime DateCreated { get; set; }
		public string? ModifiedBy { get; set; }
		public DateTime DateModified { get; set; }
		public bool IsActive { get; set; }
	}
}
