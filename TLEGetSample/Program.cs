using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TLEGetSample
{
    class Program
    {
        static void Main(string[] args)
        {
            var strUrl = "https://portalvhds4wdrc7dngb89n.blob.core.windows.net/satellites/TLE.TXT";
            var client = new WebClient();
            var stream = client.OpenRead(strUrl);
            var reader = new StreamReader(stream);
            string[] line0 = {""}, line1 = {""}, line2={""};
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                if (String.IsNullOrWhiteSpace(line))
                {
                    continue;
                }
                var arrayLine = line.Split(new Char[] { ' ', '\t' }).Where(s => s.Trim() != "").Select(s => s).ToArray();
                if (arrayLine[0] == "1") 
                {
                    line1 = arrayLine;
                }
                else if(arrayLine[0] == "2")
                {
                    line2 = arrayLine;
                    Console.WriteLine(line1[1] + ":" +line0[0]);
                }
                else
                {
                    line0 = arrayLine;
                }
            }
            Console.ReadLine();
        }
    }
}
