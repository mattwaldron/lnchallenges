using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CodingChallengeFramework;
using SoupServings;
using MakeChange;
using FewestPizzas;

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
            var pizza = new FirstToppingShare();
            var npeople = 7;
            var prefs = PizzaPreferences.Random(npeople);
            var npizzas = pizza.Run(3, prefs);
            Console.WriteLine($"Ordering {npizzas} for {npeople} people");
        }
    }
}
