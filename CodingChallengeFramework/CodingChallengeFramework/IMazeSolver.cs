using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CodingChallengeFramework
{
    [InheritedExport(typeof(IMazeSolver))]
    public interface IMazeSolver
    {
        int Run(string [] maze);
    }

    public class MazeSolverChallenge : Challenge

    {
        [ImportMany(typeof(IMazeSolver), AllowRecomposition = true)]
        protected IMazeSolver[] mazeSolvers = null;

        public override void Run(IEnumerable<string> args)
        {
            var argArray = args.ToList();
            if (argArray.Count < 1)
            {
                argArray.Add("2");
            }
            if (argArray.Count < 2)
            {
                argArray.Add(argArray[0]);
            }

            Compose();

            var maze = MazeGenerator.GetMaze(Convert.ToInt32(argArray[0]), Convert.ToInt32(argArray[1]));
            Console.WriteLine("Testing against the following maze:");
            foreach (var row in maze)
            {
                Console.WriteLine(row);
            }
            Console.WriteLine("");

            var sw = new Stopwatch();
            foreach (var q in mazeSolvers)
            {
                string answer = "";
                sw.Restart();
                try
                {
                    var result = q.Run(maze);
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
