using PaymentAPI.Infra.EF.Context;
using System;

namespace PaymentAPI.Infra.EF.Repositories
{
    public abstract class GenericRepository : IDisposable
    {
        protected ApplicationDbContext Context;
        private bool _disposed;

        protected GenericRepository(ApplicationDbContext context)
        {
            Context = context;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                Context.Dispose();
            }

            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
