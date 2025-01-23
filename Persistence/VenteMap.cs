using Dapper.FluentMap.Mapping;
using Persistence.Contracts.Models;

namespace Persistence
{
    public class VenteMap : EntityMap<Vente>
    {
        public VenteMap()
        {
            Map(v => v.ClientId).ToColumn("CLI_ID");
            Map(v => v.SoldDate).ToColumn("VNT_DATE");
            Map(v => v.ProductId).ToColumn("PRD_ID");
            Map(v => v.SoldCount).ToColumn("VNT_COUNT");
            Map(v => v.Price).ToColumn("VNT_PRICE");
        }
    }
}
