namespace CCIMS.Web.Repositories.Interfaces
{
  public interface ICreateRepository<TModel> where TModel : class
  {
    string InsertedId { get; set; }
    Task CreateAsync(TModel data, string createdBy);
  }
}
