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
    public class MattDynaSoup /*: ISoupServings*/
    {
        private List<Ratio> menu;
        private Dictionary<int, (double, double)> cachedStats;
        public MattDynaSoup()
        {
            menu = new List<Ratio>();
            menu.Add(new Ratio(4, 0));
            menu.Add(new Ratio(3, 1));
            menu.Add(new Ratio(2, 2));
            menu.Add(new Ratio(1, 3));

            cachedStats = new Dictionary<int, (double, double)>();
        }
        (double aFirst, double ab) CalculateProb(int servings)
        {
            if (cachedStats.ContainsKey(servings))
            {
                return cachedStats[servings];
            }
            double probAEmptyFirst = 0;
            double probABEmptyTogether = 0;
            var pots = new List<(Ratio, int)>()
            {
                (new Ratio(servings, servings), 1)
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
                            probABEmptyTogether += (1.0 / (Math.Pow(menu.Count, gen)));
                            break;
                        case ServeEvent.AEmpty:
                            probAEmptyFirst += (1.0 / (Math.Pow(menu.Count, gen)));
                            break;
                        case ServeEvent.Served when (newRatio.a == newRatio.b):
                            var (a, ab) = CalculateProb(newRatio.a);
                            probAEmptyFirst += a / (Math.Pow(menu.Count, gen));
                            probABEmptyTogether += ab / (Math.Pow(menu.Count, gen));
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
            cachedStats[servings] = (probAEmptyFirst, probABEmptyTogether);
            Trace.WriteLine($"Cached {probAEmptyFirst:G4} for aFirst and {probABEmptyTogether:G4} for ab with servings == {servings}");
            return (probAEmptyFirst, probABEmptyTogether);
        }

        public double Run(int volume)
        {
            var servings = (volume+24) / 25;
            foreach (var s in Enumerable.Range(1, servings-1).Where(x => x%2 == servings%2))
            {
                CalculateProb(s);
            }
            var (aFirst, ab) = CalculateProb(servings);
            return aFirst + (0.5 * ab);
        }
    }
}
