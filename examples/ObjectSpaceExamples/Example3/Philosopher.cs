using System;
using dotSpace.BaseClasses.Space;
using dotSpace.Interfaces;

namespace ObjectSpace.Example3
{
    public class Philosopher : ObjectSpaceAgentBase
    {
        private int seatIndex;
        private int leftForkId;
        private int rightForkId;

        public Philosopher(string name, int seatIndex, IObjectSpace space) : base(name, space)
        {
            this.seatIndex = seatIndex;
            this.leftForkId = seatIndex;
            this.rightForkId = seatIndex == Program.PhilosophersCount - 1 ? 0 : seatIndex + 1;
        }

        protected override void DoWork()
        {
            try
            {
                while (true)
                {
                    var lf = Get<Fork>(f => f.Id == leftForkId);
                    var rf = GetP<Fork>(f => f.Id == rightForkId);

                    if (rf != null)
                    {
                        Console.WriteLine(this.name + ": I AM EATING WITH BOTH MY HANDS: " + this.seatIndex);
                        Put<Fork>(rf);
                        Put<Fork>(lf);
                        Console.WriteLine("Done eating: " + this.seatIndex);
                    }
                    else
                    {
                        Put<Fork>(lf);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }
    }
}
