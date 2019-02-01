using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using SafeQueensChallenge;

// Challenge:
// Implement the Run function below, which takes an integer and returns an integer.
// The value of n passed to the function is the number of squares on the edge of a
// chessboard (8 for a typical board, but it can be anything greater than 0).  The
// function should return the maximum number of queens that can be placed on the
// board such that no queen can immediately capture another.  There is no distinction
// of color: any queen can capture any other.  Queens can move any number of spaces
// horizontally, vertically, and diagonally.
//
// For example, with n == 2, the function should return 1, since a queen at position
// (0,0) can capture queens at position (0,1), (1,0), and (1,1) - all the other spaces
// on the board.

namespace QueensCodingChallenge
{
    public class MattSudokuBruteForce : ISafeQueens
    {
        class BoardState
        {
            public List<int> cols;
            public List<List<int>> boardExcludes;

            public List<BoardState> Cascade(int gen)
            {
                var boardEdge = boardExcludes.Count;

                var futureBoards = new List<BoardState>();
                if (gen >= boardExcludes.Count || boardExcludes[gen].Count == boardEdge || cols.Count == 0)
                {
                    return futureBoards;
                }

                foreach (var c in cols)
                {
                    if (boardExcludes[gen].Contains(c))
                    {
                        continue;
                    }
                    var next = BoardState.DeepCopy(this);
                    next.cols.Remove(c);
                    for (var i = gen; i < boardEdge; i++)
                    {
                        if (!next.boardExcludes[i].Contains(i))
                        {
                            next.boardExcludes[i].Add(c);
                        }
                    }
                    for (var i = 1; gen+i < boardEdge && c+i < boardEdge; i++)
                    {
                        if (!next.boardExcludes[gen+i].Contains(c+i))
                        {
                            next.boardExcludes[gen+i].Add(c+i);
                        }
                    }
                    for (var i = 1; gen+i < boardEdge && c - i >= 0; i++)
                    {
                        if (!next.boardExcludes[gen+i].Contains(c-i))
                        {
                            next.boardExcludes[gen+i].Add(c-i);
                        }
                    }
                    futureBoards.Add(next);
                }

                return futureBoards;
            }

            public static BoardState DeepCopy(BoardState b)
            {
                var r = new BoardState();
                if (b.cols != null)
                {
                    r.cols = new List<int>(b.cols);
                }

                if (b.boardExcludes != null)
                {
                    r.boardExcludes = new List<List<int>>();
                    foreach (var row in b.boardExcludes)
                    {
                        r.boardExcludes.Add(new List<int>(row));
                    }
                }

                return r;
            }
        }
        public int Run(int n)
        {
            var cols = Enumerable.Range(0, n).ToList();
            var board = new List<List<int>>();
            for (int i = 0; i < n; i++)
            {
                board.Add(new List<int>());
            }

            var boardFutures = new List<BoardState>();
            boardFutures.Add(new BoardState()
            {
                cols = cols,
                boardExcludes = board
            });

            var gen = 0;
            while (boardFutures.Count != 0)
            {
                var currentGen = boardFutures;
                boardFutures = new List<BoardState>();
                foreach (var b in currentGen)
                {
                    boardFutures.AddRange(b.Cascade(gen));
                }
                gen++;
            }

            return gen-1;
        }
    }
}
