using dotSpace.BaseClasses;
using dotSpace.BaseClasses.Space;
using dotSpace.Interfaces;
using dotSpace.Interfaces.Space;
using dotSpace.Objects.Network;
using dotSpace.Objects.Space;
using System;
using System.Collections.Generic;

namespace Example6
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length > 1)
            {
                if (args[0] == "table")
                {
                    // Instantiate a new space repository and add a gate to it.
                    SpaceRepository repository = new SpaceRepository();
                    repository.AddGate("tcp://" + args[1] + ":31415?KEEP");
		    repository.AddGate("tcp://" + args[1] + ":31416?KEEP");
		    repository.AddGate("tcp://" + args[1] + ":31417?KEEP");
		    repository.AddGate("tcp://" + args[1] + ":31418?KEEP");
		    repository.AddGate("tcp://" + args[1] + ":31419?KEEP");
		    

                    // Add a new Fifo based space to the repository.
                    repository.AddSpace("DiningTable", new SequentialSpace());
                    
                    // Insert the forks that the philosophers must share.
                    repository.Put("DiningTable", "FORK", 1);
                    repository.Put("DiningTable", "FORK", 2);
                    repository.Put("DiningTable", "FORK", 3);
                    repository.Put("DiningTable", "FORK", 4);
                    repository.Put("DiningTable", "FORK", 5);
                    return;
                }
                else if (args[0] == "philosopher")
                {
			Console.WriteLine("connecting "+ args[1]);

			// Instantiate a new remote space, thereby allowing a persistant networked connection to the repository.
                    ISpace remotespace1 = new RemoteSpace("tcp://" + args[1]+ ":31415/DiningTable?KEEP");
		    ISpace remotespace2 = new RemoteSpace("tcp://" + args[1]+ ":31416/DiningTable?KEEP");
		    ISpace remotespace3 = new RemoteSpace("tcp://" + args[1]+ ":31417/DiningTable?KEEP");
		    ISpace remotespace4 = new RemoteSpace("tcp://" + args[1]+ ":31418/DiningTable?KEEP");
		    ISpace remotespace5 = new RemoteSpace("tcp://" + args[1]+ ":31419/DiningTable?KEEP");

                    // Instantiate the philosopher agents and let them use the same connection to access the repository. 
                    List<AgentBase> agents = new List<AgentBase>();
                    agents.Add(new Philosopher("Alice", 1, 5, remotespace1));
                    agents.Add(new Philosopher("Charlie", 2, 5, remotespace2));
                    agents.Add(new Philosopher("Bob", 3, 5, remotespace3));
                    agents.Add(new Philosopher("Dave", 4, 5, remotespace4));
                    agents.Add(new Philosopher("Homer", 5, 5, remotespace5));
                    
                    // Let the philosophers eat.
                    agents.ForEach(a => a.Start());
                    Console.Read();
                    return;
                }
            }
            Console.WriteLine("Please specify [table|philosopher] and IP address to use");
            Console.Read();

        }
    }
}
