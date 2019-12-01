using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodingChallengeFramework;

namespace HighestScorePath
{
    // change the implementation of this class and give it a unique name!
    public class DownUntilWall : IHighestScorePath
    {
        // the row is the first index
        (int r, int c) Advance(int[,] grid, (int r, int c) pos)
        {
            if (pos.r + 1 >= grid.GetLength(0) || grid[pos.r + 1, pos.c] == -1)
            {
                // go right
                if (pos.c + 1 >= grid.GetLength(1) || grid[pos.r, pos.c + 1] == -1)
                {
                    return pos;
                }
                return (pos.r, pos.c + 1);
            }

            return (pos.r + 1, pos.c);
        }

        public int Run(int[,] grid)
        {
            int sum = 0;

            (int r, int c) pos = (0, 0);
            var end = (grid.GetLength(0) - 1, grid.GetLength(1) - 1);
            while (pos != end)
            {
                var next = Advance(grid, pos);
                if (next == pos)
                {
                    return -1;
                }
                pos = next;
                sum += grid[pos.r, pos.c];
            }

            return sum;
        }
    }
}
