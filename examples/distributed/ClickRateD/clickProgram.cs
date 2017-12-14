using System;
using System.IO;
using dotSpace.Interfaces.Space;
using dotSpace.Objects.Network;
using dotSpace.Objects.Space;

namespace ClickRate
{
    public class clickProgram
    {
        static void Main(string[] args)
        {
			if (args.Length < 1)
            {
                Console.WriteLine("Must specify TCP gate");
                return;
            }
			string gate = args[0];
			
            ISpace remoteSpace = new RemoteSpace("tcp://127.0.0.1:" + gate + "/tSpace?KEEP");

			var clickLogAgent = new ImpressionEntryParser("click", remoteSpace, Program.CLICK_FILE);
            clickLogAgent.Start();
        }
    }
}
