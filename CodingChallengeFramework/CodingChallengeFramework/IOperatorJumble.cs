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
    [InheritedExport(typeof(IOperatorJumble))]
    public interface IOperatorJumble
    {
        string Run(int value);
    }

    public class OperatorJumbleChallenge : Challenge
    {
        [ImportMany(typeof(IOperatorJumble), AllowRecomposition = true)]
        protected IOperatorJumble[] jumblers = null;

        public override void Run(IEnumerable<string> args)
        {
             var n = Convert.ToInt32(args.First());

            Console.WriteLine($"Testing OperatorJumble algorithms against value of {n}");

            Compose();
            var sw = new Stopwatch();
            foreach (var q in jumblers)
            {
                string answer = "";
                try
                {
                    sw.Restart();
                    var result = q.Run(n);
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

