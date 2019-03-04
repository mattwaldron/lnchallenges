using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using CodingChallengeFramework;

namespace MazeSolver
{
    public class MattHG : IMazeSolver
    {
        private static int? movesToEnd;
        private char[][] m;
        private int[,] n;
        public int Run(string[] maze)
        {
            m = maze.Select(r => r.ToCharArray()).ToArray();
            n = new int [m.Length, m[0].Length];
            var x = 1;
            var y = 1;
            var nmoves = 0;
            
            Explore(x, y, nmoves);
            
            return movesToEnd ?? -1;
        }

        private void Explore(int x, int y, int nmoves)
        {
            if (m[y][x] == 'F')
            {
                if (movesToEnd == null || nmoves < movesToEnd)
                {
                    movesToEnd = nmoves;
                }
            }
            else
            {
                n[y, x] = nmoves;
                var walls = new char[] {'X', 'x', 'S', 's'};
                if (!walls.Contains(m[y - 1][x]) && n[y - 1, x] == 0)
                {
                    Explore(x, y - 1, nmoves + 1);
                }

                if (!walls.Contains(m[y][x + 1]) && n[y, x + 1] == 0)
                {
                    Explore(x + 1, y, nmoves + 1);
                }

                if (!walls.Contains(m[y + 1][x]) && n[y + 1, x] == 0)
                {
                    Explore(x, y + 1, nmoves + 1);
                }

                if (!walls.Contains(m[y][x - 1]) && n[y, x - 1] == 0)
                {
                    Explore(x - 1, y, nmoves + 1);
                }
            }

            for (var i = 0; i < m[0].Length; i++)
            {
                for (var j = 0; j < m.Length; j++)
                {
                    if (n[j, i] > nmoves)
                    {
                        n[j, i] = 0;
                    }
                }
            }
        }
    }
}
