using System;
using System.IO;
using dotSpace.Interfaces.Space;
using dotSpace.Objects.Space;

namespace ClickRate
{
    public class Program
    {
        internal const int WORKERS_COUNT = 3;

        internal const string INPUT_END = "End of Input!";
        internal const string JOBS_FINISHED = "Jobs Finished!";

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

            var tSpace = new SequentialSpace();
            ReadAndSendToSpace(tSpace, clickFileName);
            ReadAndSendToSpace(tSpace, impressionFileName);

            var clickLogAgent = new ClickEntryParser("click", tSpace, clickFileName);
            clickLogAgent.Start();
            tSpace.Put(clickFileName, INPUT_END);

            for (var i = 0; i < WORKERS_COUNT; i++)
            {
                var impressionLogAgent = new ImpressionEntryParser(i.ToString(), tSpace, impressionFileName);
                impressionLogAgent.Start();
                Console.WriteLine("start");
                tSpace.Put(impressionFileName, INPUT_END);
            }

            var clickRateCalculator = new ClickRateCalculator(tSpace, outFileName);
            clickRateCalculator.Start();
        }

        private static void ReadAndSendToSpace(ISpace space, string fileName)
        {
            try
            {
                using (StreamReader sr = new StreamReader(fileName))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        space.Put(fileName, line);
                    }
                }
            }
            catch (Exception e)
            {
                // Let the user know what went wrong.
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
                Environment.Exit(1);
            }
        }
    }
}