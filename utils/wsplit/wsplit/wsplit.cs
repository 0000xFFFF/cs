using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace wsplit
{
    class wsplit
    {
        static int Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("USAGE: wsplit.exe <wordlistFile.lst/txt/...>");
                return 1;
            }

            List<string> words = new List<string>();

            foreach (string input in args)
            {
                if (!File.Exists(input)) { Console.Error.WriteLine("File not found: " + input); continue; }

                foreach (string line in File.ReadAllLines(input))
                {
                    if (!line.Contains(' ') && !string.IsNullOrEmpty(line))
                    {
                        words.Add(line);
                        continue;
                    }

                    string[] index = line.Split(' ');
                    foreach (string word in index)
                    {
                        if (!string.IsNullOrEmpty(word))
                        {
                            words.Add(word);
                        }
                    }
                }
            }

            words = words.Distinct().ToList();
            words.Sort();

            // print
            foreach (string word in words) { Console.WriteLine(word); }
            
            return 0;
        }
    }
}
