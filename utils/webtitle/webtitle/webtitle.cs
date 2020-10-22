using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Net;
using System.IO;

namespace webtitle
{
    class webtitle
    {
        static int Main(string[] args)
        {
            if (args.Length == 0) { Console.Error.WriteLine("USAGE: webtitle <url>"); return 1; }

            foreach (string url in args)
            {
                Console.WriteLine(get_html_title(url));
            }

            return 0;
        }

        public static string get_html_title(string url)
        {
            try
            {
                if (string.IsNullOrEmpty(url)) { return ""; }
                if (!Uri.IsWellFormedUriString(url, UriKind.Absolute)) { return ""; }

                System.Net.WebClient wc = new System.Net.WebClient();
                wc.Headers.Add("user-agent", "fapmap.exe");

                string titleUNICODE = System.Net.WebUtility.HtmlDecode(get_string_in_between("<title>", "</title>", System.Text.Encoding.Default.GetString(wc.DownloadData(url)), false, false));
                string title = System.Text.Encoding.ASCII.GetString(System.Text.Encoding.ASCII.GetBytes(titleUNICODE));
                return title;
            }
            catch (Exception e) { return ("ERROR: " + e.Message); }
        }
        public static string get_string_in_between(string strBegin, string strEnd, string strSource, bool includeBegin, bool includeEnd)
        {
            string[] result = { string.Empty, string.Empty };
            int iIndexOfBegin = strSource.IndexOf(strBegin);

            if (iIndexOfBegin != -1)
            {
                // include the Begin string if desired 
                if (includeBegin) { iIndexOfBegin -= strBegin.Length; }

                strSource = strSource.Substring(iIndexOfBegin + strBegin.Length);

                int iEnd = strSource.IndexOf(strEnd);
                if (iEnd != -1)
                {
                    // include the End string if desired 
                    if (includeEnd) { iEnd += strEnd.Length; }
                    result[0] = strSource.Substring(0, iEnd);

                    // advance beyond this segment 
                    if (iEnd + strEnd.Length < strSource.Length) { result[1] = strSource.Substring(iEnd + strEnd.Length); }
                }
            }
            else
            {
                // stay where we are 
                result[1] = strSource;
            }

            return result[0];
        }

    }
}