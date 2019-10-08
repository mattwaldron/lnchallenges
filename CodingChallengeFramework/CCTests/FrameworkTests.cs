using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using CodingChallengeFramework;
using SoupServings;
using MakeChange;
using FewestPizzas;
using System.IO;

namespace CCTests
{
    [TestClass]
    public class FrameworkTests
    {
        [TestMethod]
        public void Maze()
        {
            var m = MazeGenerator.GetMaze(4, 4);
            foreach (var row in m)
            {
                Console.WriteLine(row);
            }
        }

        [TestMethod]
        public void MattSoup_XinExample()
        {
            var ss = new MattProbSpread();
            var ans = ss.Run(50);
            Assert.AreEqual(0.625, ans);
        }

        [TestMethod]
        public void MattSoup_MiscVolume()
        {
            var ss = new MattNativeRecurse();
            var ans = ss.Run(50);
            Assert.AreEqual(0.625, ans);
        }

        [TestMethod]
        public void MattSoup_Sweep()
        {
            var ss = new MattDynaSoup();
            foreach (var v in Enumerable.Range(1, 30))
            {
                var vv = v * 50;
                Trace.WriteLine($"N = {vv}");
            }
        }

        [TestMethod]
        public void MattChange_Misc()
        {
            var mc = new MattRecurseDoubleBack();
            Assert.AreEqual(1, mc.Run(59, new[] { 1, 8, 47, 47, 52, 54 }));

        }

        [TestMethod]
        public void NewPizzas()
        {
            var pp = PizzaPreferences.RandomJson(6);
            Console.WriteLine(pp);
        }

        [TestMethod]
        public void SimplePizza()
        {
            var pizza = new TopFavoritesShare();
            var npeople = 7;
            var prefs = PizzaPreferences.Random(npeople);
            var npizzas = pizza.Run(3, prefs);
            Console.WriteLine($"Ordering {npizzas} for {npeople} people");
        }

        [TestMethod]
        public void PizzaCopy()
        {
            var p1 = new Pizza()
            {
                toppings = new List<PizzaTopping>()
                {
                    PizzaTopping.Anchovies,
                    PizzaTopping.Bacon,
                    PizzaTopping.BananaPeppers
                }
            };

            var p2 = Pizza.Copy(p1);
            p2.toppings[0] = PizzaTopping.Sausage;
            CollectionAssert.AreNotEqual(p1.toppings, p2.toppings);
        }

        [TestMethod]
        public void PizzaCombinatons()
        {
            var toppings = new List<PizzaTopping>()
            {
                PizzaTopping.Anchovies,
                PizzaTopping.BananaPeppers,
                PizzaTopping.BlackOlives,
                PizzaTopping.Chicken
            };

            var pizzaCombos = toppings.AllCombinations(2);
            Assert.AreEqual(10, pizzaCombos.Count);
        }

        [TestMethod]
        public void FewestPizzas_RandomGuests()
        {
            
            var pizzaCalc = new MattPopularDescending();
            var scale = Enum.GetValues(typeof(PizzaTopping)).Length;
            List<Pizza> pizzas = null;
            var seed = Environment.TickCount;
            
            for (var i = 0; i < 1000; i++)
            {
                var rand = new Random(seed + i);
                var npeople = 1+ rand.Next(20);
                var nlikes = 1 + rand.Next(scale / 2);
                var nhates = rand.Next(scale / 4);
                var ntoppings = 1 + rand.Next(scale / 4);

                var guests = PizzaPreferences.Random(npeople, nlikes, nhates, seed + i + 1);

                File.WriteAllText("fewestPizzas.txt", $"Iteration {i}:\nseed: {seed}\nmaxToppings: {ntoppings}\nnlikes: {nlikes}\nnhates: {nhates}\n{JsonConvert.SerializeObject(guests, Formatting.Indented, new StringEnumConverter())}\n\n");

                try
                {
                    pizzas = pizzaCalc.PartyOrder(ntoppings, guests);
                    if (!guests.All(guest => pizzas.Any(pizza => guest.WillEat(pizza))))
                    {
                        throw new Exception("What a terrible party!");
                    }
                }
                catch
                {
                    Console.WriteLine("Failed to pick out pizzas for:");
                    Console.WriteLine(JsonConvert.SerializeObject(guests, Formatting.Indented, new StringEnumConverter()));
                    Console.WriteLine("Attempted:");
                    Console.WriteLine(JsonConvert.SerializeObject(pizzas, Formatting.Indented, new StringEnumConverter()));
                    Assert.Fail();
                }
            }
        }

