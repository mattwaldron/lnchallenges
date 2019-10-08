using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication.ExtendedProtection;
using System.Text;
using System.Threading.Tasks;
using CodingChallengeFramework;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace FewestPizzas
{
    public class MattPopularDescending : IFewestPizzas
    {
        public bool print = false;

        int BestCasePizzaCount(int maxToppings, PizzaPreferences [] prefs)
        {
            var prefList = prefs.OrderBy(x => x.likes.Count).ToList();
            for (var i = 1; i < prefList.Count; i++)
            {
                for (var j = 0; j < i; j++)
                {
                    if (prefList[j].likes.Intersect(prefList[i].likes).Count() != 0)
                    {
                        prefList.RemoveAt(i);
                        i--;
                        break;
                    }
                }
            }
            return (int)Math.Ceiling((double)prefList.Count / maxToppings);
        }

        // borrowed from stack overflow!
        public static long NCR(int n, int r)
        {
            // naive: return Factorial(n) / (Factorial(r) * Factorial(n - r));
            return NPR(n, r) / Factorial(r);
        }

        public static long NPR(int n, int r)
        {
            // naive: return Factorial(n) / Factorial(n - r);
            return FactorialDivision(n, n - r);
        }

        private static long FactorialDivision(int topFactorial, int divisorFactorial)
        {
            long result = 1;
            for (int i = topFactorial; i > divisorFactorial; i--)
                result *= i;
            return result;
        }

        private static long Factorial(int i)
        {
            if (i <= 1)
                return 1;
            return i * Factorial(i - 1);
        }

        private bool PizzasOverlap(IEnumerable<Pizza> pizzas)
        {
            var pizzaList = pizzas.ToList();
            for (var i = 1; i< pizzaList.Count; i++)
            {
                if (pizzaList[i]
                    .toppings
                    .OrderBy(x => x)
                    .SequenceEqual(pizzas
                                    .Take(i)
                                    .Select(x => x.toppings)
                                    .Aggregate((a, b) => a.Union(b).ToList())
                                    .OrderBy(x => x)))
                {
                    return true;
                }
            }
            return false;
        }

        public List<Pizza> PartyOrder(int maxToppings, PizzaPreferences[] prefs)
        {
            // create all the possible pizzas
            var minPizzas = BestCasePizzaCount(maxToppings, prefs);
            var possibleToppings = prefs.Select(p => p.likes).Aggregate((a, b) => a.Union(b).ToList());
            var pizzas = new List<Pizza>();
            for (var i = 1; i <= maxToppings; i++)
            {
                foreach (var toppingSet in possibleToppings.Combinations(i))
                {
                    pizzas.Add(new Pizza() { toppings = toppingSet });
                }
            }

            // Find who will eat each pizza
            var pizzaSatisfaction = new Dictionary<Pizza, List<int>>();
            foreach (var thisPizza in pizzas)
            {
                pizzaSatisfaction[thisPizza] = new List<int>();
                for (var i = 0; i < prefs.Length; i++)
                {
                    var thisPerson = prefs[i];
                    if (thisPerson.WillEat(thisPizza))
                    {
                        pizzaSatisfaction[thisPizza].Add(i);
                    }
                }

                if (pizzaSatisfaction[thisPizza].Count() == prefs.Length)
                {
                    if (print)
                    {
                        Console.WriteLine(JsonConvert.SerializeObject(thisPizza, Formatting.Indented, new StringEnumConverter()));
                    }
                    return new List<Pizza>() { thisPizza };
                }
            }

            var popularPizzas = pizzaSatisfaction.Keys.OrderByDescending(k => pizzaSatisfaction[k].Count).ToList();
            List<int> satisfiedGuests;

            var pizzaSet = new List<Pizza>()
            {
                popularPizzas.First()
            };

            var idx = 1;
            while ((satisfiedGuests = pizzaSet.Select(p => pizzaSatisfaction[p]).Aggregate((a, b) => a.Union(b).ToList())).Count() < prefs.Length)
            {
                while (true)
                {
                    if (satisfiedGuests.Union(pizzaSatisfaction[popularPizzas[idx]]).Count()
                        > satisfiedGuests.Count())
                    {
                        pizzaSet.Add(popularPizzas[idx]);
                        idx++;
                        break;
                    }

                    idx++;
                }
            }

            return pizzaSet;
        }
        public int Run(int maxToppings, PizzaPreferences[] prefs)
        {
            var pizzas = PartyOrder(maxToppings, prefs);
            return pizzas?.Count ?? prefs.Length;
        }
    }
}
