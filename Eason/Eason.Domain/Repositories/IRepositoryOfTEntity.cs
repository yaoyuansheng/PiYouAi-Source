using Eason.Domain.Entities;

namespace Eason.Domain.Repositories
{
   
    public interface IRepository<TEntity> : IRepository<TEntity, int> where TEntity : class, IEntity<int>
    {

    }
}
