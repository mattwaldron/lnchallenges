using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodingChallengeFramework;

namespace CrackerBarrel
{
    public class BaseSizeFiveExample : ICrackerBarrel
    {
        public List<(ushort jumpStartSpace, ushort jumpEndSpace)> Run(byte baseSize)
        {
            // Your solution here

            // Example return for a base of 5

            List<(ushort index, ushort length)> retVector = new List<(ushort jumpStartSpace, ushort jumpEndSpace)>();

            return new List<(ushort jumpStartSpace, ushort jumpEndSpace)> { (13, 6), (15, 13), (6, 15), (7, 9), (12, 14), (15, 13), (13, 6), (2, 7), (11, 4), (3, 10), (4, 6), (10, 3), (1, 6) };
        }
    }
}
