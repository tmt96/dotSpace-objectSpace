using System;
using System.Collections.Generic;
using dotSpace.BaseClasses.Space;
using dotSpace.Interfaces;
using dotSpace.Objects.Space;

namespace ObjectSpace.Fork
{
    public class Program : ObjectSpaceAgentBase
    {
        public Program(string name, IObjectSpace space) : base(name, space)
        {
        }

        public static int PhilosophersCount {get; set;}

        protected override void DoWork()
        {
            Console.WriteLine("start");
            for (var i = 0; i < PhilosophersCount; i++)
            {
                Put<Fork>(new Fork{Id = i});
            }
        }

        static void Main(string[] args)
        {
            PhilosophersCount = 5;
            IObjectSpace space = new SequentialObjectSpace();
            var program = new Program("main", space);
            program.Start();

            List<ObjectSpaceAgentBase> agents = new List<ObjectSpaceAgentBase>();
            for (var i = 0; i < PhilosophersCount; i++)
            {
                var philosopher = new Philosopher("Philosopher " + i, i, space);
                philosopher.Start();
            }
        }
    }

    internal class Fork
    {
        public int Id;
    }
}
