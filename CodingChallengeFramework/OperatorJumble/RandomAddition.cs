using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodingChallengeFramework;

namespace OperatorJumble
{
    public class RandomAddition /*: IOperatorJumble*/
    {
        public string Run(int n)
        {
            var s = "1";
            var r = new Random();
            foreach (var x in new [] { "2", "3", "4", "5", "6", "7", "8","9"})
            {
                s += r.Next() % 2 == 0 ? "+" : "-";
                s += x;
            }
            return s;
        }
    }
}
