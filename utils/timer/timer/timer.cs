using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace timer
{
    class timer
    {
        static int Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("USAGE: timer <program to time>");
                return 1;
            }

            string argsStr = "";
            for (int i = 1; i < args.Length; i++) { argsStr += args[i] + " "; }
            
            Process proc = new Process()
            {
                StartInfo = new ProcessStartInfo()
                {
                    FileName = args[0],
                    Arguments = argsStr,
                    UseShellExecute = false
                }
            };

            DateTime currentTime = DateTime.Now;

            proc.Start();
            proc.WaitForExit();

            Console.WriteLine("\n\n\nTIME: " + (DateTime.Now - currentTime).TotalMilliseconds.ToString() + " ms");

            int ret = proc.ExitCode;
            proc.Dispose();

            return ret;
        }
    }
}
