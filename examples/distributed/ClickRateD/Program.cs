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
        internal const string INPUT_END = "End of Input!";
        internal const string JOBS_FINISHED = "Jobs Finished!";
        internal const string JOBS_START = "Jobs Start";

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
            repository.AddGate("tcp://127.0.0.1:9986?KEEP");
			repository.AddGate("tcp://127.0.0.1:9987?KEEP");
			repository.AddGate("tcp://127.0.0.1:9988?KEEP");
			repository.AddGate("tcp://127.0.0.1:9985?KEEP");
			repository.AddGate("tcp://127.0.0.1:9980?KEEP");
			repository.AddGate("tcp://127.0.0.1:9981?KEEP");

            // Add a new tree space.
            repository.AddSpace("tSpace", new TreeSpace());

			// Instantiate a remotespace (a networked space) thereby connecting to the spacerepository.
            ISpace remoteSpace1 = new RemoteSpace("tcp://127.0.0.1:9986/tSpace?KEEP");
			ISpace remoteSpace2 = new RemoteSpace("tcp://127.0.0.1:9987/tSpace?KEEP");
			ISpace remoteSpace3 = new RemoteSpace("tcp://127.0.0.1:9988/tSpace?KEEP");
			ISpace remoteSpace4 = new RemoteSpace("tcp://127.0.0.1:9985/tSpace?KEEP");
			ISpace remoteSpace5 = new RemoteSpace("tcp://127.0.0.1:9980/tSpace?KEEP");
			ISpace remoteSpace6 = new RemoteSpace("tcp://127.0.0.1:9981/tSpace?KEEP");

			var impressionLogAgent1 = new ImpressionEntryParser("1", remoteSpace1, impressionFileName);
            impressionLogAgent1.Start();
			var impressionLogAgent2 = new ImpressionEntryParser("2", remoteSpace2, impressionFileName);
            impressionLogAgent1.Start();
			var impressionLogAgent3 = new ImpressionEntryParser("3", remoteSpace3, impressionFileName);
            impressionLogAgent1.Start();
			var impressionLogAgent4 = new ImpressionEntryParser("4", remoteSpace4, impressionFileName);
            impressionLogAgent1.Start();

            var clickLogAgent = new ClickEntryParser("click", remoteSpace5, clickFileName);
            clickLogAgent.Start();

			/*

            for (var i = 0; i < WORKERS_COUNT; i++)
            {
                var impressionLogAgent = new ImpressionEntryParser(i.ToString(), remoteSpace, impressionFileName);
                impressionLogAgent.Start();
            }
			*/

            var clickRateCalculator = new ClickRateCalculator(remoteSpace6, clickFileName, impressionFileName, outFileName);
            clickRateCalculator.Start();
        }
    }
}
