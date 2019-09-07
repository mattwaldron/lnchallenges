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
        public List<PizzaTopping> likes;
        public List<PizzaTopping> hates;

        public static PizzaPreferences operator +(PizzaPreferences a, PizzaPreferences b)
        {
            var pref = new PizzaPreferences();
            pref.likes = a.likes.Select(x => x).ToList();
            pref.likes.AddRange(b.likes);

            pref.hates = a.hates.Select(x => x).ToList();
            pref.hates.AddRange(b.hates);
            return pref;
        }

        public static PizzaPreferences [] Random(int n, int nlikes = 2, int nhates = 1, int randomSeed = int.MaxValue)
        {
            if (randomSeed == int.MaxValue)
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
                prefs[i].likes = Enumerable.Repeat<Func<int>>(() => rand.Next(toppings.Length), nlikes).Select(v => (PizzaTopping)toppings.GetValue(v())).ToList();
                prefs[i].hates = Enumerable.Repeat<Func<int>>(() => rand.Next(toppings.Length), nhates).Select(v => (PizzaTopping)toppings.GetValue(v())).Where(t => !prefs[i].likes.Contains(t)).ToList();
            }
            return prefs;
        }

        public static string RandomJson(int n, Int32 randomSeed = Int32.MaxValue)
        {
            return JsonConvert.SerializeObject(Random(n, randomSeed), Formatting.Indented, new StringEnumConverter());
        }
    }
}
