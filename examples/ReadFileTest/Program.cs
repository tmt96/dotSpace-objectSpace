using System;
using System.IO;
using dotSpace.Interfaces.Space;
using dotSpace.Objects.Space;

namespace ReadFileTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var fileName = args[0];
            var space = new SequentialSpace();
            try 
            {
                using (StreamReader sr = new StreamReader(fileName)) 
                {
                    string line;
                    int i = 0;
                    while ((line = sr.ReadLine()) != null) 
                    {
                        i++;
                        space.Put(i, line);
                    }
                    for (; i > 0; i--) {
                        space.Get(i, typeof(string));
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
;
        }
    }
}
