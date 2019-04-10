using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodingChallengeFramework;

namespace MakeChange
{
    public class MattRecurseWithMod /*: IMakeChange*/
    {
        int bestMin = int.MaxValue;
        public int EasyChange(long change, int[] denominations, int coins)
        {
            foreach (var c in denominations)
            {
                if (coins > bestMin)
                {
                    break;
                }
                coins += (int)(change / c);
                change = change % c;
                if (coins > bestMin)
                {
                    break;
                }
            }
            coins = change == 0 ? coins : Int32.MaxValue;
            bestMin = Math.Min(bestMin, coins);
            return coins;
        }

        public int MakeChange(long change, int[] denominations, int coins)
        {
            if (change == 0)
            {
                bestMin = Math.Min(bestMin, coins);
                return coins;
            }
            if (denominations.Length == 0 || coins > bestMin)
            {
                return Int32.MaxValue;
            }
            if (change >= denominations[0])
            {
                if (change % denominations[0] == 0)
                {
                    coins += (int)(change / denominations[0]);
                    bestMin = Math.Min(bestMin, coins);
                    return coins;
                }
                else
                {
                    return Math.Min(EasyChange(change, denominations, coins),
                        MakeChange(change, denominations.Skip(1).ToArray(), coins));
                }
            }
            return MakeChange(change, denominations.Where(x => x < change).ToArray(), coins);
        }

        public int Run(long change, int[] denominations)
        {
            var sortedCoins = denominations.Where(x => x < change).OrderByDescending(x => x).ToArray();
            MakeChange(change, sortedCoins, 0);
            return bestMin;
        }

    }
}
