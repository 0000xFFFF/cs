using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

//line sorter
namespace lort
{
    class lort
    {
        static int Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.Error.WriteLine("No input.");
                Console.WriteLine("USAGE: lort.exe <wordlistFile.lst/txt/...>");
                Console.WriteLine("");
                Console.WriteLine("");
                return 1;
            }
            
            foreach (string input in args)
            {
                if (File.Exists(input))
                {
                    List<string> lines = File.ReadAllLines(input).ToList();
                    lines = lines.Distinct().ToList(); //remove dupes
                    lines.Sort();

                    foreach (string line in lines)
                    {
                        Console.WriteLine(line);
                    }
                }
                else
                {
                    Console.Error.WriteLine("File not found: " + input);
                }
            }
            
            return 0;
        }
    }
}
