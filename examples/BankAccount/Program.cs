using dotSpace.BaseClasses;
using dotSpace.BaseClasses.Space;
using dotSpace.Interfaces;
using dotSpace.Interfaces.Space;
using dotSpace.Objects.Space;
using System;

namespace BankAccount
{
    class Program
    {
        static void Main(string[] args)
        {

            // Instantiate a new Fifobased tuple space.
            ISpace dtu = new SequentialSpace();
            
            // Instantiate a new agent, assign the tuple space and start it.
            AgentBase userA = new User("A", dtu);
	    AgentBase userB = new User("B", dtu);
            userA.Start();
	    userB.Start();

	    Console.WriteLine("Starting");
	    dtu.Put(0);
        }
    }
}
