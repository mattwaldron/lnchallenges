using System.Collections.Generic;
using System.Linq;
using CodingChallengeFramework;

namespace OptimalPayloads
{
    public class NaiveSingleElementPayloads : IOptimalPayloads
    {
        public List<(uint index, uint length)> Run(uint maxPayload, uint payloadCost, uint elementCost, List<uint> indices)
        {
            return indices.Select(x => (x, (uint)1)).ToList();
        }
    }
}
