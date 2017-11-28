using System;
using System.IO;
using dotSpace.Interfaces.Space;
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

            var tSpace = new TreeSpace();
            
            var clickLogAgent = new ClickEntryParser("click", tSpace, clickFileName);
            clickLogAgent.Start();

            for (var i = 0; i < WORKERS_COUNT; i++)
            {
                var impressionLogAgent = new ImpressionEntryParser(i.ToString(), tSpace, impressionFileName);
                impressionLogAgent.Start();
            }

            var clickRateCalculator = new ClickRateCalculator(tSpace, clickFileName, impressionFileName, outFileName);
            clickRateCalculator.Start();
        }

    }
}