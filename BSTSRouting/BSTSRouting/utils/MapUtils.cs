using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSTSRouting
{
    class MapUtils
    {
        public PointLocation calculateMidPointLocations(double lat1, double lon1, double lat2, double lon2) {
            PointLocation point = new PointLocation();
            MathUtils mathUtils = new MathUtils();
            double dLon = mathUtils.DegreeToRadians(lon2 - lon1);
            double tempLat1 = mathUtils.DegreeToRadians(lat1);
            double tempLat2 = mathUtils.DegreeToRadians(lat2);
            double tempLon1 = mathUtils.DegreeToRadians(lon1);
            double Bx = Math.Cos(tempLat2) * Math.Cos(dLon);
            double By = Math.Cos(tempLat2) * Math.Sin(dLon);
            double lat3 = Math.Atan2(Math.Sin(tempLat1) + Math.Sin(tempLat2), Math.Sqrt((Math.Cos(tempLat1) + Bx) * (Math.Cos(tempLat1) + Bx) + By * By));
            double lon3 = tempLon1 + Math.Atan2(By, Math.Cos(tempLat1) + Bx);
            lat3 = mathUtils.RadiansToDegree(lat3);
            lon3 = mathUtils.RadiansToDegree(lon3);
            point.setLatitude(lat3);
            point.setLongitude(lon3);
            return point;
        }

        public double calculateBearing(double lat1, double lon1, double lat2, double lon2) {
            MathUtils mathUtils = new MathUtils();
            double tempLat1 = mathUtils.DegreeToRadians(lat1);
            double tempLat2 = mathUtils.DegreeToRadians(lat2);
            double tempLon1 = mathUtils.DegreeToRadians(lon1);
            double tempLon2 = mathUtils.DegreeToRadians(lon2);
            double dLon = tempLon2 - tempLon1;

            double y = Math.Sin(dLon) * Math.Cos(tempLat2);
            double x = Math.Cos(tempLat1) * Math.Sin(tempLat2) - Math.Sin(tempLat1) * Math.Cos(tempLat2) * Math.Cos(dLon);
            double bearing = Math.Atan2(y, x);

            return (mathUtils.RadiansToDegree(bearing) + 360) % 360;
        }

        public double calculateDistanceKM(double lat1, double lon1, double lat2, double lon2)
        {
            double EARTH_RADIUS_KM = 6371;
            MathUtils mathUtils = new MathUtils();
            double dLat = mathUtils.DegreeToRadians(lat2 - lat1);
            double dLon = mathUtils.DegreeToRadians(lon2 - lon1);

            double a = Math.Pow(Math.Sin(dLat / 2), 2) +
                       Math.Cos(mathUtils.DegreeToRadians(lat1)) * Math.Cos(mathUtils.DegreeToRadians(lat2)) *
                       Math.Pow(Math.Sin(dLon / 2), 2);

            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            double distance = EARTH_RADIUS_KM * c;
            return distance;
        }
    }
}