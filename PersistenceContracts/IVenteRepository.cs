using Persistence.Contracts.Models;

namespace Persistence.Contracts
{
    public interface IVenteRepository
    {
        Task Truncate();

        Task SaveAsync(IEnumerable<Vente> vente);

        Task<IEnumerable<Vente>> GetAll();
    }
}
