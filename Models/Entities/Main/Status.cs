using CCIMS.Web.Models.Abstracts;
using CCIMS.Web.Models.Interfaces;

namespace CCIMS.Web.Models.Entities.Main
{
	public class Status : HashedEntity, IAuditableByUser
	{
		public Status() : base("STS") {}
    
		public string Name { get; set; }
		public bool IsCommentable { get; set; }

		public string CreatedBy { get; set; }
		public DateTime DateCreated { get; set; }
		public string ModifiedBy { get; set; }
		public DateTime DateModified { get; set; }
		public bool IsActive { get; set; }
	}
}
