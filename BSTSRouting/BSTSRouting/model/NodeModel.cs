using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSTSRouting
{
    public class NodeModel
    {
        private String nodeId;
        private double latitude;
        private double longitude;
        
        public String getNodeId() {
            return nodeId;
        }

        public void setNodeId(String id) {
            this.nodeId = id;
        }

        public double getLatitude() {
            return latitude;
        }

        public void setLatitude(double lat) {
            this.latitude = lat;
        }

        public double getLongitude() {
            return longitude;
        }

        public void setLongitude(double lon) {
            this.longitude = lon;
        }

    }
}