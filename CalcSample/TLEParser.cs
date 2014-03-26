using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcSample
{
    class TLEParser
    {
        public TLE Parse(string str)
        {
            TLE tle = new TLE();
            return tle;
        }
        private DateTime parseEpochTime(string str)
        {
            try
            {
                int year = int.Parse(str.Take(2).ToString());
                int days = int.Parse(str.Skip(2).Take(3).ToString());
                double portion = double.Parse("0." + str.Skip(6).ToString());
                DateTime date = new DateTime(year, 1, 1);
                date.AddDays(days);
                date.AddHours(portion * 24);
                return date;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
    }
}
