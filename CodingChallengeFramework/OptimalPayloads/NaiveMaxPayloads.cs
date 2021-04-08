using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodingChallengeFramework;

namespace OptimalPayloads
{
    public class NaiveMaxPayloads : IOptimalPayloads
    {
        public List<(uint index, uint length)> Run(uint maxPayload, uint payloadCost, uint elementCost, List<uint> indices)
        {
            var indicesOrdered = indices.OrderBy(x => x).ToList();
            var payloads = new List<(uint index, uint length)>();
            while (indicesOrdered.Count > 0)
            {
                payloads.Add((indicesOrdered.First(), maxPayload));
                var pmax = indicesOrdered.First() + maxPayload;
                indicesOrdered.RemoveAll(x => x < pmax);
            }

            return payloads;
        }
    }
}
