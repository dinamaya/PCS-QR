using CCIMS.Web.Models.Abstracts;
using CCIMS.Web.Models.Interfaces;

namespace CCIMS.Web.Models.Entities.Main
{
	public class Customer : HashedEntity, IModifiableByUser, ICreatable, IActivatable
	{
		public Customer() : base("CST") {}
    
		public string FirstName {  get; set; }
		public string LastName { get; set; }

		public string Address1 { get; set; }
		public string Address2 { get; set; }
		public string Province { get; set; }
		public string City { get; set; }
		public string ContactNumber { get; set; }
		public string Email { get; set; }

		public string? ModifiedBy { get; set; }
		public DateTime DateModified { get; set; }
		public DateTime DateCreated { get; set; }
		public bool IsActive { get; set; }
	}
}
