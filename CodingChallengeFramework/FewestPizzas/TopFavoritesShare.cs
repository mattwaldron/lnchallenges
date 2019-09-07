using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodingChallengeFramework;

namespace FewestPizzas
{
    // TODO: fill in your code in the Run function and give this class a unique name
    public class TopFavoritesShare : IFewestPizzas
    {
        public int Run(int maxToppings, PizzaPreferences[] prefs)
        {
            return prefs.Select(p => p.likes
                                      .OrderBy(x => x).First())
                        .Distinct()
                        .Count();
        } 
    }
}
