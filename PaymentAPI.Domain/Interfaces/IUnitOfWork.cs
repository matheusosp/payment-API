using System;
using System.Threading.Tasks;

namespace PaymentAPI.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        Task<int> CommitAsync();
    }
}
