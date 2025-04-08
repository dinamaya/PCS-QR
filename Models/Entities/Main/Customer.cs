using CCIMS.Web.Models.Abstracts;
using CCIMS.Web.Models.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace CCIMS.Web.Models.Entities.Main
{
	public class Customer : HashedEntity, IModifiableByUser, ICreatable, IActivatable
	{
		public Customer() : base("CST") {}

        [Required]
        public string FirstName {  get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Address1 { get; set; }
		public string? Address2 { get; set; }
        [Required]
        public string Province { get; set; }
        [Required]
        public string CityMunicipality { get; set; }
        [Required]
        public string Barangay { get; set; }
        [Required]
        public string ContactNumber { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string? ModifiedBy { get; set; }
        [Required]
        public DateTime DateModified { get; set; }
        [Required]
        public DateTime DateCreated { get; set; }
        [Required]
        public bool IsActive { get; set; }
	}
}
