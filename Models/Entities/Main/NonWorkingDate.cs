using CCIMS.Web.Models.Abstracts;
using CCIMS.Web.Models.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace CCIMS.Web.Models.Entities.Main
{
	public class NonWorkingDate : BigEntity, ICreatable, IModifiable, IActivatable
	{
		[DataType(DataType.Date)] public DateOnly Date {  get; set; }
		public string Description {  get; set; }

		public DateTime DateCreated { get; set; }
		public DateTime DateModified { get; set; }
		public bool IsActive { get; set; }
	}
}
