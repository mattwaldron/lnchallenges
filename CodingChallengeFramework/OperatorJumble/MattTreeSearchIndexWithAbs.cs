using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodingChallengeFramework;
using LookupDict = System.Collections.Generic.Dictionary<(int val, int min, int max), string>;
using LookupIndex = System.Collections.Generic.Dictionary<int, System.Collections.Generic.List<(int val, int min, int max)>>;

namespace OperatorJumble
{
    public partial class MattTreeSearch : IOperatorJumble
    {

        public LookupDict LookupDictWithGroupingsAbs(bool parallel = false)
        {
            var nums = Enumerable.Range(1, 9).ToArray();
            var compDict = new LookupDict();
            if (parallel)
            {
                Parallel.ForEach(nums, min =>
                {
                    Parallel.ForEach(nums, max =>
                    {
                        if (max >= min)
                        {
                            var valAsString = string.Join("", Enumerable.Range(min, max - min + 1).Select(x => $"{x}"));
                            compDict[(Convert.ToInt32(valAsString), min, max)] = $"{valAsString}";
                            if (min == 1)
                            {
                                compDict[(Convert.ToInt32(valAsString) * -1, min, max)] = $"-{valAsString}";
                            }
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
                            if (min == 1)
                            {
                                compDict[(Convert.ToInt32(valAsString) * -1, min, max)] = $"-{valAsString}";
                            }
                        }
                    }
                }
            }

            return compDict;
        }

        public void PopulateIndexAbs(LookupDict compDict, LookupIndex index)
        {
            foreach (var kv in compDict)
            {
                index[kv.Key.max - kv.Key.min + 1].Add(kv.Key);
            }
        }

        public LookupDict BuildCompDictWithIndexAbs(int levels, bool parallel = false)
        {
            var ops = new List<Op> { Op.A, Op.S, Op.M, Op.D, Op.E, Op.C };

            var index = GenerateIndex();
            var compDict = LookupDictWithGroupingsAbs(parallel);
            PopulateIndexAbs(compDict, index);
            //var log = new StreamWriter("buildCompDictLog.txt");
            int level = 1;
            while (level++ < levels)
            {
                //log.WriteLine($"Starting level {level}");
                if (parallel)
                {
                    Parallel.ForEach(index, kv1 =>
                    {
                        Parallel.ForEach(index, kv2 =>
                        {
                            if (kv1.Key + kv2.Key == level)
                            {
                                Parallel.ForEach(index[kv1.Key], i1 =>
                                {
                                    Parallel.ForEach(index[kv2.Key], i2 =>
                                    {
                                        if (i1.max + 1 == i2.min)
                                        {
                                            Parallel.ForEach(ops, op =>
                                            {
                                                try
                                                {
                                                    var v = evalDict[op](i1.val, i2.val);
                                                    if (v != Int32.MaxValue && !compDict.ContainsKey((v, i1.min, i2.max)))
                                                    {

                                                        compDict[((v, i1.min, i2.max))] = exprDict[op](compDict[(i1.val, i1.min, i1.max)], compDict[(i2.val, i2.min, i2.max)]);
                                                        index[level].Add((v, i1.min, i2.max));

                                                    }
                                                }
                                                catch { }
                                            });
                                        }
                                    });
                                });
                            }
                        });
                    });
                }
                else
                {
                    foreach (var kv1 in index)
                    {
                        foreach (var kv2 in index)
                        {
                            if (kv1.Key + kv2.Key == level)
                            {
                                foreach (var i1 in index[kv1.Key])
                                {
                                    foreach (var i2 in index[kv2.Key])
                                    {
                                        if (i1.max + 1 == i2.min)
                                        {
                                            foreach (var op in ops)
                                            {
                                                //log.WriteLine($"Trying {exprDict[op](compDict[(i1.val, i1.min, i1.max)], compDict[(i2.val, i2.min, i2.max)])}");
                                                try
                                                {
                                                    var v = evalDict[op](i1.val, i2.val);
                                                    if (v != Int32.MaxValue && !compDict.ContainsKey((v, i1.min, i2.max)))
                                                    {
                                                        compDict[((v, i1.min, i2.max))] = exprDict[op](compDict[(i1.val, i1.min, i1.max)], compDict[(i2.val, i2.min, i2.max)]);
                                                        index[level].Add((v, i1.min, i2.max));
                                                    }
                                                }
                                                catch { }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return compDict;
        }
    }
}
