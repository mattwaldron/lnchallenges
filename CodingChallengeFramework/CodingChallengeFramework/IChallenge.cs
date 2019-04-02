using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CodingChallengeFramework
{
    public class Challenge
    {
        public virtual void Run(IEnumerable<string> args)
        {
        }

        public void Compose()
        {
            var catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new AssemblyCatalog(Assembly.GetExecutingAssembly()));
            if (Directory.Exists("solutions"))
            {
                foreach (var f in Directory.EnumerateFiles("solutions"))
                {
                    try
                    {
                        catalog.Catalogs.Add(new AssemblyCatalog(Assembly.LoadFile(Path.GetFullPath(f))));
                    }
                    catch
                    {
                    }
                }
            }

            foreach (var f in Directory.EnumerateFiles("."))
            {
                try
                {
                    catalog.Catalogs.Add(new AssemblyCatalog(Assembly.LoadFile(Path.GetFullPath(f))));
                }
                catch
                {
                }
            }

            var container = new CompositionContainer(catalog);
            container.ComposeParts(this);
        }
    }

    public class DefaultChallenge : Challenge
    {
        public override void Run(IEnumerable<string> args)
        {
            Console.WriteLine("No challenge selected");
        }
    }
}
