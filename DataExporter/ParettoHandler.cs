

using DataExporter.Models;
using Persistence.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataExporter
{
    public class ParettoHandler
    {
        class Seed
        {
            public int Count { get; set; }
            public decimal Sum { get; set; }
        }

        public IEnumerable<ParettoData> Calcualte(IEnumerable<Vente> ventes)
        {
            var seed = new Seed() { Count = 0, Sum = 0 };

            var sumCount = ventes.Aggregate(seed, (s, v) =>
            {
                seed.Count += v.SoldCount;
                seed.Sum += v.SoldCount * v.Price;
                return seed;
            });

            var stepCount = 20;
            var step = (int)(Math.Floor((double)(seed.Count) / stepCount));

            int beforeCount = 0, count = 0;
            decimal beforeSum = 0m, sum = 0m;

            foreach(var vente in ventes.OrderBy(x => x.SoldDate))
            {
                count += beforeCount + vente.SoldCount;
                sum += beforeSum + vente.SoldCount * vente.Price;

                if (count >= step)
                {
                    beforeCount = count - step;
                    beforeSum = beforeCount * vente.Price;

                    sum -= beforeSum;
                    count -= beforeCount;

                    yield return new ParettoData { SumPercent = Math.Round(sum * 100 / seed.Sum, 2), CountPercent = Math.Round((decimal)count * 100 / seed.Count, 2), Count = count };

                    count = beforeCount;
                    sum = beforeSum;
                    count = 0;
                }
            }

            yield return new ParettoData { SumPercent = Math.Round(sum * 100 / seed.Sum, 2), CountPercent = Math.Round((decimal)count * 100 / seed.Count, 2), Count = count };
        }
    }
}
