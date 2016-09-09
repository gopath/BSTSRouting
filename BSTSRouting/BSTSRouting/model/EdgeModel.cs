using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSTSRouting
{
    public class EdgeModel
    {
        private String nodeFrom;
        private String nodeTo;
        private double distance;
        
        public String getNodeFrom() {
            return nodeFrom;
        }

        public void setNodeFrom(String from) {
            this.nodeFrom = from;
        }

        public String getNodeTo()
        {
            return nodeTo;
        }

        public void setNodeTo(String to)
        {
            this.nodeTo = to;
        }

        public double getDistance() {
            return distance;
        }

        public void setDistance(double distance) {
            this.distance = distance;
        }

    }
}