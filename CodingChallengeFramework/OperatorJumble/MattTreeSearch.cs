using System;
using System.Collections.Generic;
using System.IO;
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
        /// <summary>
        /// Initialize a LookupIndex with empty Lists for each expression length (1 to 9)
        /// </summary>
        LookupIndex GenerateIndex()
        {
            var index = new LookupIndex();
            foreach (var v in Enumerable.Range(1, 9))
            {
                index[v] = new List<(int val, int min, int max)>();
            }
            return index;
        }

        /// <summary>
        /// Populate a LookupDict with all adjacent groups of numbers, e.g.: 1, 12, 123, 1234... 89, 9 
        /// </summary>
        LookupDict LookupDictWithGroupings()
        {
            var nums = Enumerable.Range(1, 9).ToArray();
            var compDict = new LookupDict();

            // Parallel seemed slower here, hmmm...
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

            return compDict;
        }

        /// <summary>
        /// Add to the index all the elements in the initial LookupDict
        /// </summary>
        void PopulateIndex(LookupDict compDict, LookupIndex index)
        {
            foreach (var kv in compDict)
            {
                index[kv.Key.max - kv.Key.min + 1].Add(kv.Key);
            }
        }

        /// <summary>
        /// Build a LookupDict for the number of levels specified.  The number of levels is the number of values in
        /// an expression.  The resulting dictionary will include all the levels less than the specified level as well.
        /// </summary>
        public LookupDict BuildCompDict(int levels)
        {
            var ops = new List<Op> { Op.A, Op.S, Op.M, Op.D, Op.E, Op.C };

            var index = GenerateIndex();
            var compDict = LookupDictWithGroupings();
            PopulateIndex(compDict, index);

            int level = 1;
            while (level++ < levels)
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
                                               if (v != Int32.MaxValue)
                                               {
                                                   var expr = exprDict[op](compDict[(i1.val, i1.min, i1.max)], compDict[(i2.val, i2.min, i2.max)]);
                                                   if (!compDict.ContainsKey((v, i1.min, i2.max)))
                                                   {
                                                       compDict[((v, i1.min, i2.max))] = expr;
                                                       index[level].Add((v, i1.min, i2.max));
                                                   }
                                                   else if (CountOps(compDict[(v, i1.min, i2.max)]) > CountOps(expr))
                                                   {
                                                       compDict[((v, i1.min, i2.max))] = expr;
                                                   }
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

            return compDict;
        }

        public string Run(int n)
        {
            var level = 9;
            var compDict = BuildCompDict(level);
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
