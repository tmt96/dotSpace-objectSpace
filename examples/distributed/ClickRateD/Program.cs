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
		
		internal const string IMP_PORT1 = "9986";
		internal const string IMP_PORT2 = "9987";
		internal const string CLICK_PORT = "9988";
		internal const string CALC_PORT = "9985";

        static void Main(string[] args)
        {
			if (args.Length < 2)
			{
				Console.WriteLine("Error: Wrong number of parameters");
				Console.WriteLine("Expected: [calc|imp1|imp2|click] [IP address] ([impressions] [clicked] [out])");
			}
			
			var address = args[1];

			if (args[0] == "calc")
			{

				if (args.Length < 5)
				{
					Console.WriteLine("Error: Wrong number of parameters");
					Console.WriteLine(args[0]);
					Console.WriteLine("Expected for calculator: [calc] [IP address] [impressions] [clicked] [out]");
					return;
				}
				
				var impressionFileName = args[2];
				var clickFileName = args[3];
				var outFileName = args[4];

				// Instantiate a new Space repository.
				SpaceRepository repository = new SpaceRepository();
            
				// Add a gate, such that we can connect to it.
				repository.AddGate("tcp://" + address + ":" + IMP_PORT1 + "?KEEP");
				repository.AddGate("tcp://" + address + ":" + IMP_PORT2 + "?KEEP");
				repository.AddGate("tcp://" + address + ":" + CLICK_PORT + "?KEEP");
				repository.AddGate("tcp://" + address + ":" + CALC_PORT + "?KEEP");

				// Add a new tree space.
				repository.AddSpace("tSpace", new TreeSpace());
				
				// Instantiate a remotespace (a networked space) thereby connecting to the spacerepository.
				ISpace remoteSpace = new RemoteSpace("tcp://" + address + ":" + CALC_PORT + "/tSpace?KEEP");
				
				var clickRateCalculator = new ClickRateCalculator(remoteSpace, clickFileName, impressionFileName, outFileName);
				clickRateCalculator.Start();
			}
			else if (args[0] == "imp1")
			{
				ISpace remoteSpace = new RemoteSpace("tcp://" + address + ":" + IMP_PORT1 +  "/tSpace?KEEP");

				var impressionLogAgent = new ImpressionEntryParser("1", remoteSpace, Program.IMP_FILE);
				impressionLogAgent.Start();
			}
			else if (args[0] == "imp2")
			{
				ISpace remoteSpace = new RemoteSpace("tcp://" + address + ":" + IMP_PORT2 +  "/tSpace?KEEP");

				var impressionLogAgent = new ImpressionEntryParser("2", remoteSpace, Program.IMP_FILE);
				impressionLogAgent.Start();
			}
			else if (args[0] == "click")
			{
				ISpace remoteSpace = new RemoteSpace("tcp://" + address + ":" + CLICK_PORT + "/tSpace?KEEP");

				var clickLogAgent = new ClickEntryParser("click", remoteSpace, Program.CLICK_FILE);
				clickLogAgent.Start();
			}
			else
			{
				Console.WriteLine("Please specify [calc|imp1|imp2|click]");
			}
        }
    }
}
