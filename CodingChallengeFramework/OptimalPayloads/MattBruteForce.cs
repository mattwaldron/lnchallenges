using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CodingChallengeFramework;

namespace OptimalPayloads
{
    // Consistently generate OutOfMemory exceptions... abort :( 
    public class MattBruteForce // : IOptimalPayloads
    {
        private uint _payloadCost;
        private uint _elementCost;
        private uint _maxPayload;

        /// <summary>
        /// Return all possible payload lists obtained by combining adjacent payloads in the start payload list.  The start
        /// payload list must be sorted.  New payload lists where the combination exceeds _maxPayload or the cost is greater
        /// than the startCost are ignored.
        /// </summary>
        List<(uint cost, List<(uint index, uint length)> payloadList)> CombineAdjacent(List<(uint index, uint length)> start, uint startCost)
        {
            var newPayloadLists = new List<(uint score, List<(uint index, uint length)> payloadList)>();
            for (var i = 0; i < start.Count - 1; i++)
            {
                var newSet = start.ToList();
                var a = newSet[i];
                var b = newSet[i + 1];
                var c = (a.index, b.length + b.index - a.index);
                if (c.Item2 > _maxPayload)
                {
                    continue;
                }
                newSet.RemoveAt(i+1);
                newSet[i] = c;
                var newCost = OptimalPayloadsChallenge.ComputeCost(newSet, _payloadCost, _elementCost);
                if (newCost > startCost)
                {
                    continue;
                }
                newPayloadLists.Add((newCost, newSet));
            }

            return newPayloadLists;
        }

        public List<(uint index, uint length)> Run(uint maxPayload, uint payloadCost, uint elementCost, List<uint> indices)
        {
            _maxPayload = maxPayload;
            _payloadCost = payloadCost;
            _elementCost = elementCost;

            var start = indices.Distinct().OrderBy(x => x).Select(index => (index, (uint)1)).ToList();

            var resultList = new List<(uint score, List<(uint index, uint length)> payloadList)>();
            var startCost = OptimalPayloadsChallenge.ComputeCost(start, _payloadCost, _elementCost);

            var thisIterationInput =
                new List<(uint cost, List<(uint index, uint length)> payloadList)> {(startCost, start)};

            while (thisIterationInput.Count > 0)
            {
                var thisIterationOutput = new List<(uint cost, List<(uint index, uint length)> payloadList)>();
                foreach (var input in thisIterationInput)
                {
                    thisIterationOutput.AddRange(CombineAdjacent(input.payloadList, input.cost));
                }

                if (thisIterationOutput.Count > 0)
                {
                    var minCost = thisIterationOutput.Min(t => t.cost);
                    thisIterationOutput = thisIterationOutput.Where(t => t.cost == minCost).ToList();
                }
                resultList.AddRange(thisIterationInput);
                thisIterationInput = thisIterationOutput;
            }

            return resultList.OrderByDescending(t => t.score).First().payloadList;
        }
    }
}
