using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Threading.Tasks;
using CodingChallengeFramework;


namespace SoupServings
{
    
    public class MattDynaFullCache /*: ISoupServings*/
    {
        private Ratio [] menu =
        {
            new Ratio(4, 0),
            new Ratio(3, 1),
            new Ratio(2, 2),
            new Ratio(1, 3)
        };
        private Dictionary<(int,int), (double, double)> cachedStats;
        public MattDynaFullCache()
        {
            cachedStats = new Dictionary<(int, int), (double, double)>();
        }
        (double aFirst, double ab) CalculateProb(int a, int b)
        {
            if (cachedStats.ContainsKey((a,b)))
            {
                return cachedStats[(a,b)];
            }
            double probAEmptyFirst = 0;
            double probABEmptyTogether = 0;
            var pots = new List<(Ratio, int)>()
            {
                (new Ratio(a, b), 1)
            };
            while (pots.Count != 0)
            {
                var (thisRatio, gen) = pots[0];
                pots.RemoveAt(0);
                foreach (var m in menu)
                {
                    var newRatio = thisRatio.Copy();
                    var ev = newRatio.Serve(m);
                    switch (ev)
                    {
                        case ServeEvent.ABEmpty:
                            probABEmptyTogether += (1.0 / (Math.Pow(menu.Length, gen)));
                            break;
                        case ServeEvent.AEmpty:
                            probAEmptyFirst += (1.0 / (Math.Pow(menu.Length, gen)));
                            break;
                        case ServeEvent.Served:
                            var (aFirst, ab) = CalculateProb(newRatio.a, newRatio.b);
                            probAEmptyFirst += aFirst / (Math.Pow(menu.Length, gen));
                            probABEmptyTogether += ab / (Math.Pow(menu.Length, gen));
                            break;
                        default:
                            if (newRatio.Status == PotStatus.BothAvailable)
                            {
                                pots.Add((newRatio, gen + 1));
                            }
                            break;
                    }
                }
            }
            cachedStats[(a,b)] = (probAEmptyFirst, probABEmptyTogether);
            Trace.WriteLine($"Cached {probAEmptyFirst:G4} for aFirst and {probABEmptyTogether:G4} for ab with pot capacity {a}, {b}");
            return (probAEmptyFirst, probABEmptyTogether);
        }

        public double Run(int volume)
        {
            var servings = (volume+24) / 25;
            foreach (var s in Enumerable.Range(1, servings - 1).Where(x => x % 2 == servings % 2))
            {
                CalculateProb(s, s);
            }
            var (aFirst, ab) = CalculateProb(servings, servings);
            return aFirst + (0.5 * ab);
        }
    }
}
