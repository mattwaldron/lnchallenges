using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace CodingChallengeFramework
{
    public class MazeGenerator
    {
        public static string[] GetMaze(int x, int y)
        {
            var m = Maze(x, y);
            var rows = new List<string>();
            var b = $"X";
            rows.Add(Enumerable.Repeat(b, (2 * m.GetLength(0)) + 1).Aggregate("", string.Concat));
            for (var j = 0; j < m.GetLength(1); j++)
            {
                var thisRow = b;
                var nextRow = b;
                for (var i = 0; i < m.GetLength(0); i++)
                {
                    thisRow += " ";

                    thisRow += (i == m.GetLength(0) - 1)
                        ? b
                        : (m[i, j].connections.Contains(m[i + 1, j]))
                            ? " "
                            : b;
                    nextRow += (j == m.GetLength(1) - 1)
                        ? b
                        : (m[i, j].connections.Contains(m[i, j + 1]))
                            ? " "
                            : b;
                    nextRow += b;
                }
                rows.Add(thisRow);
                rows.Add(nextRow);
            }

            var ca = rows[1].ToCharArray();
            ca[1] = 'S';
            rows[1] = string.Join("", ca);
            ca = rows[rows.Count - 2].ToCharArray();
            ca[ca.Length - 2] = 'F';
            rows[rows.Count - 2] = string.Join("", ca);

            return rows.ToArray();
        }
        
        class Node
        {
            public bool visited;
            public readonly int x;
            public readonly int y;
            public readonly List<Node> connections;
            public Node(int i, int j)
            {
                visited = false;
                connections = new List<Node>();
                x = i;
                y = j;
            }
        }

        static (int x, int y) Dir(int c)
        {
            int x, y;
            c = c % 4;
            if (c % 2 == 0)
            {
                x = 0;
                y = 1;
            }
            else
            {
                x = 1;
                y = 0;
            }
            if (c == 1 || c == 2)
            {
                x *= -1;
                y += -1;
            }
            return (x, y);
        }

        static bool InBoundaries(Node[,] z, int x, int y)
        {
            return (x >= 0 && y >= 0 && x < z.GetLength(0) && y < z.GetLength(1));
        }

        static Node[,] Maze(int width, int height)
        {
            var z = new Node[width, height];
            for (var i = 0; i < width; i++)
            {
                for (var j = 0; j < height; j++)
                {
                    z[i, j] = new Node(i, j);
                }
            }
            var r = new Random();
            var (x, y) = (r.Next(width - 1), r.Next(height - 1));
            var nodes = new List<Node> { z[x, y] };
            z[x, y].visited = true;
            while (nodes.Count > 0)
            {
                var count = (r.Next(100));
                try
                {
                    for (var dc = 0; dc < 4; dc++)
                    {
                        var (dx, dy) = Dir(count + dc);
                        if (InBoundaries(z, x + dx, y + dy) && !z[x + dx, y + dy].visited)
                        {
                            z[x, y].connections.Add(z[x + dx, y + dy]);
                            z[x + dx, y + dy].connections.Add(z[x, y]);
                            x += dx;
                            y += dy;
                            nodes.Add(z[x, y]);
                            z[x, y].visited = true;
                            throw new Exception("found new node");
                        }
                    }
                    nodes.RemoveAt(nodes.Count - 1);
                    x = nodes[nodes.Count - 1].x;
                    y = nodes[nodes.Count - 1].y;
                }
                catch { }
            }
            return z;
        }
    }
}
