using System.ComponentModel.DataAnnotations;

namespace CCIMS.Web.Models.Abstracts;

public abstract class SmallEntity
{
	[Key, Required] public int ID { get; set; }
}
