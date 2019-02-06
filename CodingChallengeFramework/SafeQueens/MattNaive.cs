using CodingChallengeFramework;

namespace QueensCodingChallenge
{
    public class MattNaive : ISafeQueens
    {
        public int Run(int n)
        {
            var board = new bool[n, n];
            var nqueens = 0;

            void Place(int x, int y)
            {
                nqueens++;
                board[x, y] = true;
                for (var j = 0; j < n; j++)
                {
                    board[x, j] = true;
                }
                for (var i = 0; i < n; i++)
                {
                    board[i, y] = true;
                }

                for (int i = x, j = y; i < n && j < n && i >= 0 && j >= 0; i--, j--)
                {
                    board[i, j] = true;
                }
                for (int i = x, j = y; i < n && j < n && i >= 0 && j >= 0; i++, j--)
                {
                    board[i, j] = true;
                }
                for (int i = x, j = y; i < n && j < n && i >= 0 && j >= 0; i--, j++)
                {
                    board[i, j] = true;
                }
                for (int i = x, j = y; i < n && j < n && i >= 0 && j >= 0; i++, j++)
                {
                    board[i, j] = true;
                }
            }

            for (var i = 0; i < n; i++)
            {
                for (var j = 0; j < n; j++)
                {
                    if (!board[i, j])
                    {
                        Place(i, j);
                    }
                }
            }

            return nqueens;
        }
    }
}