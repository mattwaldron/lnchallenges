using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CodingChallengeFramework;


namespace SoupServings
{
    public class MattNativeRecurseCache /*: ISoupServings*/
    {
        private static (int a, int b)[] menu;
        private Dictionary<(int, int), (double, double)> cachedStats;

        public MattNativeRecurseCache()
        {
            menu = new (int a, int b)[]
            {
                (4, 0),
                (3, 1),
                (2, 2),
                (1, 3),
            };

            cachedStats = new Dictionary<(int, int), (double, double)>();
        }

        (double aFirst, double a) CalculateProb(int a, int b)
        {
            if (cachedStats.ContainsKey((a, b)))
            {
                return cachedStats[(a, b)];
            }
            double probAEmptyFirst = 0;
            double probABEmptyTogether = 0;
            foreach (var m in menu)
            {
                if (a - m.a <= 0 && b - m.b <= 0)
                {
                    probABEmptyTogether += 0.25;
                }
                else if (a - m.a <= 0)
                {
                    probAEmptyFirst += 0.25;
                }
                else if (b - m.b <= 0)
                {
                    continue;
                }
                else
                {
                    var (aFirst, ab) = CalculateProb(a - m.a, b - m.b);
                    probABEmptyTogether += 0.25 * ab;
                    probAEmptyFirst += 0.25 * aFirst;
                }
            }

            cachedStats[(a, b)] = (probAEmptyFirst, probABEmptyTogether);
            return (probAEmptyFirst, probABEmptyTogether);
        }

        public double Run(int volume)
        {
            var servings = (volume + 24) / 25;
            var (aFirst, ab) = CalculateProb(servings, servings);
            return aFirst + (0.5 * ab);
        }
    }
}
