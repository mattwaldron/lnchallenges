using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingChallengeFramework
{
    interface IChallenge
    {
        void Run(IEnumerable<string> args);
    }

    public class DefaultChallenge : IChallenge
    {
        public void Run(IEnumerable<string> args)
        {
            Console.WriteLine("No challenge selected");
        }
    }
}
