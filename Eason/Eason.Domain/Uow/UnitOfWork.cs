using System;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Eason.Domain.Uow
{
    public class UnitOfWork : IUnitOfWork
    {
        public DbContext Context { get; set; }

        public UnitOfWork()
        {


        }
        public int Commit()
        {
            return Context.SaveChanges();
        }

        public async Task<int> CommitAsync()
        {
            return await Context.SaveChangesAsync();
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