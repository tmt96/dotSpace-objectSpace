using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using dotSpace.BaseClasses.Space;
using dotSpace.Interfaces.Space;
using dotSpace.Objects.Space;

namespace ClickRate
{
    class ClickRateCalculator : AgentBase
    {
        private string clickFileName;
        private string impressionFileName;
        private string outFileName;

        public ClickRateCalculator(ISpace tSpace, string clickFileName, string impressionFileName, string outFileName) : base(string.Empty, tSpace)
        {
            this.outFileName = outFileName;
            this.clickFileName = clickFileName;
            this.impressionFileName = impressionFileName;
        }

        protected override void DoWork()
        {
            ReadAndSendToSpace(clickFileName);
            ReadAndSendToSpace(impressionFileName);
            for (int i = 0; i < Program.WORKERS_COUNT; i++)
            {
                Put(impressionFileName, Program.INPUT_END);
            }
            Put(clickFileName, Program.INPUT_END);
            Summarize();
            Environment.Exit(0);
        }

        private void Summarize()
        {
            Get(Program.JOBS_FINISHED);
            var allAdsInfo = GetAll(typeof(string), typeof(string), typeof(int), typeof(int));

            using (StreamWriter sw = new StreamWriter(outFileName, false, Encoding.UTF8, 0x10000))
            {
                Console.WriteLine("start writing");
                foreach (var adInfo in allAdsInfo)
                {
                    var clickRate = (double)((int)adInfo[3]) / (double)((int)adInfo[2]);
                    var line = 
                        (string) adInfo[1] + " " + 
                        (string) adInfo[0] + "\t" +
                        clickRate;
                    sw.WriteLine(line);
                }
                Console.WriteLine("finish job");
            }
        }

        private void ReadAndSendToSpace(string fileName)
        {
            try
            {
                using (StreamReader sr = new StreamReader(fileName))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        Put(fileName, line);
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
