using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodingChallengeFramework;


namespace SoupServings
{
    enum ServeEvent
    {
        AEmpty,
        ABEmpty,
        Served
    }

    enum PotStatus
    {
        BothAvailable,
        OnlyA,
        OnlyB,
        Empty
    }

    class Ratio
    {
        public int a;
        public int b;

        public PotStatus Status
        {
            get
            {
                if (a <= 0 && b <= 0)
                {
                    return PotStatus.Empty;
                }

                if (a <= 0)
                {
                    return PotStatus.OnlyB;
                }

                if (b <= 0)
                {
                    return PotStatus.OnlyA;
                }

                return PotStatus.BothAvailable;
            }
        }

        public Ratio(int aa, int bb)
        {
            a = aa;
            b = bb;
        }

        public ServeEvent Serve(Ratio serving)
        {
            var preServe = Status;
            a -= serving.a;
            b -= serving.b;
            var postServe = Status;
            if (preServe == postServe)
            {
                return ServeEvent.Served;
            }

            if (postServe == PotStatus.OnlyB)
            {
                return ServeEvent.AEmpty;
            }

            if (preServe == PotStatus.BothAvailable && postServe == PotStatus.Empty)
            {
                return ServeEvent.ABEmpty;
            }

            return ServeEvent.Served;
        }
    }

    class MattProbSpread : ISoupServings
    {
        private static List<Ratio> servings;

        public MattProbSpread()
        {
            servings = new List<Ratio>();
            servings.Add(new Ratio(100, 0));
            servings.Add(new Ratio(75, 25));
            servings.Add(new Ratio(50, 50));
            servings.Add(new Ratio(25, 75));
        }


        public double Run(int volume)
        {
            var pots = new Ratio(volume, volume);
            throw new NotImplementedException();
        }
    }
}
