using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CodingChallengeFramework
{
    [InheritedExport(typeof(IFewestPizzas))]
    public interface IFewestPizzas
    {
        int Run(int maxToppings, PizzaPreferences[] prefs);
    }

    public class FewestPizzasChallenge : Challenge
    {
        [ImportMany(typeof(IFewestPizzas), AllowRecomposition = true)]
        protected IFewestPizzas[] pizzaAlgos = null;

        public override void Run(IEnumerable<string> args)
        {
            var maxToppings = Int32.Parse(args.First());

            PizzaPreferences[] prefs;
            if (args.Skip(1).First().ToLower() == "random")
            {
                if (args.Count() >= 4)
                {
                    prefs = PizzaPreferences.Random(int.Parse(args.Skip(2).First()),
                                                    int.Parse(args.Skip(3).First()));
                }
                else
                {
                    prefs = PizzaPreferences.Random(int.Parse(args.Skip(2).First()));
                }
            }
            else
            {
                prefs = JsonConvert.DeserializeObject<PizzaPreferences[]>(args.Skip(1).Aggregate((a, b) => $"{a}{b}"));
            }

            Console.WriteLine($"Testing FewestPizzas algorithms with max toppings {maxToppings} and preferences {JsonConvert.SerializeObject(prefs, Formatting.Indented, new StringEnumConverter())}");

            Compose();
            var sw = new Stopwatch();
            foreach (var q in pizzaAlgos)
            {
                string answer = "";
                try
                {
                    sw.Restart();
                    var result = q.Run(maxToppings, prefs);
                    answer = $"{result}";
                }
                catch (Exception ex)
                {
                    answer = $" !!! Threw exception with message: {ex.Message}";
                }
                Console.WriteLine($"{q.GetType().Name} (in {sw.ElapsedMilliseconds} ms) << {answer}");
            }
        }
    }
}
