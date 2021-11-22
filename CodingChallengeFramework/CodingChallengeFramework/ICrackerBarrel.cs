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
            List<Jump> fullJumpList = new List<Jump>();
            List<Pin> fullPinList = CreatePinsAndJumpTable(baseSize, fullJumpList);
            InitPinJumps(fullPinList, fullJumpList);

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
                if(VerifyJumps(result,fullPinList))
                {
                    return "Prime solution found - 1 pin left in starting hole";
                }
                else
                {
                    return "Invalid solution - jumps not valid";
                }                
            }
            else
            {
                if(VerifyJumps(result,fullPinList))
                {
                    return "Non-Prime Solution found - 1 pin left not in starting hole";
                }
                else
                {
                    return "Invalid solution - jumps not valid";
                }               
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
                Console.ReadKey();
            }           
        }

        public static List<Pin> CreatePinsAndJumpTable(int baseSize, List<Jump> jumps)
        {           
            List<Pin> retPinList = new List<Pin>();
            int PinID = 1;           
            for (int tier = 1; tier <= baseSize; tier++)
            {  
                for (int tierPos = 1; tierPos <= tier; tierPos++)
                {
                    //Set Pin
                    retPinList.Add(new Pin(PinID));                 

                    //Set Jumps for each Pin
                    if (tier > 2)
                    {
                        if (tierPos < (tier - 1))
                        {
                            // add SW<->NE jump pins
                            int SW_NEMiddlePinID = PinID - (tier - 1);
                            jumps.Add(new Jump(PinID, SW_NEMiddlePinID, SW_NEMiddlePinID - (tier - 2)));
                            jumps.Add(new Jump(SW_NEMiddlePinID - (tier - 2), SW_NEMiddlePinID, PinID));
                            // W<->E jump pins
                            jumps.Add(new Jump(PinID, PinID + 1, PinID + 2));
                            jumps.Add(new Jump(PinID + 2, PinID + 1, PinID));
                        }

                        if (tierPos > 2)
                        {
                            // add NW<->SE jump pins
                            int NW_SEMiddlePin = PinID - tier;
                            jumps.Add(new Jump(PinID, NW_SEMiddlePin, NW_SEMiddlePin - (tier - 1)));
                            jumps.Add(new Jump(NW_SEMiddlePin - (tier - 1), NW_SEMiddlePin, PinID));
                        }
                    }

                    PinID++;
                }
            }

            return retPinList;
        }
        
        public static void InitPinJumps(List<Pin> pins, List<Jump> jumpList)
        {
            foreach (Pin p in pins)
            {
                foreach (Jump j in jumpList)
                {
                    if (j.startPin == p.ID)
                    {
                        p.Jumps.Add(j);
                    }
                }
            }
        }

        public static bool VerifyJumps(List<(ushort jumpStartSpace, ushort jumpEndSpace)> result, List<Pin> pins)
        {
            //Set open pin
            pins[result[0].jumpEndSpace - 1].Epmty = true;

            foreach((ushort jumpStartSpace, ushort jumpEndSpace) jump in result)
            {
                if(!PerformJump(jump,pins))
                {
                    return false;
                }
            }

            return true;
        }

        public static bool PerformJump((ushort jumpStartSpace, ushort jumpEndSpace) jump, List<Pin> pins)
        {
            Pin p = pins[jump.jumpStartSpace - 1];

            // Make sure it has valid jump
            foreach(Jump j in p.Jumps)
            {
                // Find jump
                if(j.startPin == jump.jumpStartSpace && j.endPin == jump.jumpEndSpace)
                {
                    // validate jump
                    if(!pins[j.startPin - 1].Epmty && !pins[j.middlePin - 1] .Epmty && pins[j.endPin - 1].Epmty)
                    {
                        // Do jump
                        pins[j.startPin - 1].Epmty = true;
                        pins[j.middlePin - 1].Epmty =  true;
                        pins[j.endPin - 1].Epmty = false;
                        return true;
                    }
                }
            }

            return false;
        }

        public class Pin
        {
            public int ID;
            public bool Epmty;
            public List<Jump> Jumps = new List<Jump>();     

            public Pin(int pinID)
            {
                ID = pinID;
                Epmty = false;
            }
        }

        public class Jump
        {
            public int startPin;
            public int middlePin;
            public int endPin;      

            public Jump(int start, int middle, int end)
            {
                startPin = start;
                middlePin = middle;
                endPin = end;
            }
        }   
    }
}
