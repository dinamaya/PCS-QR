using CCIMS.Web.Models.DTOs;

namespace CCIMS.Web.Repositories.Interfaces
{
	public interface IEditRepository<TModel> where TModel : class
	{
		Task EditAsync(TModel editRequestDto, string modifiedBy);
	}
}
