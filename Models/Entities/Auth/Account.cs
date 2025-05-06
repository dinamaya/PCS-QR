using Microsoft.AspNetCore.Identity;
using CCIMS.Web.App_Code._Globals;
using CCIMS.Web.Models.Abstracts;
using CCIMS.Web.Models.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CCIMS.Web.Models.Entities.Auth
{
	public class Account : IdentityUser, IAuditableByUser
	{
    public Account() => base.Id = Utils.Security.GenerateExtendedGuid("ACC", 2);

    public string PersonID { get; set; }

		public string CreatedBy { get; set; }
		public DateTime DateCreated { get; set; }
		public string? ModifiedBy { get; set; }
		public DateTime DateModified { get; set; }
		public bool IsActive { get; set; }
	}
}
