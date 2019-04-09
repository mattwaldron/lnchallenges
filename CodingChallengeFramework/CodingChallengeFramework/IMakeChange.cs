using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CodingChallengeFramework
{
    [InheritedExport(typeof(IMakeChange))]
    public interface IMakeChange
    {
        int Run(long change, int[] denominations);
    }

    public class MakeChangeChallenge : Challenge

    {
        [ImportMany(typeof(IMakeChange), AllowRecomposition = true)]
        protected IMakeChange[] changeMakers = null;

        public override void Run(IEnumerable<string> args)
        {
            long n;
            int[] denoms;
            if (args.First() == "random")
            {
                n = Convert.ToInt64(args.Skip(1).First());
                var r = new Random();
                denoms = Enumerable.Repeat(0, Convert.ToInt32(args.Skip(2).First())).Select(x => r.Next(1, (int)n)).Append(1).ToArray();
            }
            else
            {
                n = Convert.ToInt64(args.First());
                denoms = args.Skip(1).Select(x => Convert.ToInt32(x)).ToArray();
            }

            Console.WriteLine($"Testing MakeChange algorithms against value of {n} and denominations: [{string.Join(", ", denoms.OrderBy(x => x))}]");

            Compose();
            var sw = new Stopwatch();
            foreach (var q in changeMakers)
            {
                string answer = "";
                try
                {
                    // prime the pump!
                    q.Run(1, new[] { 1 });
                    sw.Restart();
                    var result = q.Run(n, denoms);
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
