using Eason.Domain.Entities;
using System;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Eason.Domain.Uow
{
    public interface IUnitOfWork : IDisposable
    {
         DbContext Context { get; set; }
        int Commit();
        Task<int> CommitAsync();
    }
}