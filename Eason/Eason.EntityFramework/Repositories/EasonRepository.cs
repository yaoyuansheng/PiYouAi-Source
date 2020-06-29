using Eason.Domain.Entities;
using Eason.Domain.Repositories;
using Eason.Domain.Uow;
using System;
using System.Data.Entity;

namespace Eason.EntityFramework.Repositories
{
    public class EasonRepository<TEntity, TPrimaryKey> : RepositoryBase<TEntity, TPrimaryKey>, IDisposable where TEntity : class, IEntity<TPrimaryKey>
    {
        public IUnitOfWork UnitOfWork { get; set; }
        public override DbContext Context { get; set; }
        public EasonRepository()
        {
            UnitOfWork = new UnitOfWork();
            UnitOfWork.Context = new EasonEntities();
            this.Context = UnitOfWork.Context;
        }
        public void Dispose()
        {
            if (Context != null)
            {
                (Context as IDisposable).Dispose();
            }
        }
    }
}
