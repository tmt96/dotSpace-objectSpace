using System;
using System.IO;
using System.Linq;
using dotSpace.Interfaces.Space;
using dotSpace.Objects.Space;

namespace ReadFileTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var fileName = args[0];
            // var space = new SequentialSpace();
            var treeSpace = new TreeSpace();
            try 
            {
                using (StreamReader sr = new StreamReader(fileName)) 
                {
                    string line;
                    int i = 0;
                    while ((line = sr.ReadLine()) != null) 
                    {
                        i++;
                        // space.Put(i, line);
                        treeSpace.Put(line);
                        // treeSpace.GetP(typeof(string));

                    }
                    Console.WriteLine(i);
                    // for (; i > 0; i--) {
                        // space.Get(i, typeof(string));
                        // treeSpace.QueryP(typeof(string));
                        // treeSpace.GetP(typeof(string));
                        // Console.WriteLine(res[1]);
                    // }
                    var resList  = treeSpace.GetAll(typeof(string));
                    // foreach (var res in resList)        
                    // {
                    //     Console.WriteLine(res[0]);
                    // }
                    var res = String.Join('\n', resList.Select(str => str[0]));
                    Console.WriteLine(res);
                    // tree.QueryAll(typeof(int), typeof(string));
                }
            }
            catch (Exception e) 
            {
                // Let the user know what went wrong.
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                Environment.Exit(1);
            }
        }
    }
}
