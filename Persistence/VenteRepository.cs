using Dapper;
using Persistence.Contracts;
using Persistence.Contracts.Models;
using System.Data.SqlClient;

namespace Persistence
{
    public class VenteRepository : IVenteRepository
    {
        private readonly string _connectionString;
        public VenteRepository(string connectionString)
        {
                _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public async Task Truncate()
        {
            using (var db = new SqlConnection(_connectionString))
            {
                await db.ExecuteAsync("TRUNCATE TABLE Ventes");
            }
        }

        /// <summary>
        /// Better to save the ventes using sqlbulkcopy
        /// </summary>
        /// <param name="vente"></param>
        /// <returns></returns>
        public async Task SaveAsync(IEnumerable<Vente> ventes)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                await db.ExecuteAsync("INSERT INTO Ventes VALUES (@ClientId, @SoldDate, @ProductId, @SoldCount, @Price)", ventes);
            }
        }

        public async Task<IEnumerable<Vente>> GetAll()
        {
            using (var db = new SqlConnection(_connectionString))
            {
                return await db.QueryAsync<Vente>("SELECT * FROM Ventes");
            }
        }
    }
}
