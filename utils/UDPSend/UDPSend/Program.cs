using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

namespace UDPSend
{
    class Program
    {
        static int Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("About: send an UDP packet");
                Console.WriteLine("USAGE: " + System.AppDomain.CurrentDomain.FriendlyName + " <IP> <PORT> <MSG>");
            }

            if (args.Length != 3) { Console.Error.WriteLine("ERROR: not enough arguments"); return 1; }
            if (!IPAddress.TryParse(args[0], out IPAddress IP)) { Console.Error.WriteLine("ERROR: " + args[0] + "' is not a valid IP address"); return 1; }
            if (!int.TryParse(args[1], out int PORT)) { Console.Error.WriteLine("ERROR: " + args[1] + "' is not a valid port"); return 1; }

            SendPacket(IP, PORT, args[2]);

            return 0;
        }

        private static void SendPacket(IPAddress IP, int PORT, string MSG)
        {
            Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            IPEndPoint endPoint = new IPEndPoint(IP, PORT);
            byte[] send_buffer = Encoding.ASCII.GetBytes(MSG);

            Console.WriteLine("Sending... '" + MSG + "' to " + IP + ":" + PORT);
            sock.SendTo(send_buffer, endPoint);
            Console.WriteLine("Sent.");
        }
    }
}
