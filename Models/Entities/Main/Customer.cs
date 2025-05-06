using CCIMS.Web.Models.Abstracts;
using CCIMS.Web.Models.Interfaces;
using System.ComponentModel.DataAnnotations;
using CCIMS.Web.App_Code._Globals.Constants;

namespace CCIMS.Web.Models.Entities.Main
{
	public class Customer : HashedEntity, IModifiableByUser, ICreatable, IActivatable
	{
		public Customer() : base("CST") { }

		[Required]
		[RegularExpression(RegEx.NAMES, ErrorMessage = "First name contains invalid characters.")]
		public string FirstName { get; set; }
		[Required]
		[RegularExpression(RegEx.NAMES, ErrorMessage = "Last name contains invalid characters.")]
		public string LastName { get; set; }
		[Required]
		public string Address { get; set; }
		[Required(ErrorMessage = "Contact number is required.")]
		[RegularExpression(RegEx.PHONE, ErrorMessage = "Only numeric characters are allowed.")]
		public string ContactNumber { get; set; }
		[Required(ErrorMessage = "Email address is required.")]
		[RegularExpression(RegEx.EMAIL_STRICT, ErrorMessage = "Invalid email format.")]
		public string Email { get; set; }
		public string? ModifiedBy { get; set; }
		[Required]
		public DateTime DateModified { get; set; }
		[Required]
		public DateTime DateCreated { get; set; }
		[Required]
		public bool IsActive { get; set; }
	}
}
