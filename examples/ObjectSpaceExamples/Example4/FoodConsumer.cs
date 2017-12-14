using dotSpace.BaseClasses;
using dotSpace.BaseClasses.Space;
using dotSpace.Interfaces;
using dotSpace.Interfaces.Space;
using dotSpace.Objects.Space;
using System;

namespace ObjectSpace.Example4
{
    public class FoodConsumer : ObjectSpaceAgentBaseSimple
    {

        public FoodConsumer(string name, IObjectSpaceSimple ts) : base(name, ts)
        {
        }

        protected override void DoWork()
        {
			Console.WriteLine(name + " is awake.");
                       
            // The tuple is necessary to capture the result of a get operation
            Food f;
            try
            {
                while (true)
                {
                    // The get operation returns a tuple, that we save into t
                    f = this.Get<Food>();

                    // Note how the fields of the tuple t are accessed
                    Console.WriteLine(name + " shopping " + f.amount + " units of " + f.name + "...");
                }
            }
            catch (Exception e)
            {
				Console.WriteLine(name);
                Console.WriteLine(e.StackTrace);
            }
        }
    }
}
