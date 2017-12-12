using System;
using System.Collections.Generic;
using dotSpace.BaseClasses.Space;
using dotSpace.Interfaces;
using dotSpace.Objects.Space;
using dotSpace.Objects.Network;

namespace ObjectSpace.Example3
{
    public class Program
    {

        public static int PhilosophersCount {get; set;}

        static void Main(string[] args)
        {
            PhilosophersCount = 5;
			ObjectSpaceRepository repository = new ObjectSpaceRepository();
			repository.AddGate("tcp://127.0.0.1:8989?KEEP");
			repository.AddSpace("sos", new SequentialObjectSpace());
			IObjectSpace remotespace = new RemoteObjectSpace(("tcp://127.0.0.1:8989/sos?KEEP"));

			Console.WriteLine("start");
            for (var i = 0; i < PhilosophersCount; i++)
            {
                repository.Put<Fork>("sos", new Fork{Id = i});
            }

            List<ObjectSpaceAgentBase> agents = new List<ObjectSpaceAgentBase>();
            for (var i = 0; i < PhilosophersCount; i++)
            {
                var philosopher = new Philosopher("Philosopher " + i, i, remotespace);
                philosopher.Start();
            }
        }
    }

    internal class Fork
    {
        public int Id;
    }
}
