using System.ComponentModel.DataAnnotations;

namespace CCIMS.Web.Models.Interfaces
{
	public interface IModifiableByUser : IModifiable
	{
		public string? ModifiedBy { get; set; }
	}
}
