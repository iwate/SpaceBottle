using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculationWorkerRole
{
    class User
    {
        private static double aLatitude = 2 * Math.PI * 6378150 / 360;
        private static double aLongitude = 2 * Math.PI;
        public string UserId { get; set; }
        public Position Position { get; set; }
        public double calcDistanceSquare(Position satellite)
        {
            var latDiff = this.Position.latitude.Value - satellite.latitude.Value;
            var longDiff = this.Position.longitude.Value - satellite.longitude.Value;
            if (longDiff > 180)
            {
                longDiff -= 360;
            }
            else if (longDiff < -180)
            {
                longDiff += 360;
            }
            var x = longitudeToMeter(longDiff);
            var y = latitudeToMeter(latDiff);
            return x * x + y * y;
        }

        private static double a = 6378137;
        private static double eSquare = 0.006694380022900788;
        private static double alpha = Math.PI / 360;
        public double latitudeToMeter(double latitude)
        {
            return latitude * alpha * a * (1 - eSquare) / Math.Pow(1 - eSquare * Math.Sin(latitude), 3 / 2);
        }
        public double longitudeToMeter(double longitude)
        {
            return longitude * alpha * a * Math.Cos(longitude) / Math.Sqrt(1 - eSquare * Math.Pow(Math.Sin(longitude), 2));
        }
    }
}
