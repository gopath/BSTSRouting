using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSTSRouting
{
    public class WayNodeModel
    {
        private String wayId;
        private String nodeId;
        private double latitude;
        private double longitude;
        private String wayName;

        public String getWayId(){
            return wayId;
        }

        public void setWayId(String id) {
            this.wayId = id;
        }

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

        public String getWayName() {
            return wayName;
        }

        public void setWayName(String name) {
            this.wayName = name;
        }

    }
}