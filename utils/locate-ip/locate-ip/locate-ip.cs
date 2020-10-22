using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Globalization;
using System.Net;

namespace locate_ip
{
    class locate_ip
    {
        static int Main(string[] args)
        {
            if (args.Length == 0) { Console.WriteLine("input an ip"); return 1; }


            foreach (string arg in args)
            {
                try
                {
                    IpInfo ipInfo = new IpInfo();
                    string info = new WebClient().DownloadString("http://ipinfo.io/" + arg);
                    ipInfo = JsonConvert.DeserializeObject<IpInfo>(info);
                    RegionInfo myRI1 = new RegionInfo(ipInfo.Country);

                    Console.WriteLine("ip.......: " + ipInfo.Ip);
                    Console.WriteLine("hostname.: " + ipInfo.Hostname);
                    Console.WriteLine("city.....: " + ipInfo.City);
                    Console.WriteLine("region...: " + ipInfo.Region);
                    Console.WriteLine("country..: " + ipInfo.Country);
                    Console.WriteLine("loc......: " + ipInfo.Loc);
                    Console.WriteLine("org......: " + ipInfo.Org);
                    Console.WriteLine("postal...: " + ipInfo.Postal);
                    Console.WriteLine("");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }


            return 0;
        }

        public class IpInfo
        {

            [JsonProperty("ip")]
            public string Ip { get; set; }

            [JsonProperty("hostname")]
            public string Hostname { get; set; }

            [JsonProperty("city")]
            public string City { get; set; }

            [JsonProperty("region")]
            public string Region { get; set; }

            [JsonProperty("country")]
            public string Country { get; set; }

            [JsonProperty("loc")]
            public string Loc { get; set; }

            [JsonProperty("org")]
            public string Org { get; set; }

            [JsonProperty("postal")]
            public string Postal { get; set; }
        }
    }
}