        public void FewestPizzas_SpecificSeed()
        {

            var pizzaCalc = new MattOptimizedCombos();
            var scale = Enum.GetValues(typeof(PizzaTopping)).Length;
            List<Pizza> pizzas = null;

            var seed = 404613422 + 69;
            var rand = new Random(seed);
            var npeople = 1 + rand.Next(30);
            var nlikes = 1 + rand.Next(scale / 2);
            var nhates = rand.Next(scale / 4);
            var ntoppings = 1 + rand.Next(scale / 4);

            var guests = PizzaPreferences.Random(npeople, nlikes, nhates, seed + 1);

            File.WriteAllText("fewestPizzas.txt", $"seed: {seed}\nmaxToppings: {ntoppings}\nnlikes: {nlikes}\nnhates: {nhates}\n{JsonConvert.SerializeObject(guests, Formatting.Indented, new StringEnumConverter())}\n\n");

            try
            {
                pizzas = pizzaCalc.PartyOrder(ntoppings, guests);
                if (!guests.All(guest => pizzas.Any(pizza => guest.WillEat(pizza))))
                {
                    throw new Exception("What a terrible party!");
                }
            }
            catch
            {
                Console.WriteLine("Failed to pick out pizzas for:");
                Console.WriteLine(JsonConvert.SerializeObject(guests, Formatting.Indented, new StringEnumConverter()));
                Console.WriteLine("Attempted:");
                Console.WriteLine(JsonConvert.SerializeObject(pizzas, Formatting.Indented, new StringEnumConverter()));
                Assert.Fail();
            }
        }

        double NCR(int n, int r)
        {
            if (n == 0 || r == 0)
            {
                return 0;
            }
            var num = Enumerable.Range(r + 1, n - r).Select(x => (double)x).Aggregate((a, b) => a * b);
            var den = Enumerable.Range(1, n - r).Select(x => (double)x).Aggregate((a, b) => a * b);

            return num / den;
        }

        [TestMethod]
        public void Combinations_RandomInts()
        {
            var rand = new Random();
            for (var i = 0; i < 100; i++)
            {
                var n = rand.Next(30);
                var r = rand.Next(n);
                Console.Write($"Verifying with n == {n} and r == {r}... ");
                var ncr = NCR(n, r);
                var combos = Enumerable.Range(1, n).Combinations(r);
                var ncombos = 0;
                foreach (var c in combos)
                {
                    //Console.WriteLine(c.Select(x => $"{x}").Aggregate((a, b) => $"{a}, {b}"));
                    ncombos++;
                }
                Assert.AreEqual(ncr, (double)ncombos, 1);
                Console.WriteLine($"okay!");
            }
        }

        [TestMethod]
        public void Combinations_IntExample()
        {
            var n = 20;
            var r = 10;
            Console.Write($"Verifying with n == {n} and r == {r}... ");
            var ncr = NCR(n, r);
            var combos = Enumerable.Range(1, n).Combinations(r);
            var ncombos = 0;
            foreach (var c in combos)
            {
                //Console.WriteLine(c.Select(x => $"{x}").Aggregate((a, b) => $"{a}, {b}"));
                ncombos++;
            }
            Assert.AreEqual(ncr, (double)ncombos, 1);
            Console.WriteLine($"okay!");
        }
    }
}
