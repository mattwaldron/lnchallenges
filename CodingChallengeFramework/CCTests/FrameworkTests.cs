﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CodingChallengeFramework;

namespace CCTests
{
    [TestClass]
    public class FrameworkTests
    {
        [TestMethod]
        public void Maze()
        {
            var m = MazeGenerator.GetMaze(4, 4);
            foreach (var row in m)
            {
                Console.WriteLine(row);
            }
        }
    }
}
