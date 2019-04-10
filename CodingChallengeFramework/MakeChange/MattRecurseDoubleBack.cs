using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodingChallengeFramework;

namespace MakeChange
{
    public class MattRecurseDoubleBack /*: IMakeChange*/
    {
        public int EasyChange(long change, int [] denominations, int coins)
        {
            foreach (var c in denominations)
            {
                while (change >= c)
                {
                    coins++;
                    change -= c;
                }
            }

            return change == 0 ? coins : Int32.MaxValue;
        }

        public int MakeChange(long change, int [] denominations, int coins)
        {
            if (change == 0)
            {
                return coins;
            }
            if (denominations.Length == 0)
            {
                return Int32.MaxValue;
            }
            if (change >= denominations[0])
            {
                return Math.Min(EasyChange(change, denominations.ToArray(), coins),
                        MakeChange(change, denominations.Skip(1).ToArray(), coins));
            }
            return MakeChange(change, denominations.Where(x => x < change).ToArray(), coins);
        }

        public int Run(long change, int[] denominations)
        {
            var sortedCoins = denominations.Where(x => x < change).OrderByDescending(x => x).ToArray();
            return MakeChange(change, sortedCoins, 0);
        }

    }
}
