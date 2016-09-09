using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using System.Drawing.Imaging;
using MySql.Data.MySqlClient;
using GMap.NET;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using GMap.NET.WindowsForms.ToolTips;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Support;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Internal;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.PhantomJS;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BSTSRouting
{
    public partial class RoutingForm : Form
    {

        private DBConnect dbConnection;
        private List<WayNodeModel> listFrom;
        private List<WayNodeModel> listDestinantion;
                
        public RoutingForm()
        {
            InitializeComponent();
            dbConnection = new DBConnect();
        }

        private void RoutingForm_Load(object sender, EventArgs e)
        {
            // Initialize map:
            gMapControl.MapProvider = GMap.NET.MapProviders.OpenStreetMapProvider.Instance;
            GMaps.Instance.Mode = AccessMode.ServerOnly;
            gMapControl.Manager.Mode = AccessMode.ServerOnly;
            gMapControl.DragButton = MouseButtons.Left;

            // Set center of map
            gMapControl.Position = new PointLatLng(-6.89148, 107.6106591);
            gMapControl.MinZoom = 2;
            gMapControl.MaxZoom = 18;
            gMapControl.Zoom = 15;

            // set attribute for textbox become autocomplete
            textBoxFrom.AutoCompleteMode = AutoCompleteMode.Suggest;
            textBoxFrom.AutoCompleteSource = AutoCompleteSource.CustomSource;
            AutoCompleteStringCollection autoCompleteNodeFrom = new AutoCompleteStringCollection();
            textBoxDestination.AutoCompleteMode = AutoCompleteMode.Suggest;
            textBoxDestination.AutoCompleteSource = AutoCompleteSource.CustomSource;
            AutoCompleteStringCollection autoCompleteNodeDestination = new AutoCompleteStringCollection();

            // get data from database and put into listFrom
            listFrom = dbConnection.getWayNodeData();
            for (int i = 0; i < listFrom.Count; i++)
            {
                autoCompleteNodeFrom.Add(listFrom[i].getWayName());
            }
            textBoxFrom.AutoCompleteCustomSource = autoCompleteNodeFrom;

            // get data from database and put into listDestination
            listDestinantion = dbConnection.getWayNodeData();
            for (int i = 0; i < listDestinantion.Count; i++)
            {
                autoCompleteNodeDestination.Add(listDestinantion[i].getWayName());
            }
            textBoxDestination.AutoCompleteCustomSource = autoCompleteNodeDestination;
            
        }

        private void buttonRouting_Click(object sender, EventArgs e)
        {
            if (!textBoxFrom.Text.Equals("") && !textBoxDestination.Text.Equals("")) {
                String nodeFromName = textBoxFrom.Text;
                String nodeDestinationName = textBoxDestination.Text;
                WayNodeModel mFrom = dbConnection.getWayNodeDataAttribute(nodeFromName);
                WayNodeModel mDestination = dbConnection.getWayNodeDataAttribute(nodeDestinationName);
                String lat1 = Convert.ToString(mFrom.getLatitude());
                String lon1 = Convert.ToString(mFrom.getLongitude());
                String lat2 = Convert.ToString(mDestination.getLatitude());
                String lon2 = Convert.ToString(mDestination.getLongitude());

                // create valid latitude & longitude type from database, ex : -69097867 to -6.9097867 and 1076106278 to 107.6106278
                lat1 = lat1.Insert(2, ".");
                lon1 = lon1.Insert(3, ".");
                lat2 = lat2.Insert(2, ".");
                lon2 = lon2.Insert(3, ".");
                Console.WriteLine("Lat1:" + lat1 + " Lon1:" + lon1);
                Console.WriteLine("Lat2:" + lat2 + " Lon2:" + lon2);
                
                // add marker for source
                GMapOverlay markersOverlayFrom = new GMapOverlay(mFrom.getWayName());
                GMarkerGoogle markerFrom = new GMarkerGoogle(new PointLatLng(Convert.ToDouble(lat1), Convert.ToDouble(lon1)), GMarkerGoogleType.green);
                markersOverlayFrom.Markers.Add(markerFrom);
                gMapControl.Overlays.Add(markersOverlayFrom);

                // add marker for destination
                GMapOverlay markersOverlayDest = new GMapOverlay(mDestination.getWayName());
                GMarkerGoogle markerDest = new GMarkerGoogle(new PointLatLng(Convert.ToDouble(lat2), Convert.ToDouble(lon2)), GMarkerGoogleType.red);
                markersOverlayDest.Markers.Add(markerDest);
                gMapControl.Overlays.Add(markersOverlayDest);

                // getCenterMap from 2 location
                PointLocation centerMapLocation = new PointLocation();
                MapUtils mapUtil = new MapUtils();
                centerMapLocation = mapUtil.calculateMidPointLocations(Convert.ToDouble(lat1), Convert.ToDouble(lon1), Convert.ToDouble(lat2), Convert.ToDouble(lon2));
                double bearing = mapUtil.calculateBearing(Convert.ToDouble(lat1), Convert.ToDouble(lon1), Convert.ToDouble(lat2), Convert.ToDouble(lon2));
                double distance = mapUtil.calculateDistanceKM(Convert.ToDouble(lat1), Convert.ToDouble(lon1), Convert.ToDouble(lat2), Convert.ToDouble(lon2));
                Console.WriteLine("Lat3:" + centerMapLocation.getLatitude() + " Lon3:" + centerMapLocation.getLongitude());
                Console.WriteLine("Bearing:" + bearing);
                Console.WriteLine("Distance:" + distance);
                Console.WriteLine("Pixel Distance:" + CentimeterToPixel(distance * 10));

                // get route result
                string jsonResultPath = getRoutePath(mFrom.getNodeId(), mDestination.getNodeId());
                JObject results = JObject.Parse(jsonResultPath);
                List<NodeModel> listNode = new List<NodeModel>();
                foreach (var result in results["path"])
                {
                    NodeModel node = new NodeModel();
                    string nodeId = (string)result["node_id"];
                    string lat = (string)result["latitude"];
                    string lon = (string)result["longitude"];
                    // create valid latitude & longitude type from database, ex : -69097867 to -6.9097867 and 1076106278 to 107.6106278
                    lat = lat.Insert(2, ".");
                    lon = lon.Insert(3, ".");
                    node.setNodeId(nodeId);
                    node.setLatitude(Convert.ToDouble(lat));
                    node.setLongitude(Convert.ToDouble(lon));
                    listNode.Add(node);
                    //Console.WriteLine("NodeId: {0}, Latitude: {1}, Longitude: {2}", nodeId, lat, lon);
                }

                // draw path
                GMapOverlay routeOverlay = new GMapOverlay("path"); 
                List<PointLatLng> points = new List<PointLatLng>();                
                // path point
                for (int i = 0; i < listNode.Count; i++) {
                    points.Add(new PointLatLng(listNode[i].getLatitude(), listNode[i].getLongitude()));
                    Console.WriteLine("NodeId: {0}, Latitude: {1}, Longitude: {2}", listNode[i].getNodeId(), listNode[i].getLatitude(), listNode[i].getLongitude());
                }
                // draw line
                GMapRoute gRoute = new GMapRoute(points, "route");
                gRoute.Stroke.Width = 7;
                //gRoute.Stroke.Color = Color.SeaGreen;
                //gRoute.Stroke = new Pen(Color.Blue, 7);
                routeOverlay.Routes.Add(gRoute);
                gMapControl.Overlays.Add(routeOverlay);

                // show getCenterMap from 2 location
                gMapControl.Position = new GMap.NET.PointLatLng(centerMapLocation.getLatitude(), centerMapLocation.getLongitude());
                gMapControl.Zoom = 16;
                int width = gMapControl.Width;
                int height = gMapControl.Height;
                Console.WriteLine("[MapControl] Width: " + width + " Height:" + height);

                /*
                // AStar calculation
                AStar astar = new AStar();
                astar.calcuateShortestPath(mFrom.getNodeId(), mDestination.getNodeId());
                */
                
                /*
                // test take 5 screenshots
                List<WayNodeModel> listWays = dbConnection.getWayNodeData();
                for(int i = 0; i < 5; i++){
                    createMapTileScreenshots(listWays[i].getNodeId(), Convert.ToString(listWays[i].getLatitude()), Convert.ToString(listWays[i].getLongitude())); 
                }*/                
                
                /*String pathScreenshots = System.IO.Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory) + Path.DirectorySeparatorChar + "screenshots" + Path.DirectorySeparatorChar;
                Console.WriteLine("Screenshots Path: " + pathScreenshots);
                try
                {
                    // setting for proxy
                    //PhantomJSOptions phOptions = new PhantomJSOptions();
                    //phOptions.AddAdditionalCapability(CapabilityType.Proxy, "cache.itb.ac.id");

                    // hide cmd windows of phantomsjs
                    var driverService = PhantomJSDriverService.CreateDefaultService();
                    driverService.HideCommandPromptWindow = true;

                    // initiate phantomjs driver
                    PhantomJSDriver phantom;
                    phantom = new PhantomJSDriver(driverService);
                    // setting the default timeout to 30 seconds
                    phantom.Manage().Timeouts().ImplicitlyWait(new TimeSpan(0, 0, 30));
                    string screenshotUrl = "http://localhost/bsts_routing/tilegen/v2/index.php?latitude=-6.9131786&longitude=107.6474137";
                    phantom.Navigate().GoToUrl(screenshotUrl);
                    
                    // grab the snapshot
                    Screenshot sh = phantom.GetScreenshot();
                    sh.SaveAsFile(pathScreenshots + "test.png", ImageFormat.Png);
                    phantom.Quit();
                     
                } catch (Exception error) {
                    Console.WriteLine(error.Message);
                }*/
                        
            }
            else {
                MessageBox.Show("Please input address first!");                        
            }
            
        }

        string getRoutePath(string nodeStartId, string nodeDestinationId) {

            string url = "http://localhost/bsts_routing/index.php/api/route/routing/" + nodeStartId + "/" + nodeDestinationId;
            // Create a request for the URL. 
            WebRequest request = WebRequest.Create(url);
            // Set the Method property of the request to POST.
            request.Method = "GET";            
            // Set the ContentType property of the WebRequest.
            request.ContentType = "application/x-www-form-urlencoded";
            // Get the response.
            WebResponse response = request.GetResponse();
            // Display the status.
            Console.WriteLine(((HttpWebResponse)response).StatusDescription);
            
            // Get the stream containing content returned by the server.
            Stream dataStream = response.GetResponseStream();
            // Open the stream using a StreamReader.
            StreamReader reader = new StreamReader(dataStream);
            // Read the content.
            string responseFromServer = reader.ReadToEnd();
            // Display the content.
            Console.WriteLine(responseFromServer);
            // Clean up the streams and the response.
            reader.Close();
            response.Close();

            return responseFromServer;
        }

        private void createMapTileScreenshots(string nodeId, string latitude, string longitude) {
            String pathScreenshots = System.IO.Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory) + Path.DirectorySeparatorChar + "screenshots" + Path.DirectorySeparatorChar;
            Console.WriteLine("Screenshots Path: " + pathScreenshots);
            try
            {
                // setting for proxy
                //PhantomJSOptions phOptions = new PhantomJSOptions();
                //phOptions.AddAdditionalCapability(CapabilityType.Proxy, "cache.itb.ac.id");

                // hide cmd windows of phantomsjs
                var driverService = PhantomJSDriverService.CreateDefaultService();
                driverService.HideCommandPromptWindow = true;

                // initiate phantomjs driver
                PhantomJSDriver phantom;
                phantom = new PhantomJSDriver(driverService);
                // setting the default timeout to 30 seconds
                phantom.Manage().Timeouts().ImplicitlyWait(new TimeSpan(0, 0, 30));
                string screenshotUrl = "http://localhost/bsts_routing/tilegen/v2/index.php?latitude=" + latitude + "&longitude=" + longitude;
                phantom.Navigate().GoToUrl(screenshotUrl);

                // grab the snapshot
                Screenshot sh = phantom.GetScreenshot();
                TimeUtils timeUtils = new TimeUtils();
                string fileName = nodeId + "_" + Convert.ToString(timeUtils.ToUnixTime(DateTime.Now)) + ".png";
                sh.SaveAsFile(pathScreenshots + fileName, ImageFormat.Png);
                phantom.Quit();

            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
            }
        }

        int CentimeterToPixel(double Centimeter)
        {
            double pixel = -1;
            using (Graphics g = this.CreateGraphics())
            {
                pixel = Centimeter * g.DpiY / 2.54d;
            }
            return (int)pixel;
        }
    }
}