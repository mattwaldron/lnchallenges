using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodingChallengeFramework;

namespace MakeChange
{
    // Duplicate or replace this class with your own and give it a unique name
    public class PennyPincher : IMakeChange
    {
        public int Run(long change, int[] denominations)
        {
            return (int)((change + 1) / denominations.Min());
        }
    }
}
