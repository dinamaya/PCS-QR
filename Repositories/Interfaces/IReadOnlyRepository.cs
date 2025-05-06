using CCIMS.Web.Models.DTOs;

namespace CCIMS.Web.Repositories.Interfaces
{
  /// <summary>
  /// Represents a read-only repository interface for accessing data of type <typeparamref name="TAll"/> and <typeparamref name="TOne"/>.
  /// Provides methods to retrieve all entities or a specific entity by ID.
  /// </summary>
  /// <typeparam name="TAll">The type of the collection of entities to be returned, typically used for listing or summary views.</typeparam>
  /// <typeparam name="TOne">The type of a single entity to be returned, typically used for detailed views.</typeparam>
  public interface IReadOnlyRepository<TAll, TOne>
      where TAll : class
      where TOne : class
  {
    /// <summary>
    /// Retrieves all entities of type <typeparamref name="TAll"/>.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains an enumerable collection of <typeparamref name="TAll"/> entities.</returns>
    Task<IEnumerable<TAll>> GetAll();

    /// <summary>
    /// Retrieves a single entity of type <typeparamref name="TOne"/> by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the entity.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the <typeparamref name="TOne"/> entity if found; otherwise, null.</returns>
    Task<TOne> GetById(string id);
  }

}
