using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.IO.Pipes;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CodingChallengeFramework
{
    [InheritedExport(typeof(IHighestScorePath))]
    public interface IHighestScorePath
    {
        int Run(int [,] grid);
    }

    public class HighestScorePathChallenge : Challenge
    {
        [ImportMany(typeof(IHighestScorePath), AllowRecomposition = true)]
        protected IHighestScorePath[] scoredPaths = null;

        public override void Run(IEnumerable<string> args)
        {
            var argArray = args.ToArray();
            int nrows;
            int ncols;
            int[,] grid;
            try
            {
                grid = JsonConvert.DeserializeObject<int[,]>(string.Join("", argArray));
                nrows = grid.GetLength(0);
                ncols = grid.GetLength(1);
            }
            catch
            {
                nrows = int.Parse(argArray[0]);
                ncols = int.Parse(argArray[1]);
                Random rand;
                if (argArray.Length > 2 && int.TryParse(argArray[2], out var seed))
                {
                    rand = new Random(seed);
                }
                else
                {
                    seed = Environment.TickCount;
                    Console.WriteLine($"Using seed {seed}");
                    rand = new Random(seed);
                }
                grid = new int[nrows, ncols];
                for (var r = 0; r < nrows; r++)
                {
                    for (var c = 0; c < ncols; c++)
                    {
                        grid[r, c] = rand.Next(20) - 1;
                        if (grid[r, c] > 5)
                        {
                            grid[r, c] %= 6;
                        }
                    }
                }

                grid[0, 0] = 0;
                grid[nrows - 1, ncols - 1] = 0;
            }

            Console.WriteLine($"Testing HighestScorePath algorithms with grid:");
            for (var r = 0; r < nrows; r++)
            {
                for (var c = 0; c < ncols; c++)
                {
                    Console.Write($"{grid[r,c],3}");
                }
                Console.WriteLine("");
            }

            Compose();
            var sw = new Stopwatch();
            foreach (var q in scoredPaths)
            {
                var answer = "";
                try
                {
                    sw.Restart();
                    var result = q.Run(grid);
                    answer = $"{result}";
                }
                catch (Exception ex)
                {
                    answer = $" !!! Threw exception with message: {ex.Message}";
                }
                Console.WriteLine($"{q.GetType().Name} (in {sw.ElapsedMilliseconds} ms) << {answer}");
            }
        }
    }
}
