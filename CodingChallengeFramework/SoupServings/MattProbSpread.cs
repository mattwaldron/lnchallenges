using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodingChallengeFramework;


namespace SoupServings
{
    public class MattProbSpread : ISoupServings
    {
        private static List<Ratio> servings;

        public MattProbSpread()
        {
            servings = new List<Ratio>();
            servings.Add(new Ratio(100, 0));
            servings.Add(new Ratio(75, 25));
            servings.Add(new Ratio(50, 50));
            servings.Add(new Ratio(25, 75));
        }

        public double Run(int volume)
        {
            double probAEmptyFirst = 0;
            double probABEmptyTogether = 0;
            var pots = new List<(Ratio, int)>()
            {
                (new Ratio(volume, volume), 1)
            };
            while (pots.Count != 0)
            {
                var (thisRatio, gen) = pots[0];
                pots.RemoveAt(0);
                foreach (var s in servings)
                {
                    var newRatio = thisRatio.Copy();
                    var ev = newRatio.Serve(s);
                    switch (ev)
                    {
                        case ServeEvent.ABEmpty:
                            probABEmptyTogether += (1.0 / (Math.Pow(servings.Count, gen)));
                            break;
                        case ServeEvent.AEmpty:
                            probAEmptyFirst += (1.0 / (Math.Pow(servings.Count, gen)));
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

            return probAEmptyFirst + 0.5 * probABEmptyTogether;
        }
    }
}
