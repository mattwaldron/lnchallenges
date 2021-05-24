using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using CodingChallengeFramework;

namespace MatrixScore
{
    public class MattBinTreeOneline /*: IMatrixSoulution*/
    {
        public List<(int, int)> Run(int[,] matrix, int m, int n)
        {
            var leaves = new List<Node>() {
                new Node()
                {
                    parent = null,
                    pos = (0, 0),
                    total = 0
                }
            };

            while (leaves.Count != 0)
            {
                leaves = leaves.SelectMany(f => MattBinTreeSearch.Advance(f, matrix))
                               .GroupBy(f => f.pos)
                               .Select(g => g.OrderByDescending(f => f.total)
                                             .First())
                               .OrderByDescending(nn => nn.total)
                               .ToList();
                var bestLeaf = leaves.First();
                if (bestLeaf.pos == (m-1, n-1)) {
                    return MattBinTreeSearch.GetPath(bestLeaf);
                }
            }
            
            return null;
        }
    }
}
