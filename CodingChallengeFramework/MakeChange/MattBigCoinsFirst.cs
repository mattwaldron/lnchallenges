using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodingChallengeFramework;

namespace MakeChange
{
    public class MattBigCoinsFirst : IMakeChange
    {
        public int Run(long change, int[] denominations)
        {
            var sortedCoins = denominations.OrderByDescending(x => x).ToArray();
            var ncoins = 0;

            foreach (var c in sortedCoins)
            {
                while (change >= c)
                {
                    ncoins++;
                    change -= c;
                }
            }
            
            return change == 0 ? ncoins : Int32.MaxValue;
        }
    }
}
