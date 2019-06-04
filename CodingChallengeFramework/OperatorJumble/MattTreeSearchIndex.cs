using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodingChallengeFramework;
using LookupDict = System.Collections.Concurrent.ConcurrentDictionary<(int val, int min, int max), string>;
using LookupIndex = System.Collections.Generic.Dictionary<int, System.Collections.Generic.List<(int val, int min, int max)>>;

namespace OperatorJumble
{
    public partial class MattTreeSearch : IOperatorJumble
    {
        public LookupIndex GenerateIndex()
        {
            var index = new LookupIndex();
            foreach (var v in Enumerable.Range(1, 9))
            {
                index[v] = new List<(int val, int min, int max)>();
            }
            return index;
        }

        public void PopulateIndex(LookupDict compDict, LookupIndex index)
        {
            foreach (var kv in compDict)
            {
                index[kv.Key.max - kv.Key.min + 1].Add(kv.Key);
            }
        }

        public LookupDict BuildCompDictWithIndex(int levels, bool parallel = false)
        {
            var ops = new List<Op> { Op.A, Op.S, Op.M, Op.D, Op.E, Op.C };

            var index = GenerateIndex();
            var compDict = LookupDictWithGroupings(parallel);
            PopulateIndex(compDict, index);

            int level = 1;
            while (level++ < levels)
            {
                if (parallel)
                {
                    Parallel.ForEach (index, kv1 =>
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
