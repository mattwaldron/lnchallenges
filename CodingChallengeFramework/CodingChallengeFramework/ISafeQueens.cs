using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace CodingChallengeFramework
{
    [InheritedExport(typeof(ISafeQueens))]
    public interface ISafeQueens
    {
        int Run(int n);
    }

    public class SafeQueensChallenge : IChallenge

    {
        [ImportMany(typeof(ISafeQueens), AllowRecomposition = true)]
        protected ISafeQueens [] SafeQueensImpls = null;

        public void Run(IEnumerable<string> args)
        {
            var n = Convert.ToInt32(args.First());
            Compose();
            var sw = new Stopwatch();
            foreach (var q in SafeQueensImpls)
            {
                string answer = "";
                sw.Restart();
                try
                {
                    var result = q.Run(n);
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
            foreach (var f in Directory.EnumerateFiles("solutions"))
            {
                catalog.Catalogs.Add(new AssemblyCatalog(Assembly.LoadFile(Path.GetFullPath(f))));
            }
            var container = new CompositionContainer(catalog);
            container.ComposeParts(this);
        }
    }
}
