using System;
using System.Net;
using System.IO;

namespace discordbot
{
    class discordbot
    {
        static int Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("USAGE: discordbot.exe <file.json> <webhookurl>");
                return 1;
            }
            
            //FILE SETUP
            string file = args[0];
            if (!File.Exists(file)) { Console.WriteLine(file + " not found...");  return 1; }
            
            string json = string.Empty; // string json = "{ \"content\":\"TEST123\" }";

            //READ FILE AND PRINT
            foreach (string line in File.ReadAllLines(file)) { json = json + line; }

            if (string.IsNullOrEmpty(json)) { Console.Error.WriteLine(file + " is empty..."); return 1; }

            Console.WriteLine(json);
            
            //REQUEST
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(args[1]);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }
            
            try
            {
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    Console.WriteLine(result);
                }

            }
            catch (Exception ex) { Console.Error.WriteLine(ex.Message); return 1; }

            return 0;
        }
        
    }
}
