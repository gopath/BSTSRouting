using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BSTSRouting
{
    public enum DistanceType { ml, km };

    class Haversine
    {
        // Returns the distance in miles or kilometers of any two latitude / longitude points.
        public static double Distance(Node node1, Node node2, DistanceType type)
        {
            MathUtils mathUtils = new MathUtils();
            double R = (type == DistanceType.ml) ? 3960 : 6371;
            double dLat = mathUtils.DegreeToRadians(node2.Latitude - node1.Latitude);
            double dLon = mathUtils.DegreeToRadians(node2.Longitude - node1.Longitude);
            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(mathUtils.DegreeToRadians(node1.Latitude)) * Math.Cos(mathUtils.DegreeToRadians(node2.Latitude)) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            double c = 2 * Math.Asin(Math.Min(1, Math.Sqrt(a)));
            double d = R * c;
            return d;
        }

    }
}
