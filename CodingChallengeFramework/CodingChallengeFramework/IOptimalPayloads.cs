using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace CodingChallengeFramework
{
    [InheritedExport(typeof(IOptimalPayloads))]
    public interface IOptimalPayloads
    {
        List<(uint index, uint length)> Run(uint maxPayload, uint payloadCost, uint elementCost, List<uint> indices);
    }

    public class OptimalPayloadsChallenge : Challenge
    {
        [ImportMany(typeof(IOptimalPayloads), AllowRecomposition = true)]
        protected IOptimalPayloads[] payloadOptimizers = null;

        public override void Run(IEnumerable<string> args)
        {
            var argArray = args.ToArray();
            uint maxPayload, payloadCost, elementCost;
            List<uint> indices;

            if (argArray.Length == 1 && argArray[0] == "random")
            {
                var rand = new Random();
                maxPayload = (uint)rand.Next(2, 1000);
                payloadCost = (uint) rand.Next(0, 100);
                elementCost = (uint) rand.Next(0, 30);
                var nelems = rand.Next(10, 2000);
                indices = Enumerable.Repeat<Func<uint>>(() => (uint)rand.Next(nelems * 3), nelems)
                    .Select(f => f())
                    .Distinct()
                    .OrderBy(x => x)
                    .ToList();
            }
            else
            {
                if (!uint.TryParse(argArray[0], out maxPayload))
                {
                    Console.WriteLine($"Invalid maxPayload: {argArray[0]}");
                    throw new Exception();
                }
                if (!uint.TryParse(argArray[1], out payloadCost))
                {
                    Console.WriteLine($"Invalid payloadCost: {argArray[1]}");
                    throw new Exception();
                }
                if (!uint.TryParse(argArray[2], out elementCost))
                {
                    Console.WriteLine($"Invalid elementCost: {argArray[2]}");
                    throw new Exception();
                }

                indices = argArray.Skip(3).Select(s =>
                {
                    if (!uint.TryParse(s, out var v))
                    {
                        Console.WriteLine($"Invalid index: {s}");
                        throw new Exception();
                    }
                    return v;
                }).ToList();
            }

            Console.WriteLine($"Evaluating payload optimizers for maxPayload = {maxPayload}, payloadCost = {payloadCost}, elementCost = {elementCost}, nelements = {indices.Count}");
            Console.WriteLine($"Indices: {string.Join(" ", indices)}");

            Compose();
            var sw = new Stopwatch();
            foreach (var q in payloadOptimizers)
            {
                sw.Restart();
                var answer = "";
                try
                {
                    var result = q.Run(maxPayload, payloadCost, elementCost, indices);
                    var npayloads = result.Count;
                    answer = $"npayloads = {npayloads}, cost = {npayloads*payloadCost + result.Aggregate((a, b) => (0, a.length + b.length)).length*elementCost}";
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
