using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodingChallengeFramework;

using LookupDict = System.Collections.Concurrent.ConcurrentDictionary<(int val, int min, int max), string>;


namespace OperatorJumble
{
    public partial class MattTreeSearch : IOperatorJumble
    {
        public enum Op
        {
            M,
            D,
            A,
            S,
            E,
            C
        }

        public static Dictionary<Op, Func<int, int, int>> evalDict = new Dictionary<Op, Func<int, int, int>>()
        {
            {Op.M, (a, b) => checked(a * b)},
            {Op.D, (a, b) => {
                if (a % b != 0) { return Int32.MaxValue; }
                else { return a / b; }
                } },
            {Op.A, (a, b) => a + b},
            {Op.S, (a, b) => a - b},
            {Op.E, (a, b) => checked((Int32) Math.Pow((double) a, (double) b))},
            {Op.C, (a, b) => (a >= 0 && b >= 0) ? Convert.ToInt32($"{a}{b}") : Int32.MaxValue}
        };

        public static Dictionary<Op, Func<string, string, string>> exprDict = new Dictionary<Op, Func<string, string, string>>()
        {
            {Op.M, (a, b) => $"({a} * {b})"},
            {Op.D, (a, b) => $"({a} / {b})"},
            {Op.A, (a, b) => $"({a} + {b})"},
            {Op.S, (a, b) => $"({a} - {b})"},
            {Op.E, (a, b) => $"({a} ^ {b})"},
            {Op.C, (a, b) => $"({a} || {b})"}
        };

        public LookupDict LookupDictWithGroupings(bool parallel = false)
        {
            var nums = Enumerable.Range(1, 9).ToArray();
            var compDict = new LookupDict();
            if (parallel)
            {
                // this one is slower!  probably waste a lot of time waiting for locks...
                Parallel.ForEach(nums, min =>
                {
                    Parallel.ForEach(nums, max =>
                    {
                        if (max >= min)
                        {
                            var valAsString = string.Join("", Enumerable.Range(min, max - min + 1).Select(x => $"{x}"));
                            compDict[(Convert.ToInt32(valAsString), min, max)] = $"{valAsString}";
                        }
                    });
                });
            }
            else
            {
                foreach (var min in nums)
                {
                    foreach (var max in nums)
                    {
                        if (max >= min)
                        {
                            var valAsString = string.Join("", Enumerable.Range(min, max - min + 1).Select(x => $"{x}"));
                            compDict[(Convert.ToInt32(valAsString), min, max)] = $"{valAsString}";
                        }
                    }
                }
            }

            return compDict;
        }

        public void MergeDicts(LookupDict compDict, LookupDict nextGen)
        {
            foreach (var kv in nextGen)
            {
                if (!compDict.ContainsKey(kv.Key)
                    && kv.Key.val != Int32.MaxValue)
                {
                    compDict.TryAdd(kv.Key, kv.Value);
                }
            }
        }

        public LookupDict BuildCompDict(int levels, bool parallel = false)
        {
            var ops = new List<Op> { Op.A, Op.S, Op.M, Op.D, Op.E, Op.C };
            var compDict = LookupDictWithGroupings(parallel);
            int level = 2;
            while (level++ <= levels)
            {
                var nextGen = new LookupDict();
                if (parallel)
                {
                    Parallel.ForEach(compDict, kv1 =>
                    {
                        Parallel.ForEach(compDict, kv2 =>
                        {
                            if (kv2.Key.min == kv1.Key.max + 1)
                            {
                                Parallel.ForEach(ops, op =>
                                {
                                    try
                                    {
                                        var v = evalDict[op](kv1.Key.val, kv2.Key.val);
                                        nextGen[(v, kv1.Key.min, kv2.Key.max)] = exprDict[op](kv1.Value, kv2.Value);
                                    }
                                    catch
                                    {
                                    }
                                });
                            }
                        });
                    });
                }
                else
                {
                    foreach (var kv1 in compDict)
                    {
                        foreach (var kv2 in compDict)
                        {
                            if (kv2.Key.min == kv1.Key.max + 1)
                            {
                                foreach (var op in ops)
                                {
                                    try
                                    {
                                        var v = evalDict[op](kv1.Key.val, kv2.Key.val);
                                        nextGen[(v, kv1.Key.min, kv2.Key.max)] =
                                            exprDict[op](kv1.Value, kv2.Value);
                                    }
                                    catch
                                    {
                                    }
                                }
                            }
                        }
                    }
                }

                MergeDicts(compDict, nextGen);
            }

            return compDict;
        }

        

        public string Run(int n)
        {
            var level = 9;
            var compDict = BuildCompDictWithIndex(level, false);
            using (var f = new StreamWriter($"C:\\Users\\waldr\\Documents\\opjum\\seqIndexOut{level}.csv"))
            {
                foreach (var kv in compDict.Where(x => (x.Key.max - x.Key.min + 1 == level) && (x.Key.val > 0)).OrderBy(x => x.Key.val))
                {
                    f.WriteLine($"{kv.Key.val}, {kv.Value}");
                }
            }
            return (compDict.ContainsKey((n, 1, 9)))
                ? compDict[(n, 1, 9)]
                : "No solution";
        }
    }
}
