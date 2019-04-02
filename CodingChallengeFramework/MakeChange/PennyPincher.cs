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
        public int Run(int change, int[] denominations)
        {
            return (change + 1) / denominations.Min();
        }
    }
}
