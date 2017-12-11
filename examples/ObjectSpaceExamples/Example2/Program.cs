using System;
using System.Collections.Generic;
using dotSpace.BaseClasses.Space;
using dotSpace.Interfaces;
using dotSpace.Objects.Space;

namespace ObjectSpace.Example2
{
    public class Program
    {
		static void Main(string[] args)
		{
		    IObjectSpace ospace = new SequentialObjectSpace();
			List<ObjectSpaceAgentBase> agents = new List<ObjectSpaceAgentBase>();

            agents.Add(new Consumer("Bob", ospace));
            agents.Add(new FoodConsumer("Charlie", ospace));
            agents.Add(new SmallFoodConsumer("Dave", ospace));
			
            agents.ForEach(a => a.Start());

			Console.WriteLine("Alice adding items to the grocery list...");

			Console.WriteLine("Alice adding bananas");
			ospace.Put(new Food{name = "bananas", amount = 3});
			
			Console.WriteLine("Alice adding apples");
			ospace.Put(new Food{name = "apples", amount = 7});

			Console.WriteLine("Alice adding oranges");
			ospace.Put(new Food{name = "oranges", amount = 10});

			Console.WriteLine("Alice adding grapes");
			ospace.Put(new Food{name = "grapes", amount = 20});

			Console.WriteLine("Alice adding strawberries");
			ospace.Put(new Food{name = "strawberries", amount = 30});
			
			Console.WriteLine("Alice adding soap");
			ospace.Put(new Item{name = "soap"});
			
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
