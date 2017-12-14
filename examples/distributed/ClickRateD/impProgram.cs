using System;
using System.IO;
using dotSpace.Interfaces.Space;
using dotSpace.Objects.Network;
using dotSpace.Objects.Space;

namespace ClickRate
{
    public class impProgram
    {
        static void Main(string[] args)
        {
			if (args.Length < 1)
            {
                Console.WriteLine("Must specify TCP address");
                return;
            }
			string address = args[0];
			
            ISpace remoteSpace = new RemoteSpace("tcp://" + address + "/tSpace?KEEP");

			var impressionLogAgent = new ImpressionEntryParser("imp", remoteSpace, Program.IMP_FILE);
            impressionLogAgent.Start();
        }
    }
}
