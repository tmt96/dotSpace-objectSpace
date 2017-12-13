using System;
using System.Collections.Generic;
using dotSpace.BaseClasses.Space;
using dotSpace.Interfaces;
using dotSpace.Objects.Space;
using dotSpace.Objects.Network;

namespace ObjectSpace.Example4
{
    public class Program
    {
		static void Main(string[] args)
		{

			ObjectSpaceRepository repository = new ObjectSpaceRepository();
			repository.AddGate("tcp://127.0.0.1:8989?KEEP");
            repository.AddGate("tcp://127.0.0.1:8988?KEEP");
            repository.AddGate("tcp://127.0.0.1:8987?KEEP");

            repository.AddSpace("sos", new SequentialObjectSpace());
			IObjectSpaceSimple remotespace1 = new RemoteObjectSpace(("tcp://127.0.0.1:8989/sos?KEEP"));
			IObjectSpaceSimple remotespace2 = new RemoteObjectSpace(("tcp://127.0.0.1:8988/sos?KEEP"));
			IObjectSpaceSimple remotespace3 = new RemoteObjectSpace(("tcp://127.0.0.1:8987/sos?KEEP"));

			List<ObjectSpaceAgentBaseSimple> agents = new List<ObjectSpaceAgentBaseSimple>();

            agents.Add(new Consumer("Bob", remotespace1));
            agents.Add(new FoodConsumer("Charlie", remotespace2));
            agents.Add(new FoodConsumer("Dave", remotespace3));
			
            agents.ForEach(a => a.Start());

			Console.WriteLine("Alice adding items to the grocery list...");

			Console.WriteLine("Alice adding bananas");
		    repository.Put("sos", new Food{name = "bananas", amount = 3});
			
			Console.WriteLine("Alice adding apples");
			repository.Put("sos", new Food{name = "apples", amount = 7});

			Console.WriteLine("Alice adding oranges");
			repository.Put("sos", new Food{name = "oranges", amount = 10});

			Console.WriteLine("Alice adding grapes");
			repository.Put("sos", new Food{name = "grapes", amount = 20});

			Console.WriteLine("Alice adding strawberries");
			repository.Put("sos", new Food{name = "strawberries", amount = 30});
			
			Console.WriteLine("Alice adding soap");
			repository.Put("sos", new Item{name = "soap"});
			
		}
	}

	public class Item
	{
		public string name;
	}

	public class Food : Item
	{
		public int amount;
	}
}
