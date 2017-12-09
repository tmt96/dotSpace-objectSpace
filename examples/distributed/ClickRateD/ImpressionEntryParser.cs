using System;
using dotSpace.BaseClasses.Space;
using dotSpace.Interfaces.Space;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ClickRate
{
    public class ImpressionEntryParser : AgentBase
    {
        private readonly string fileName;

        public ImpressionEntryParser(String name, ISpace space, string fileName) : base(name, space) => this.fileName = fileName;

        protected override void DoWork()
        {
			Console.WriteLine("Start impression parser");
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
                        return;
                    }
                }
                else if (((string) tuple[1]).Equals(Program.INPUT_END))
                {
                    endSignal = true;
                    continue;
                }
                else
                {
                    var jObject = JObject.Parse((string) tuple[1]);
                    var impressionId = jObject["impressionId"].ToString();
                    var adId = jObject["adId"].ToString();
                    var referrer = jObject["referrer"].ToString();

                    UpdateAdImpressionAndClickCounts(adId, referrer);
                    Put(impressionId, adId, referrer);
                }
            }
        }

        private void UpdateAdImpressionAndClickCounts(string adID, string referrerUrl)
        {
            var impressionCount = 1; 
            var clickCount = 0;
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
