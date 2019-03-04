using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.Composition;

namespace CodingChallengeFramework
{
    class Program
    {
        static void Main(string[] args)
        {
            IChallenge c = new DefaultChallenge();
            if (args[0].Contains("queens"))
            {
                c = new SafeQueensChallenge();
            }
            else if (args[0].Contains("maze"))
            {
                c = new MazeSolverChallenge();
            }
            else if (args[0].Contains("soup"))
            {
                c = new SoupServingsChallenge();
            }
            c.Run(args.Skip(1));
        }
    }
}
