using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CodingChallengeFramework
{
    public enum PizzaTopping
    {
        Anchovies,
        Bacon,
        BananaPeppers,
        Beef,
        BlackOlives,
        CanadianBacon,
        Chicken,
        GreenOlive,
        GreenPepper,
        ItalianSausage,
        Jalapeno,
        Mushroom,
        Onion,
        Pineapple,
        Pepperoni,
        Sausage

    }

    public class Pizza
    {
        public List<PizzaTopping> toppings;
    }

    public class PizzaPreferences
    {
        public List<PizzaTopping> favorites;
        public List<PizzaTopping> dislikes;

        public static PizzaPreferences operator +(PizzaPreferences a, PizzaPreferences b)
        {
            var pref = new PizzaPreferences();
            pref.favorites = a.favorites.Select(x => x).ToList();
            pref.favorites.AddRange(b.favorites);

            pref.dislikes = a.dislikes.Select(x => x).ToList();
            pref.dislikes.AddRange(b.dislikes);
            return pref;
        }

        public static PizzaPreferences [] Random(int n, Int32 randomSeed = Int32.MaxValue)
        {
            const int nfavs = 2;
            const int nhates = 1;

            if (randomSeed == Int32.MaxValue)
            {
                randomSeed = Environment.TickCount;
            }
            var rand = new Random(randomSeed);
            Console.WriteLine($"Using random seed {randomSeed}");

            var prefs = new PizzaPreferences[n];
            var toppings = Enum.GetValues(typeof(PizzaTopping));
            foreach (var i in Enumerable.Range(0, n))
            {
                prefs[i] = new PizzaPreferences();
                prefs[i].favorites = Enumerable.Repeat<Func<int>>(() => rand.Next(toppings.Length), nfavs).Select(v => (PizzaTopping)toppings.GetValue(v())).ToList();
                prefs[i].dislikes = Enumerable.Repeat<Func<int>>(() => rand.Next(toppings.Length), nhates).Select(v => (PizzaTopping)toppings.GetValue(v())).Where(t => !prefs[i].favorites.Contains(t)).ToList();
            }
            return prefs;
        }

        public static string RandomJson(int n, Int32 randomSeed = Int32.MaxValue)
        {
            return JsonConvert.SerializeObject(Random(n, randomSeed), Formatting.Indented, new StringEnumConverter());
        }
    }
}
