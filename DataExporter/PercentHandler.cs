

using DataExporter.Models;
using Persistence.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataExporter
{
    public class PercentHandler
    {
        class Seed
        {
            public int Count { get; set; }
            public decimal Sum { get; set; }
        }

        private readonly decimal _level;

        public PercentHandler(decimal level)
        {
            _level = level;
        }

        public IEnumerable<PercentData> Calcualte(IEnumerable<Vente> ventes)
        {
            var sumCount = ventes.Aggregate(0m, (s, v) => s + v.SoldCount * v.Price );

            var aggregatedInfo = new List<PercentData>();
            foreach(var vente in ventes.GroupBy(x => x.ProductId))
            {
                var sum = vente.Aggregate(0m, (s, v) => s + v.SoldCount * v.Price);
                aggregatedInfo.Add(new PercentData() { Sum = sum, ProductId = vente.Key, Percent = Math.Round(sum * 100 / sumCount, 2) });
            }

            var percentSum = 0m;
            foreach(var product in aggregatedInfo.OrderByDescending(p => p.Sum))
            {
                percentSum += product.Percent;

                if (percentSum > _level)
                {
                    //var delta = _level - percentSum;
                    //var sum = product.
                    //yield return new PercentData { ProductId = product.ProductId, Sum =  };
                    yield break;
                }

                yield return product;
            }
        }
    }
}
