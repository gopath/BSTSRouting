using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSTSRouting
{
    class MathUtils
    {
        public double DegreeToRadians(double angle)
        {
            return (Math.PI / 180) * angle;
        }

        public double RadiansToDegree(double angle)
        {
            return angle * (180.0 / Math.PI);
        }
    }
}
