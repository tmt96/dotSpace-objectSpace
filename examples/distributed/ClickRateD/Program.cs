using System;
using System.IO;
using dotSpace.Interfaces.Space;
using dotSpace.Objects.Network;
using dotSpace.Objects.Space;

namespace ClickRate
{
    public class Program
    {
        internal const int WORKERS_COUNT = 4;
		internal const string IMP_FILE = "impression file";
		internal const string CLICK_FILE = "click file";
        internal const string INPUT_END = "End of Input!";
        internal const string JOBS_FINISHED = "Jobs Finished!";
        internal const string JOBS_START = "Jobs Start";
		
		internal const string IMP_GATE1 = "9986";
		internal const string IMP_GATE2 = "9987";
		internal const string CLICK_GATE = "9988";
		internal const string CALC_GATE = "9985";

        static void Main(string[] args)
        {
            if (args.Length < 3)
            {
                Console.WriteLine("Error: Wrong number of parameters");
                Console.WriteLine(args[0]);
                Console.WriteLine("Expected: [impressions] [clicked] [out]");
                return;
            }
            var impressionFileName = args[0];
            var clickFileName = args[1];
            var outFileName = args[2];

			// Instantiate a new Space repository.
            SpaceRepository repository = new SpaceRepository();
            
            // Add a gate, such that we can connect to it.
            repository.AddGate("tcp://127.0.0.1:" + IMP_GATE1 + "?KEEP");
			repository.AddGate("tcp://127.0.0.1:" + IMP_GATE2 + "?KEEP");
			repository.AddGate("tcp://127.0.0.1:" + CLICK_GATE + "?KEEP");
			repository.AddGate("tcp://127.0.0.1:" + CALC_GATE + "?KEEP");

            // Add a new tree space.
            repository.AddSpace("tSpace", new TreeSpace());

			// Instantiate a remotespace (a networked space) thereby connecting to the spacerepository.
			ISpace remoteSpace = new RemoteSpace("tcp://127.0.0.1:" + CALC_GATE + "/tSpace?KEEP");

            var clickRateCalculator = new ClickRateCalculator(remoteSpace, clickFileName, impressionFileName, outFileName);
            clickRateCalculator.Start();
        }
    }
}
