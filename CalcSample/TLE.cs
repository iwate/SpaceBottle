using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcSample
{
    class TLE
    {
        public int SatelliteNumber {get; set;}
        public string Classification { get; set; }
        public string InternationalDesignator { get; set; }
        public DateTime EpochTime { get; set; }
        public double MeanMotion1 { get; set; }
        public double MeanMotion2 { get; set; }
        public double BSTARDragTrem { get; set; }
        public int EphemerisType { get; set; }
        public int ElementNumber { get; set; }
        public int Checksum1 { get; set; }
        public double Inclination { get; set; }
        public double RightAscension { get; set; }
        public double Eccentricity { get; set; }
        public double ArgumentOfPerigee { get; set; }
        public double MeanAnomaly { get; set; }
        public double MeanMotion { get; set; }
        public double RevolutionNumber { get; set; }
        public int Checksum2 { get; set; }
        private void setEpochTime(string str)
        {
            try
            {
                int year = int.Parse(str.Substring(0, 2));
                year += (year < 57)? 2000 : 1900; // 時限爆弾
                double days = double.Parse(str.Substring(2, 3));
                double portion = double.Parse("0." + str.Substring(6));
                this.EpochTime = new DateTime(year, 1, 1, 0 ,0, 0, DateTimeKind.Utc)
                    .AddDays(days - 1)
                    .AddHours(portion * 24);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        public TLE() { }
        public TLE(string str)
        {
            string[] lines = str.Split('\n');
            var line1 = lines[0].Split(new Char[] { ' ', '\t' }).Where(s => s.Trim()  != "").Select(s => s).ToArray();
            var line2 = lines[1].Split(new Char[] { ' ', '\t' }).Where(s => s.Trim() != "").Select(s => s).ToArray();

            this.SatelliteNumber = int.Parse(line1[1].Substring(0, 5));
            this.Classification = line1[1].Substring(5, 1);
            this.InternationalDesignator = line1[2];
            this.setEpochTime(line1[3]);
            this.MeanMotion1 = double.Parse(line1[4]) * 2;
            this.MeanMotion2 = double.Parse("0." + line1[5].Substring(0, line1[5].Length - 2) + "E" + line1[5].Substring(line1[5].Length - 2, 2)) * 6;
            this.BSTARDragTrem = double.Parse("0." + line1[6].Substring(0, line1[6].Length - 2) + "E" + line1[6].Substring(line1[6].Length - 2, 2));
            this.EphemerisType = int.Parse(line1[7]);
            this.ElementNumber = int.Parse(line1[8]);
            if(line1.Length > 9){
                this.Checksum1 = int.Parse(line1[9]);
            }

            this.Inclination = double.Parse(line2[2]);
            this.RightAscension = double.Parse(line2[3]);
            this.Eccentricity = double.Parse("0." + line2[4]);
            this.ArgumentOfPerigee = double.Parse(line2[5]);
            this.MeanAnomaly = double.Parse(line2[6]);
            this.MeanMotion = double.Parse(line2[7]);
            this.RevolutionNumber = double.Parse(line2[8]);
            if (line2.Length > 9)
            {
                this.Checksum2 = int.Parse(line2[9]);
            }
        }
    }
}
