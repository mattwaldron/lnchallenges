using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodingChallengeFramework;

namespace FewestPizzas
{
    public static class MattPizzaExtensions
    {
        public static PizzaPreferences AddPrefs(PizzaPreferences a, PizzaPreferences b)
        {
            var p = new PizzaPreferences();
            p.likes = a.likes.ToList();
            p.likes.AddRange(b.likes.ToList());
            p.hates = a.hates.ToList();
            p.hates.AddRange(b.hates.ToList());
            return p;
        }

        public static PizzaPreferences CombinePrefs(IList<PizzaPreferences> prefs)
        {
            var p = new PizzaPreferences();
            p.likes = new List<PizzaTopping>();
            p.hates = new List<PizzaTopping>();
            foreach (var pr in prefs)
            {
                p.likes.Intersect(pr.likes);
                p.hates.Union(pr.hates);
            }
            return p;
        }

        public static PizzaPreferences CombinePrefs(IList<PizzaPreferences> prefs, PizzaPreferences pref)
        {
            var p = new PizzaPreferences();
            p.likes = new List<PizzaTopping>();
            p.hates = new List<PizzaTopping>();
            foreach (var pr in prefs)
            {
                p.likes = p.likes.Intersect(pr.likes).ToList();
                p.hates = p.hates.Union(pr.hates).ToList();
            }
            p.likes = p.likes.Where(x => !p.hates.Contains(x)).ToList();
            return p;
        }

        static List<Pizza> Combine(IEnumerable<PizzaTopping> baseTop, IEnumerable<PizzaTopping> opts)
        {
            var pizzas = new List<Pizza>();
            foreach (var o in opts)
            {
                var p = new Pizza() { toppings = baseTop.ToList() };
                p.toppings.Add(o);
                pizzas.Add(p);
            }
            return pizzas;
        }

        class PizzaCompare<T> : IEqualityComparer<T>
        {
            public bool Equals(T x, T y)
            {
                var a = x as Pizza;
                var b = y as Pizza;
                if (a == null || b == null)
                {
                    return false;
                }
                if (a.toppings.Count != b.toppings.Count)
                {
                    return false;
                }
                return a.toppings.OrderBy(v => v).SequenceEqual(b.toppings.OrderBy(v => v));
            }

            public int GetHashCode(T obj)
            {
                var p = obj as Pizza;
                if (p == null)
                {
                    throw new Exception("Object must be pizza");
                }
                return p.toppings.OrderBy(x => x).Select(x => x.ToString()).Aggregate((a, b) => $"{a}{b}").GetHashCode();
            }
        }

        public static List<Pizza> AllCombinations(this IEnumerable<PizzaTopping> toppings, int maxToppings)
        {
            var pizzas = new List<Pizza>();
            foreach (var t in toppings)
            {
                pizzas.Add(new Pizza() { toppings = new List<PizzaTopping>() { t } });
            }
            for (var i = 2; i <= maxToppings; i++)
            {
                var newPizzas = new List<Pizza>();
                foreach (var p in pizzas)
                {
                    foreach (var p2 in pizzas.Where(x => x != p && p.toppings.Count + x.toppings.Count == i))
                    {
                        var combination = p.toppings.Union(p2.toppings); ;
                        if (combination.Count() == i)
                        {
                            newPizzas.Add(new Pizza() { toppings = combination.ToList() });
                        }
                    }
                }
                pizzas.AddRange(newPizzas.Distinct(new PizzaCompare<Pizza>()));
            }

            return pizzas;
        }

        public static IEnumerable<List<T>> Combinations<T>(this IEnumerable<T> vals, int n)
        {
            if (vals.Count() < n || n == 0)
            {
                yield break;
            }
            /*if (vals.Count() == n)
            {
                yield return vals.ToList();
            }*/
            if (n == 1)
            {
                foreach (var v in vals)
                {
                    yield return new List<T>() { v };
                }
            }
            else
            {
                for (var i = 0; i < vals.Count() - n + 1; i++)
                {
                    var baselist = vals.Skip(i).Take(1).ToList();
                    foreach (var ext in vals.Skip(i + 1).Combinations(n - 1))
                    {
                        yield return baselist.Union(ext).ToList();
                    }
                }
            }
        }

        public static List<List<Pizza>> AllCombinations(this IEnumerable<Pizza> pizzas, int max)
        {
            var pizzaCombos = new List<List<Pizza>>();
            foreach (var p in pizzas)
            {
                pizzaCombos.Add(new List<Pizza>() { p });
            }
            for (var i = 2; i <= max; i++)
            {
                var newCombos = new List<List<Pizza>>();
                foreach (var p in pizzaCombos)
                {
                    foreach (var p2 in pizzaCombos.Where(x => x != p && p.Count + x.Count == i))
                    {
                        var combination = p.Union(p2);
                        if (combination.Count() == i)
                        {
                            newCombos.Add(combination.ToList());
                        }
                    }
                }
                pizzaCombos.AddRange(newCombos.Distinct());
            }

            return pizzaCombos;
        }

        public static bool WillEat(this PizzaPreferences prefs, Pizza p)
        {
            if (prefs.hates.Intersect(p.toppings).Count() != 0)
            {
                return false;
            }
            if (prefs.likes.Intersect(p.toppings).Count() == 0)
            {
                return false;
            }
            return true;
        }
    }
}
