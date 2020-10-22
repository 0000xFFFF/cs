using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace UDPListen
{
    class Program
    {
        private static bool verbose = false;
        static int Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("About: Capture UDP packets on a specifed IP and PORT.");
                Console.WriteLine("USAGE: " + System.AppDomain.CurrentDomain.FriendlyName + " <IP> <PORT> <OPTION>");
                Console.WriteLine("OPTIONS:");
                Console.WriteLine(" -v = verbose output");
                return 1;
            }

            if (args.Length < 2)
            {
                Console.Error.WriteLine("ERROR: missing arguments");
                return 1;
            }

            if (args.Length == 3) { if ((args[2]) == "-v") { verbose = true; } }

            IPAddress ip;
            if (args[0] == "any") { ip = IPAddress.Any; }
            else if (!IPAddress.TryParse(args[0], out ip))
            {
                Console.Error.WriteLine("ERROR: '" + args[0] + "' is not a valid IP");
                return 1;
            }

            if (!int.TryParse(args[1], out int port))
            {
                Console.Error.WriteLine("ERROR: '" + args[0] + "' is not a valid port");
                return 1;
            }

            listen(ip, port);

            return 0;
        }
        
        private static void listen(IPAddress ip, int PORT)
        {
            UdpClient listener = new UdpClient(PORT);
            IPEndPoint groupEP = new IPEndPoint(ip, PORT);
            string received_data;
            byte[] receive_byte_array;
            try
            {
                if (verbose == true)
                {
                    Console.WriteLine("listening on [" + (ip == IPAddress.Any ? "ANY" : ip.ToString()) + "] " + PORT + " ...");
                }

                while (true)
                {
                    receive_byte_array = listener.Receive(ref groupEP); //LISTEN
                    if (verbose == true) { Console.WriteLine("IPv4: {0}", groupEP.ToString()); }
                    received_data = Encoding.ASCII.GetString(receive_byte_array, 0, receive_byte_array.Length);
                    if (verbose == true) { Console.WriteLine("Data: {0}", received_data); } else { Console.WriteLine(received_data); }
                }
            }
            catch (Exception e) { Console.Error.WriteLine("ERROR: " + e.Message); }

            listener.Close();
        }
    }
}
