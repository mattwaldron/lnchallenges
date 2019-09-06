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
            {Op.E, (a, b) => {
                if (b < 0) { return Int32.MaxValue; }
                else {return checked((Int32) Math.Pow((double) a, (double) b)); }
                } },
            {Op.C, (a, b) => (a >= 0 && b >= 0) ? Convert.ToInt32($"{a}{b}") : Int32.MaxValue}
        };

        public static Dictionary<Op, Func<string, string, string>> exprDict = new Dictionary<Op, Func<string, string, string>>()
        {
            {Op.M, (a, b) => $"({a} * {b})"},
            {Op.D, (a, b) => $"({a} / {b})"},
            {Op.A, (a, b) => $"({a} + {b})"},
            {Op.S, (a, b) => $"({a} - {b})"},
            {Op.E, (a, b) => $"({a} ^ {b})"},
            {Op.C, (a, b) => $"({a} | {b})"}
        };

        public static int CountOps(string s)
        {
            return s.Count(c => "*/+-^|".Contains(c));
        }
    }
}
