
namespace Persistence.Contracts.Models
{
    public class Vente
    {
        public int ClientId { get; set; }

        public DateTime SoldDate { get; set; }

        public int ProductId { get; set; }

        public short SoldCount { get; set; }

        public decimal Price { get; set; }
    }
}
