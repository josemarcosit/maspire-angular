using maspire_angular.Infrastructure.Persistence;

namespace maspire_angular.Shared.Abstractions
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MaspireDbContext maspireDbContext;
        public UnitOfWork(MaspireDbContext maspireDbContext)
        {
            this.maspireDbContext = maspireDbContext;
        }
        public async Task CompleteAsync()
        {
            await maspireDbContext.SaveChangesAsync();
        }
    }
    public interface IUnitOfWork
    {
        Task CompleteAsync();
    }
}