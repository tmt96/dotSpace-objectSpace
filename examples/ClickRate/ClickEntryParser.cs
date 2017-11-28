using System;
using dotSpace.BaseClasses.Space;
using dotSpace.Interfaces.Space;
using Newtonsoft.Json;

namespace ClickRate
{
    public class ClickEntryParser : AgentBase
    {
        private readonly string fileName;

        public ClickEntryParser(String name, ISpace space, string fileName) : base(name, space) => this.fileName = fileName;

        protected override void DoWork()
        {
            var endSignal = false;
            while (true) 
            {
                var tuple = GetP(fileName, typeof(string));
                if (tuple == null)
                {
                    if (!endSignal)
                    {
                        continue;
                    }
                    else
                    {
                        Console.WriteLine("end");
                        Put(Program.JOBS_FINISHED);
                        return;
                    }
                }
                else if (((string)tuple[1]).Equals(Program.INPUT_END))
                {
                    endSignal = true;
                    continue;
                }
                else
                {
                    var (adID, referrerUrl) = getClickEntryAdInfo((string) tuple[1]);
                    UpdateAdImpressionAndClickCounts(adID, referrerUrl);
                }
            }
        }

        private (string, string) getClickEntryAdInfo(string clickEntry)
        {
            var ClickEntry = new { impressionId = "" };
            
            var entry = JsonConvert.DeserializeAnonymousType(clickEntry, ClickEntry);
            var adInfo = Get(entry.impressionId, typeof(string), typeof(string));
            return ((string) adInfo[1], (string) adInfo[2]);
        }

        private void UpdateAdImpressionAndClickCounts(string adID, string referrerUrl)
        {
            var impressionCount = 0; 
            var clickCount = 1;
            var allTuplesOfAd = GetAll(adID, referrerUrl, typeof(int), typeof(int));

            foreach (var tuple in allTuplesOfAd) 
            {
                impressionCount += (int) tuple[2];
                clickCount += (int) tuple[3];
            }
            Put(adID, referrerUrl, impressionCount, clickCount);
        }
    }
}