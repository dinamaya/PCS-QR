using System.ComponentModel.DataAnnotations;

namespace CCIMS.Web.Models.Abstracts;

public abstract class BigEntity
{
	[Key, Required] public long Id { get; set; }
}
