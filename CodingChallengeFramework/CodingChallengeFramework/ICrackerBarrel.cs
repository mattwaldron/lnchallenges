using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace CodingChallengeFramework
{
    [InheritedExport(typeof(ICrackerBarrel))]
    public interface ICrackerBarrel
    {
        List<(ushort jumpStartSpace, ushort jumpEndSpace)> Run(byte baseSize);
    }

    public class CrackerBarrelChallenge : Challenge
    {
        public static int OnePinLeftJumpCount(short baseSize)
        {
            return  (baseSize * (baseSize + 1) / 2) - 2;
        }

        public static string ComputeSolution(List<(ushort jumpStartSpace, ushort jumpEndSpace)> result, short baseSize)
        {
            if(result.Count == 0)
            {
                return "No Solution";
            }
            else if(result.Count != OnePinLeftJumpCount(baseSize))
            {
                return "Incorrect, more than 1 pin left";
            }
            else if (result.Count == OnePinLeftJumpCount(baseSize) && result[0].jumpEndSpace == result[result.Count-1].jumpEndSpace)
            {
                return "1 pin left in starting hole";
            }
            else
            {
                return "1 pin left not in starting hole";
            }          
        }

        [ImportMany(typeof(ICrackerBarrel), AllowRecomposition = true)]
        protected ICrackerBarrel[] crackerBarrelSolutions = null;

        public override void Run(IEnumerable<string> args)
        {
            var argArray = args.ToArray();
            byte _baseSize;          

            if (argArray.Length == 1 && argArray[0] == "random")
            {
                var rand = new Random();
                _baseSize = (byte)rand.Next(2, byte.MaxValue);               
            }
            else if (!byte.TryParse(argArray[0], out _baseSize))
            {     
                Console.WriteLine($"Invalid base size: {argArray[0]}");
                throw new Exception();                 
            }

            Console.WriteLine($"Find solution for a triangle with base size = {_baseSize}");
          
            Compose();
            var sw = new Stopwatch();
            foreach (var q in crackerBarrelSolutions)
            {                
                sw.Restart();
                var answer = "";
                try
                {
                    var result = q.Run(_baseSize);                    
                    answer = $"Numer of jumps:{result.Count} {ComputeSolution(result, _baseSize)}";                   
                }
                catch (Exception ex)
                {
                    answer = $" !!! Threw exception with message: {ex.Message}";
                }
                Console.WriteLine($"{q.GetType().Name} (in {sw.ElapsedMilliseconds} ms) << {answer}");
            }           
        }
    }
}
