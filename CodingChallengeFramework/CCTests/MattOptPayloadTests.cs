using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OptimalPayloads;


namespace CCTests
{
    [TestClass]
    public class MattOptPayloadTests
    {
        [TestMethod]
        public void SplitLargest()
        {
            var indices = new List<uint>
            {
                0, 1, 2, 5, 6, 7
            };

            var split = MattSplitLargestGaps.SplitLargestGap(indices);

            CollectionAssert.AreEqual(new List<uint> {0, 1, 2}, split[0]);
            CollectionAssert.AreEqual(new List<uint> { 5, 6, 7 }, split[1]);
        }

        [TestMethod]
        public void SplitLargest2()
        {
            var indices = new List<uint>
            {
                0, 1, 2, 5, 6, 7, 10, 11, 12, 13, 14, 15
            };

            var split = MattSplitLargestGaps.SplitLargestGap(indices);

            CollectionAssert.AreEqual(new List<uint> { 0, 1, 2, 5, 6, 7 }, split[0]);
            CollectionAssert.AreEqual(new List<uint> { 10, 11, 12, 13, 14, 15 }, split[1]);
        }
    }
}
