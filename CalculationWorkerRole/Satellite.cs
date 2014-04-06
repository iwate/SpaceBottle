using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculationWorkerRole
{
    class Satellite
    {
        private satellites _context;
        public satellites context
        {
            get { return this._context; }
            set
            {
                this._context = value;
                this.setEpochTime(value.epoch_date);
                this.dMeanMotion = Double.Parse(value.d_mean_motion) * 2;
                string dd_mean_motion = value.dd_mean_motion;
                double sign = 1.0;
                if(dd_mean_motion.Substring(0, 1) == "-"){
                    dd_mean_motion = dd_mean_motion.Substring(1, dd_mean_motion.Length - 1);
                    sign = -1.0;
                }
                this.ddMeanMotion = sign * Double.Parse("0." + dd_mean_motion.Substring(0, dd_mean_motion.Length - 2) + "E" + dd_mean_motion.Substring(dd_mean_motion.Length - 2, 2)) * 6;
                this.Inclination = Double.Parse(value.inclination);
                this.Ascension = Double.Parse(value.ascension);
                this.Eccentricity = Double.Parse("0." + value.eccentricity);
                this.ArgOfPerigee = Double.Parse(value.arg_of_perigee);
                this.MeanAnomaly = Double.Parse(value.mean_anomaly);
                this.MeanMotion = Double.Parse(value.mean_motion);
                this.RevolutionNumber = Double.Parse(value.revolution_number);
            }
        }
        public DateTime EpochTime { get; set; }
        public double dMeanMotion { get; set; }
        public double ddMeanMotion { get; set; }
        public double Inclination { get; set; }
        public double Ascension { get; set; }
        public double Eccentricity { get; set; }
        public double ArgOfPerigee { get; set; }
        public double MeanAnomaly { get; set; }
        public double MeanMotion { get; set; }
        public double RevolutionNumber { get; set; }
        private void setEpochTime(string str)
        {
            try
            {
                int year = int.Parse(str.Substring(0, 2));
                year += (year < 57) ? 2000 : 1900; // 時限爆弾
                double days = double.Parse(str.Substring(2, 3));
                double portion = double.Parse("0." + str.Substring(6));
                this.EpochTime = new DateTime(year, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                    .AddDays(days - 1)
                    .AddHours(portion * 24);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        private double calcEccentricAnomaly(double M, double e)
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
        private double calcSiderealTime(DateTime date)
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
            var JD = Math.Floor(365.25 * Y) + Math.Floor(Y / 400) - Math.Floor(Y / 100) + Math.Floor(30.59 * (M - 2)) + D + 1721088.5 + h / 24 + m / 1440 + s / 86400;
            var TJD = JD - 2440000.5;
            return (0.671262 + 1.0027379094 * TJD) * 86400.0;
        }
        private double calcSiderealRad(DateTime date)
        {
            var h = calcSiderealTime(date);
            h = h - Math.Floor(h / 86400.0) * 86400.0;
            return h * 2.0 * Math.PI / 86400.0;
        }
        public Position getPositionAt(DateTime now) // now is UTC Time
        {
            var dt = (now - this.EpochTime).TotalDays;
            var Mm = this.MeanMotion + this.dMeanMotion * dt; // [rev / day]
            var MmRad = 2.0 * Math.PI * Mm;
            var MmRadSec = MmRad / 86400.0;
            var GM = 3.986004418E14;
            var a = Math.Pow(GM / Math.Pow(MmRadSec, 2), 1.0 / 3); // 立方根の精度が不安
            var M0 = this.MeanAnomaly / 360; // [rev]
            var M = M0 + this.MeanMotion * dt + 0.5 * (this.dMeanMotion * Math.Pow(dt, 2)); // [rev]
            var Mrad = M * 2.0 * Math.PI; // [rad]
            var e = this.Eccentricity;
            var E = calcEccentricAnomaly(Mrad, e); //[rad]
            var U = a * (Math.Cos(E) - e);
            var V = a * Math.Sqrt(1.0 - e * e) * Math.Sin(E);
            var AoP0 = this.ArgOfPerigee * Math.PI / 180; // [rad]
            var RA0 = this.Ascension * Math.PI / 180; // [rad]
            var i = this.Inclination * Math.PI / 180; // [rad]
            var r = 6378137;
            var J2 = 1.0826158e-3;
            var t = 1.5 * J2 * r * r * MmRad / (a * a * (1.0 - e * e) * (1.0 - e * e));
            var AoP = AoP0 + (2.5 * Math.Pow(Math.Cos(i), 2) - 0.5) * t * dt;
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
            if (alpha < 0)
            {
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
            return new Position { Latitude = latitude, Longitude = longitude };
        }
    }
}
