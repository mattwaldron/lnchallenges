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
    public class Node
    {
        public (int, int) pos;
        public int total;
        public Node parent;
    }

    public class MattBinTreeSearch : IMatrixSoulution
    {
        public static List<Node> Advance(Node n, int[,] matrix)
        {
            var pos = n.pos;
            var steps = new List<Node>();
            if ((pos.Item1 + 1) < matrix.GetLength(0)
                && matrix[pos.Item1 + 1, pos.Item2] != -1)
            {
                steps.Add(new Node()
                {
                    parent = n,
                    pos = (pos.Item1 + 1, pos.Item2),
                    total = n.total + matrix[pos.Item1 + 1, pos.Item2]
                });
            }

            if ((pos.Item2 + 1) < matrix.GetLength(1)
                && matrix[pos.Item1, pos.Item2 + 1] != -1)
            {
                steps.Add(new Node()
                {
                    parent = n,
                    pos = (pos.Item1, pos.Item2 + 1),
                    total = n.total + matrix[pos.Item1, pos.Item2 + 1]
                });
            }

            return steps;
        }

        public static List<(int, int)> GetPath(Node n)
        {
            var path = new List<(int, int)>();
            while (n != null)
            {
                path.Add(n.pos);
                n = n.parent;
            }

            path.Reverse();
            return path;
        }

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
                var newLeaves = new List<Node>();
                foreach (var f in leaves)
                {
                    newLeaves.AddRange(Advance(f, matrix));
                }

                var prunedLeaves = new List<Node>();
                foreach (var g in newLeaves.GroupBy(f => f.pos))
                {
                    prunedLeaves.Add(g.OrderByDescending(f => f.total).First());
                }

                var bestLeaf = prunedLeaves.OrderByDescending(n => n.total).First();
                if (bestLeaf.pos == (m - 1, n - 1))
                {
                    return GetPath(bestLeaf);
                }

                leaves = prunedLeaves;
            }
            
            return null;
        }
    }
}
