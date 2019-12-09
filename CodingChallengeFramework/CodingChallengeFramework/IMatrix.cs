using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Linq;

namespace CodingChallengeFramework
{
    [InheritedExport(typeof(IMatrixSoulution))]
    public interface IMatrixSoulution
    {
        List<(int, int)> Run(int[,] matrix, int m, int n);
    }

    public class MatrixTraversalChallenge : Challenge
    {
        [ImportMany(typeof(IMatrixSoulution), AllowRecomposition = true)]
        protected IMatrixSoulution[] matrixAlgos = null;

        public override void Run(IEnumerable<string> args)
        {
            int nrows;
            int ncols;
            int[,] matrix;
            var argArray = args.ToArray();
            try
            {
                matrix = JsonConvert.DeserializeObject<int[,]>(string.Join("", argArray));
                nrows = matrix.GetLength(0);
                ncols = matrix.GetLength(1);
            }
            catch
            {
                if (args.Count() < 2 || (nrows = Convert.ToInt32(argArray[0])) <= 0 || (ncols = Convert.ToInt32(argArray[1])) <= 0)
                {
                    Console.WriteLine("Invalid arguments: must specify matrix with bounds greater than 0 or give a desired matrix");
                    return;
                }

                if (!(argArray.Length > 2 && int.TryParse(argArray[2], out var seed)))
                {
                    seed = Environment.TickCount;
                }
                var random = new Random(seed);
                Console.WriteLine($"Using seed {seed}");

                matrix = new int[nrows, ncols];
                for (var r = 0; r < nrows; r++)
                {
                    for (var c = 0; c < ncols; c++)
                    {
                        matrix[r, c] = random.Next(0, 20);
                        if (matrix[r, c] == 0)
                        {
                            matrix[r, c] = -1;
                        }
                        else
                        {
                            matrix[r, c] = (matrix[r, c] % 5) + 1;
                        }
                    }
                }
                matrix[0, 0] = 0;
                matrix[nrows - 1, ncols - 1] = 0;
            }

            Console.WriteLine($"Testing MatrixTraversal algorithms with the following matrix");
            for (var r = 0; r < nrows; r++)
            {
                for (var c = 0; c < ncols; c++)
                {
                    Console.Write($"{matrix[r, c], 3}");
                }
                Console.WriteLine();
            }

            Compose();
            var sw = new Stopwatch();
            foreach (var q in matrixAlgos)
            {
                List<(int, int)> result;
                try
                {
                    sw.Restart();
                    result = q.Run(matrix, nrows, ncols);
                    sw.Stop();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"{q.GetType().Name} (in {sw.ElapsedMilliseconds} ms) << !!! Threw exception with message: {ex.Message}");
                    continue;
                }

                var answer = "";
                var total = 0;
                if (result == null)
                {
                    answer = "There is no solution";
                }
                else
                {
                    foreach (var r in result)
                    {
                        answer += $"{r} ";
                        total += matrix[r.Item1, r.Item2];
                    }
                }
                Console.WriteLine($"{q.GetType().Name} (in {sw.ElapsedMilliseconds} ms) << {total} points \n\t\tPath: {answer}");
            }
        }
    }
}


