using System;
using System.Collections.Generic;
using dotSpace.BaseClasses.Space;
using dotSpace.Interfaces;
using dotSpace.Objects.Space;
using dotSpace.Objects.Network;

namespace ObjectSpace.ProducerConsumerD
{
    public class Program
    {
        static void Main(string[] args)
        {
            var ipAddress = args[1];
            switch (args[0])
            {
                case "producer":
                    ObjectSpaceRepository repository = new ObjectSpaceRepository();
                    repository.AddGate("tcp://" + ipAddress + ":8989?KEEP");
                    repository.AddGate("tcp://" + ipAddress + ":8988?KEEP");
                    repository.AddGate("tcp://" + ipAddress + ":8987?KEEP");

                    repository.AddSpace("sos", new SequentialObjectSpace());
                    IObjectSpaceSimple remotespace3 = new RemoteObjectSpace(("tcp://" + ipAddress + ":8987/sos?KEEP"));

                    Console.WriteLine("Alice adding items to the grocery list...");

                    Console.WriteLine("Alice adding bananas");
                    repository.Put("sos", new Food { name = "bananas", amount = 3 });

                    Console.WriteLine("Alice adding apples");
                    repository.Put("sos", new Food { name = "apples", amount = 7 });

                    Console.WriteLine("Alice adding oranges");
                    repository.Put("sos", new Food { name = "oranges", amount = 10 });

                    Console.WriteLine("Alice adding grapes");
                    repository.Put("sos", new Food { name = "grapes", amount = 20 });

                    Console.WriteLine("Alice adding strawberries");
                    repository.Put("sos", new Food { name = "strawberries", amount = 30 });

                    Console.WriteLine("Alice adding soap");
                    repository.Put("sos", new Item { name = "soap" });

                    Console.WriteLine("Alice adding toothpaste");
                    repository.Put("sos", new Item { name = "toothpaste" });

                    Console.WriteLine("Alice adding charger");
                    repository.Put("sos", new Item { name = "charger" });

                    break;
                case "consumer":
                    IObjectSpaceSimple remotespace1 = new RemoteObjectSpace(("tcp://" + ipAddress + ":8989/sos?KEEP"));
                    var consumer = new Consumer("Bob", remotespace1);
                    consumer.Start();
                    break;
                case "food":
                    IObjectSpaceSimple remotespace2 = new RemoteObjectSpace(("tcp://" + ipAddress + ":8988/sos?KEEP"));
                    var foodConsumer = new FoodConsumer("Charlie", remotespace2);
                    foodConsumer.Start();
                    break;
                default:
                    Environment.Exit(1);
                    break;
            }
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
