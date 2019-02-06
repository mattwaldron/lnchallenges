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

    public class MazeSolverChallenge : IChallenge

    {
        [ImportMany(typeof(IMazeSolver), AllowRecomposition = true)]
        protected IMazeSolver[] mazeSolvers = null;

        public void Run(IEnumerable<string> args)
        {
            var argArray = args.ToArray();
            
            Compose();

            var maze = MazeGenerator.GetMaze(Convert.ToInt32(argArray[1]), Convert.ToInt32(argArray[2]));
            
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

        public void Compose()
        {
            var catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new AssemblyCatalog(Assembly.GetExecutingAssembly()));
            try
            {
                foreach (var f in Directory.EnumerateFiles("solutions"))
                {
                    catalog.Catalogs.Add(new AssemblyCatalog(Assembly.LoadFile(Path.GetFullPath(f))));
                }
            }
            catch {}

            var container = new CompositionContainer(catalog);
            container.ComposeParts(this);
        }
    }
}
