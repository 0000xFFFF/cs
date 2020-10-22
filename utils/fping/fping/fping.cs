using System;
using System.Diagnostics;
using System.Threading;
using System.Net.NetworkInformation;
using System.Net;
using System.Linq;

namespace fping
{
    class fping
    {
        private static int count = 0;
        private static int fails = 0;
        static object lockObj = new object();

        static int Main(string[] args)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            foreach (NetworkInterface iface in NetworkInterface.GetAllNetworkInterfaces())
            {
                foreach(GatewayIPAddressInformation gate in iface.GetIPProperties().GatewayAddresses)
                {
                    scan(get_cut_up_gateway(gate.Address.ToString()));
                }
            }

            sw.Stop();
            Console.WriteLine("");
            Console.WriteLine(": elapsed..: " + sw.ElapsedMilliseconds + " ms");
            Console.WriteLine(": active...: " + count + " hosts");
            Console.WriteLine(": fails....: " + fails);
            return 0;
        }

        private static string get_cut_up_gateway(string gateway)
        {
            int dots = gateway.Count(f => (f == '.'));
            string cut_up_gateway = "";

            //extract gateway without the end bit
            int count = 0;
            foreach (char ch in gateway)
            {
                if (ch == '.') { count++; }
                cut_up_gateway = cut_up_gateway + ch;
                if (count == dots) { break; }
            }

            return cut_up_gateway;
        }

        private static void scan(string gateway)
        {
            for (int i = 1; i < 255; i++)
            {
                IPAddress ip;
                if (!IPAddress.TryParse(gateway + i.ToString(), out ip)) { return; }

                Ping ping = new Ping();
                ping.PingCompleted += new PingCompletedEventHandler((object sender, PingCompletedEventArgs e) =>
                {
                    lock (lockObj)
                    {
                        if (e.Reply == null)
                        {
                            Console.WriteLine("pinging " + e.Reply.Address + " failed.");
                            fails++;
                        }
                        else if (e.Reply.Status == IPStatus.Success)
                        {
                            Console.WriteLine(e.Reply.Address + " (" + e.Reply.RoundtripTime + " ms)");
                            count++;
                        }
                    }
                });

                try { ping.SendAsync(ip, 300, ip); }
                catch (Exception) { }
            }
        }
    }
}