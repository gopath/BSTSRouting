using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSTSRouting
{
    class MidPointLocation
    {
        public PointLocation getCenterLocations(double lat1, double lon1, double lat2, double lon2) {
            PointLocation point = new PointLocation();
            MathUtils mathUtils = new MathUtils();
            double dLon = mathUtils.ConvertToRadians(lon2 - lon1);
            double tempLat1 = mathUtils.ConvertToRadians(lat1);
            double tempLat2 = mathUtils.ConvertToRadians(lat2);
            double tempLon1 = mathUtils.ConvertToRadians(lon1);
            double Bx = Math.Cos(tempLat2) * Math.Cos(dLon);
            double By = Math.Cos(tempLat2) * Math.Sin(dLon);
            double lat3 = Math.Atan2(Math.Sin(tempLat1) + Math.Sin(tempLat2), Math.Sqrt((Math.Cos(tempLat1) + Bx) * (Math.Cos(tempLat1) + Bx) + By * By));
            double lon3 = tempLon1 + Math.Atan2(By, Math.Cos(tempLat1) + Bx);
            lat3 = mathUtils.RadianToDegree(lat3);
            lon3 = mathUtils.RadianToDegree(lon3);
            point.setLatitude(lat3);
            point.setLongitude(lon3);
            return point;
        }
    }
}