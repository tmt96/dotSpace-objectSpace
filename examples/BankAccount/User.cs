using dotSpace.BaseClasses;
using dotSpace.BaseClasses.Space;
using dotSpace.Interfaces;
using dotSpace.Interfaces.Space;
using System;

namespace BankAccount
{
    public class User : AgentBase
    {
        public User(string name, ISpace space) : base(name, space)
        {
        }

        protected override void DoWork()
        {

	    Console.WriteLine(name + " starting");

	    int value;
	    string action;

	    if (name == "A")
	    {
		value = 100;
		action = "Deposit";
	    }
	    else
	    {
		value = -100;
		action = "Withdraw";
	    }
		
	    for (int i = 0; i < 100; i++)
	    {
	        ITuple tuple = this.Get(typeof(int));
		Console.WriteLine("Current value: {0}", tuple[0]);
		Console.WriteLine(action);
		this.Put((int)tuple[0] + value);
	    }

            if (name == "A")
	    {
		Console.WriteLine(action + " done");
	    }
	    else
	    {
		Console.WriteLine(action + " done");
	    }
           
        }
    }
}
