using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodingChallengeFramework;

namespace FewestPizzas
{
    public class FirstToppingShare
    {
        public int Run(int maxToppings, PizzaPreferences[] prefs)
        {
            return prefs.Select(p => p.favorites
                                      .OrderBy(x => x).First())
                        .Distinct()
                        .Count();
        } 
    }
}
