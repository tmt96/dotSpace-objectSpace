using dotSpace.BaseClasses;
using dotSpace.BaseClasses.Space;
using dotSpace.Interfaces;
using dotSpace.Interfaces.Space;
using dotSpace.Objects.Space;
using System;

namespace ObjectSpace.Example2
{
    public class SmallFoodConsumer : ObjectSpaceAgentBase
    {

        public SmallFoodConsumer(string name, IObjectSpace ts) : base(name, ts)
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
                    f = this.Get<Food>(food => food.amount <= 5);

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
