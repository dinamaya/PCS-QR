using CCIMS.Web.Models.Abstracts;
using CCIMS.Web.Models.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace CCIMS.Web.Models.Entities.Main
{
	public class Customer : HashedEntity, IModifiableByUser, ICreatable, IActivatable
	{
		public Customer() : base("CST") { }

		[Required]
		public string FirstName { get; set; }
		[Required]
		public string LastName { get; set; }
		[Required]
		public string Address { get; set; }
		[Required(ErrorMessage = "Contact number is required.")]
		[RegularExpression(@"^\d+$", ErrorMessage = "Only numeric characters are allowed.")]
		public string ContactNumber { get; set; }
		[Required(ErrorMessage = "Email address is required.")]
		[EmailAddress(ErrorMessage = "Invalid email address format.")]
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
