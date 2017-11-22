using System;
using System.IO;
using dotSpace.BaseClasses.Space;
using dotSpace.Objects.Space;

namespace ClickRate
{
    class ClickRateCalculator : AgentBase
    {
        private string outFileName;

        public ClickRateCalculator(SequentialSpace tSpace, string outFileName) : base(string.Empty, tSpace)
        {
            this.outFileName = outFileName;
        }

        protected override void DoWork()
        {
            Get(Program.JOBS_FINISHED);
            var allAdsInfo = GetAll(typeof(string), typeof(string), typeof(int), typeof(int));

            using (StreamWriter sw = new StreamWriter(outFileName))
            {
                foreach (var adInfo in allAdsInfo)
                {
                    var line = string.Format(
                        "{0}, {1}\t{2}",
                        (string) adInfo[0],
                        (string) adInfo[1],
                        (double) adInfo[2] / (double) adInfo[3]);
                    sw.WriteLine(line);
                }
            }
        }
    }
}
