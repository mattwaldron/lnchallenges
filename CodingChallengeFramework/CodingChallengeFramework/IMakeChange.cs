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
        int Run(int change, int[] denominations);
    }

    public class MakeChangeChallenge : Challenge

    {
        [ImportMany(typeof(IMakeChange), AllowRecomposition = true)]
        protected IMakeChange[] changeMakers = null;

        public override void Run(IEnumerable<string> args)
        {
            var n = Convert.ToInt32(args.First());
            var denoms = args.Skip(1).Select(x => Convert.ToInt32(x)).ToArray();
            Compose();
            var sw = new Stopwatch();
            foreach (var q in changeMakers)
            {
                string answer = "";
                sw.Restart();
                try
                {
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
