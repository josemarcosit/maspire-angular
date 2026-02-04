using angular_vega.Persistence;

namespace angular_vega.Core
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly VegaDbContext vegaDbContext;

        public UnitOfWork(VegaDbContext vegaDbContext)
        {
            this.vegaDbContext = vegaDbContext;
        }
        public async Task CompleteAsync()
        {
            await vegaDbContext.SaveChangesAsync();
        }
    }
    public interface IUnitOfWork
    {
        Task CompleteAsync();
    }
}