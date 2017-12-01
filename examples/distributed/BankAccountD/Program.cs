using dotSpace.BaseClasses;
using dotSpace.BaseClasses.Space;
using dotSpace.Interfaces;
using dotSpace.Interfaces.Space;
using dotSpace.Objects.Network;
using dotSpace.Objects.Space;
using System;

namespace BankAccount
{
	class Program
	{
		static void Main(string[] args)
		{

			// Instantiate a new Space repository.
            SpaceRepository repository = new SpaceRepository();
            
            // Add a gate, such that we can connect to it.
            repository.AddGate("tcp://127.0.0.1:9876?KEEP");

            // Add a new Fifo based space.
            repository.AddSpace("dtu", new SequentialSpace());

			Console.WriteLine("Starting");
			repository.Put("dtu", 0);

			// Instantiate a remotespace (a networked space) thereby connecting to the spacerepository.
            ISpace remotespace = new RemoteSpace("tcp://127.0.0.1:9876/dtu?KEEP");
			
			// Instantiate a new agent, assign the tuple space and start it.
			AgentBase userA = new User("A", remotespace);
			AgentBase userB = new User("B", remotespace);
			userA.Start();
			userB.Start();
		}
	}
}
