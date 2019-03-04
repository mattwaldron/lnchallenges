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
    [InheritedExport(typeof(ISoupServings))]
    public interface ISoupServings
    {
        double Run(int volume);
    }

    public class SoupServingsChallenge : IChallenge

    {
        [ImportMany(typeof(ISoupServings), AllowRecomposition = true)]
        protected ISoupServings[] soupServings = null;

        public void Run(IEnumerable<string> args)
        {
            var n = Convert.ToInt32(args.First());
            Compose();
            var sw = new Stopwatch();
            foreach (var q in soupServings)
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
            try
            {
                foreach (var f in Directory.EnumerateFiles("solutions"))
                {
                    catalog.Catalogs.Add(new AssemblyCatalog(Assembly.LoadFile(Path.GetFullPath(f))));
                }
            }
            catch { }

            var container = new CompositionContainer(catalog);
            container.ComposeParts(this);
        }
    }
}
