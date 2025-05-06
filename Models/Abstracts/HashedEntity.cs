using CCIMS.Web.App_Code._Globals;
using System.ComponentModel.DataAnnotations;

namespace CCIMS.Web.Models.Abstracts
{
	public abstract class HashedEntity
	{
    [Required, Key] public string Id { get; set; }

    public HashedEntity(string prefix) => Id = Utils.Security.GenerateExtendedGuid(prefix);
  }
}
