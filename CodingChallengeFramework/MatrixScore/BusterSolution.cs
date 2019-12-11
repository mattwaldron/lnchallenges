using System;
using System.Collections.Generic;
using CodingChallengeFramework;

namespace MatrixTraversal
{
    internal class BusterSolution : IMatrixSoulution
    {
        int TotalValue = 0;
        List<(int, int)> Solution = new List<(int, int)>();
        int MatrixRows;
        int MatrixColumns;
        int[,] TestMatrix;

        public List<(int, int)> Run(int[,] matrix, int rows, int columns)
        {
            TestMatrix = matrix;
            MatrixRows = rows - 1;
            MatrixColumns = columns - 1;
            List<(int, int)> travel = new List<(int, int)>() { new ValueTuple<int, int>(0, 0) };
            try
            {
                Move(travel, 0, 0, 0);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed: {ex.Message}");
                Console.ReadLine();
            }
            return (Solution.Count > 0 ? Solution : null);
        }

        private void Move(List<(int, int)> travel, int x, int y, int value)
        {
            if (x < MatrixRows)
            {
                MoveDown(travel, x, y, value);
            }
            if (y < MatrixColumns)
            {
                MoveRight(travel, x, y, value);
            }
        }

        private void MoveDown(List<(int, int)> travel, int x, int y, int value)
        {
            // Only process legal moves
            if (TestMatrix[++x, y] >= 0)
            {
                List<(int, int)> newTravel = new List<(int, int)>(travel) { new ValueTuple<int, int>(x, y) };
                value += TestMatrix[x, y];
                if (x == MatrixRows && y == MatrixColumns)
                {
                    if (value > TotalValue)
                    {
                        TotalValue = value;
                        Solution = newTravel;
                    }
                }
                else
                {
                    Move(newTravel, x, y, value);
                }
            }
        }

        private void MoveRight(List<(int, int)> travel, int x, int y, int value)
        {
            // Only process legal moves
            if (TestMatrix[x, ++y] >= 0)
            {
                List<(int, int)> newTravel = new List<(int, int)>(travel) { new ValueTuple<int, int>(x, y) };
                value += TestMatrix[x, y];
                if (y == MatrixColumns && x == MatrixRows)
                {
                    if (value > TotalValue)
                    {
                        TotalValue = value;
                        Solution = newTravel;
                    }
                }
                else
                {
                    Move(newTravel, x, y, value);
                }
            }
        }
    }
}
