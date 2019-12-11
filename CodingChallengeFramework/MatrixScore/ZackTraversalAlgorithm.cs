using System.Collections.Generic;
using CodingChallengeFramework;

namespace MatrixTraversal
{
    class ZackTraversalAlgorithm : IMatrixSoulution
    {
        public List<(int, int)> Run(int[,] matrix, int m, int n)
        {
            // create a matrix to store the best path to each given point
            var matrixResult = new Dictionary<int, Dictionary<int, (int, List<(int, int)>)>>();
            for (var i = 0; i < m; i++)
            {
                for (var j = 0; j < n; j++)
                {
                    if (!matrixResult.ContainsKey(i))
                    {
                        matrixResult.Add(i, new Dictionary<int, (int, List<(int, int)>)>());
                    }
                    matrixResult[i].Add(j, (0, new List<(int, int)> { (0, 0) }));
                }
            }

            for (var i = 1; i < m; i++)
            {
                if (matrix[i, 0] == -1 || matrixResult[i - 1][0].Item2 == null)
                {
                    matrixResult[i][0] = (-1, null);
                }
                else
                {
                    matrixResult[i][0] = (matrixResult[i - 1][0].Item1 + matrix[i, 0], new List<(int, int)>(matrixResult[i - 1][0].Item2));
                    matrixResult[i][0].Item2.Add((i, 0));
                }
            }

            for (var i = 1; i < n; i++)
            {
                if (matrix[0, i] == -1 || matrixResult[0][i - 1].Item2 == null)
                {
                    matrixResult[0][i] = (-1, null);
                }
                else
                {
                    matrixResult[0][i] = (matrixResult[0][i - 1].Item1 + matrix[0, i], new List<(int, int)>(matrixResult[0][i - 1].Item2));
                    matrixResult[0][i].Item2.Add((0, i));
                }
            }

            // loop through all positions in the matrix and update result at each position
            for (var i = 1; i < m; i++)
            {
                for (var j = 1; j < n; j++)
                {
                    if (matrix[i, j] == -1)
                    {
                        matrixResult[i][j] = (-1, null);
                    }
                    else // can get to this space
                    {
                        // choose path that gave most points
                        matrixResult[i][j] = matrixResult[i - 1][j].Item1 > matrixResult[i][j - 1].Item1
                            ? (matrixResult[i - 1][j].Item1 + matrix[i, j], matrixResult[i - 1][j].Item2)
                            : (matrixResult[i][j - 1].Item1 + matrix[i, j], matrixResult[i][j - 1].Item2);

                        if (matrixResult[i][j].Item2 != null)
                        {
                            matrixResult[i][j] = (matrixResult[i][j].Item1, new List<(int, int)>(matrixResult[i][j].Item2));
                            matrixResult[i][j].Item2.Add((i, j));
                        }
                        else
                        {
                            matrixResult[i][j] = (-1, null);
                        }
                    }
                }
            }

            // return the resultant path to the last square
            return matrixResult[m - 1][n - 1].Item2;
        }
    }
}
