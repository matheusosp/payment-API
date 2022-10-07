using System;
using System.Threading.Tasks;

namespace PaymentAPI.Domain.Contracts
{
    public interface IUnitOfWork : IDisposable
    {
        Task<int> CommitAsync();
    }
}
