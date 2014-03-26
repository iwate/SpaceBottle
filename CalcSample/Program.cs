using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcSample
{
    class Program
    {
        static string strTle = "1 07530U 74089B   14083.48346656 -.00000031  00000-0  78220-4 0  9247\n2 07530 101.4647  68.1286 0012384 100.2533  53.9338 12.53603706 80090 4";
        static void Main(string[] args)
        {
            var now = new DateTime(2014, 3, 25, 21, 0, 0, DateTimeKind.Utc);
            var tle = new TLE(strTle);
            var dt = (now - tle.EpochTime).TotalDays;
            var Mm = tle.MeanMotion +  tle.MeanMotion1 * dt; // [rev / day]
            var MmRad = 2.0 * Math.PI * Mm;
            var MmRadSec = MmRad / 86400.0;
            var GM = 3.986004418E14;
            var a = Math.Pow(GM / Math.Pow(MmRadSec, 2), 1.0 / 3); // 立方根の精度が不安
            var M0 = tle.MeanAnomaly / 360; // [rev]
            var M = M0 + tle.MeanMotion * dt + 0.5 * (tle.MeanMotion1 * Math.Pow(dt, 2)); // [rev]
            var Mrad = M * 2.0 * Math.PI; // [rad]
            var e = tle.Eccentricity;
            var E = calcEccentricAnomaly(Mrad, e); //[rad]
            var U = a * (Math.Cos(E) - e);
            var V = a * Math.Sqrt(1.0 - e * e) * Math.Sin(E);
            var AoP0 = tle.ArgumentOfPerigee * Math.PI / 180; // [rad]
            var RA0 = tle.RightAscension * Math.PI / 180; // [rad]
            var i = tle.Inclination * Math.PI / 180; // [rad]
            var r = 6378137;
            var J2 = 1.0826158e-3;
            var t = 1.5 * J2 * r * r * MmRad / (a * a * (1.0 - e * e) * (1.0 - e * e));
            var AoP = AoP0 + ( 2.5 * Math.Pow(Math.Cos(i), 2) - 0.5) * t * dt;
            var RA = RA0 - Math.Cos(i) * t * dt;
            var cosAoP = Math.Cos(AoP);
            var sinAoP = Math.Sin(AoP);
            var cosRA = Math.Cos(RA);
            var sinRA = Math.Sin(RA);
            var cosi = Math.Cos(i);
            var sini = Math.Sin(i);
            var x = U * (cosRA * cosAoP - sinRA * cosi * sinAoP) - V * (cosRA * sinAoP + sinRA * cosi * cosAoP);
            var y = U * (sinRA * cosAoP + cosRA * cosi * sinAoP) - V * (sinRA * sinAoP - cosRA * cosi * cosAoP);
            var z = U * sini * sinAoP + V * sini * cosAoP;

            var alpha = Math.Atan2(y, x);
            if(alpha < 0){
                alpha += 2.0 * Math.PI;
            }
            var delta = Math.Atan2(z, Math.Sqrt(x * x + y * y));
            var distance = Math.Sqrt(x * x + y * y + z * z);
            var theta = calcSiderealRad(now);
            var longitude = alpha - theta;
            longitude -= Math.Floor(longitude / (2.0 * Math.PI)) * (2.0 * Math.PI);
            longitude *= 180 / Math.PI;
            if (longitude > 180)
            {
                longitude -= 360;
            }
            var latitude = delta * 180 / Math.PI;
            Console.WriteLine(latitude + "," + longitude);
            Console.ReadLine();
        }
        static double calcEccentricAnomaly(double M, double e)
        {
            var Ei = M + e * Math.Sin(M);
            double Mi;
            while (true)
            {
                Mi = Ei - e * Math.Sin(Ei);
                if (M == Mi)
                {
                    return Ei;
                }
                Ei = Ei + (M - Mi) / (1 - e * Math.Cos(Ei));
            }
        }
        static double calcSiderealTime(DateTime date)
        {
            double Y, M, D, h, m, s;
            if (date.Month < 2)
            {
                Y = date.Year - 1;
                M = date.Month + 12;
            }
            else
            {
                Y = date.Year;
                M = date.Month;
            }
            D = date.Day;
            h = date.Hour;
            m = date.Minute;
            s = date.Second;
            var JD = Math.Floor(365.25 * Y) + Math.Floor(Y / 400) - Math.Floor(Y / 100) + Math.Floor(30.59 * (M-2)) + D + 1721088.5 + h / 24 + m / 1440 + s / 86400;
            var TJD = JD - 2440000.5;
            return (0.671262 + 1.0027379094 * TJD) * 86400.0;
        }
        static double calcSiderealRad(DateTime date)
        {
            var h = calcSiderealTime(date);
            h = h - Math.Floor(h / 86400.0) * 86400.0;
            return h * 2.0 * Math.PI / 86400.0;
        }
    }
}
