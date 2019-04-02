using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public Ratio Copy()
        {
            return new Ratio(a, b);
        }
    }

}
