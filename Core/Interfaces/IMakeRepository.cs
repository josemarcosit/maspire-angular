using angular_vega.Core.Models;

namespace angular_vega.Core
{
    public interface IMakeRepository
    {       
        Task<IEnumerable<Make>> GetAllMakesAsync();              
    }
}
