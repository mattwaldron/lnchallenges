using System.Collections.Generic;
using SafeQueensChallenge;

namespace QueensCodingChallenge
{
    public class MattMiddleOut : ISafeQueens
    {
        public static List<(int x, int y)> SpiralStack(int n)
        {
            var stack = new List<(int x, int y)>();
            if (n == 0)
            {
                return stack;
            }
            var edges = new Dictionary<char, int>() {{'r', n-1},
                {'d', n-1},
                {'l', 0},
                {'u', 0}};
            var dir = 'r';
            var pt = (x: 0, y: 0);

            do
            {
                stack.Add(pt);
                switch (dir)
                {
                    case 'r':
                        if (pt.x == edges['r'])
                        {
                            dir = 'd';
                            edges['u'] = pt.y + 1;
                        }
                        break;
                    case 'd':
                        if (pt.y == edges['d'])
                        {
                            dir = 'l';
                            edges['r'] = pt.x - 1;
                        }
                        break;
                    case 'l':
                        if (pt.x == edges['l'])
                        {
                            dir = 'u';
                            edges['d'] = pt.y - 1;
                        }
                        break;
                    case 'u':
                        if (pt.y == edges['u'])
                        {
                            dir = 'r';
                            edges['l'] = pt.x + 1;
                        }
                        break;
                }

                switch (dir)
                {
                    case 'r':
                        pt = (pt.x + 1, pt.y);
                        break;
                    case 'd':
                        pt = (pt.x, pt.y + 1);
                        break;
                    case 'l':
                        pt = (pt.x - 1, pt.y);
                        break;
                    case 'u':
                        pt = (pt.x, pt.y - 1);
                        break;
                }

            } while (stack.Count < (n * n));

            return stack;
        }

        public int Run(int n)
        {
            int nqueens = 0;
            var ss = SpiralStack(n);
            ss.Reverse();
            while (ss.Count > 0)
            {
                var pt = ss[0];
                ss.RemoveAt(0);
                nqueens++;
                for (var j = 0; j < n; j++)
                {
                    ss.Remove((pt.x, j));
                }
                for (var i = 0; i < n; i++)
                {
                    ss.Remove((i, pt.y));
                }

                for (int i = pt.x, j = pt.y; i < n && j < n && i >= 0 && j >= 0; i--, j--)
                {
                    ss.Remove((i, j));
                }
                for (int i = pt.x, j = pt.y; i < n && j < n && i >= 0 && j >= 0; i++, j--)
                {
                    ss.Remove((i, j));
                }
                for (int i = pt.x, j = pt.y; i < n && j < n && i >= 0 && j >= 0; i--, j++)
                {
                    ss.Remove((i, j));
                }
                for (int i = pt.x, j = pt.y; i < n && j < n && i >= 0 && j >= 0; i++, j++)
                {
                    ss.Remove((i, j));
                }
            }

            return nqueens;
        }
    }
}