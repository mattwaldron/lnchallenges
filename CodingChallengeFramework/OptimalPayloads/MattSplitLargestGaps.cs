using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodingChallengeFramework;

namespace OptimalPayloads
{
    public class MattSplitLargestGaps : IOptimalPayloads
    {
        private uint _payloadCost;
        private uint _elementCost;
        private uint _maxPayload;

        public static List<List<uint>> SplitLargestGap(List<uint> indices)
        {
            var gaps = new List<uint>();
            for (var i = 0; i < indices.Count - 1; i++)
            {
                gaps.Add(indices[i + 1] - indices[i]);
            }

            var maxGapIndices = gaps.Select((g, i) => (g, i)).Where(gi => gi.Item1 == gaps.Max()).Select(gi => gi.Item2).ToList();
            // If more than one max, find the one closest to the middle
            int maxGapIndex = maxGapIndices.Count > 1
                ? maxGapIndices.Select(mgi => (Math.Abs(mgi - indices.Count / 2), mgi))
                    .OrderBy(mgi => mgi.Item1)
                    .First()
                    .Item2
                : (int)maxGapIndices.First();
            return new List<List<uint>>() {
                indices.Take(maxGapIndex + 1).ToList(),
                indices.Skip(maxGapIndex + 1).ToList()};
        }

        public uint ComputeCostFromIndexList(List<List<uint>> indices)
        {
            var offsetsAndLengths = indices.Select(p => (p.Min(), p.Max() - p.Min())).ToList();
            return OptimalPayloadsChallenge.ComputeCost(offsetsAndLengths, _payloadCost, _elementCost);
        }

        public List<(uint index, uint length)> Run(uint maxPayload, uint payloadCost, uint elementCost, List<uint> indices)
        {
            _maxPayload = maxPayload;
            _payloadCost = payloadCost;
            _elementCost = elementCost;

            var payloadList = new List<List<uint>>() { indices.Distinct().OrderBy(x => x).ToList() };

            // Chop up into acceptable payloads
            while (payloadList.Any(p => p.Count > _maxPayload))
            {
                payloadList = payloadList.OrderByDescending(p => p.Count).ToList();
                var largestPayload = payloadList[0];
                payloadList.RemoveAt(0);
                payloadList.AddRange(SplitLargestGap(largestPayload));
            }

            while (true)
            {
                // find which one yields the best cost improvement
                var maxCostReductionIndex = -1;
                uint maxCostReduction = 0;
                List<List<uint>> newPayloads = null;
                for (var i = 0; i < payloadList.Count; i++)
                {
                    if (payloadList[i].Count == 1)
                    {
                        continue;
                    }

                    var thisSegment = new List<List<uint>> { payloadList[i] };
                    var thisSegmentCost = ComputeCostFromIndexList(thisSegment);
                    newPayloads = SplitLargestGap(thisSegment[0]);
                    var thisSplitSegmentCost = ComputeCostFromIndexList(newPayloads);
                    if (thisSegmentCost - thisSplitSegmentCost > maxCostReduction)
                    {
                        maxCostReduction = thisSegmentCost - thisSplitSegmentCost;
                        maxCostReductionIndex = i;
                    }
                }

                if (maxCostReductionIndex == -1)
                {
                    // no improvements possible!
                    break;
                }

                payloadList.RemoveAt(maxCostReductionIndex);
                payloadList.AddRange(newPayloads);
            }

            return payloadList.Select(p => (p.Min(), p.Max() - p.Min())).ToList();
        }
    }
}
