using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
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
                String nodeFrom = textBoxFrom.Text;
                String nodeDestination = textBoxDestination.Text;
                List<WayNodeModel> lFrom = dbConnection.getWayNodeDataAttribute(nodeFrom);
                List<WayNodeModel> lDestination = dbConnection.getWayNodeDataAttribute(nodeDestination);

                String lat1 = Convert.ToString(lFrom[0].getLatitude());
                String lon1 = Convert.ToString(lFrom[0].getLongitude());
                String lat2 = Convert.ToString(lDestination[0].getLatitude());
                String lon2 = Convert.ToString(lDestination[0].getLongitude());

                // create valid latitude & longitude type, ex : -69097867 to -6.9097867 and 1076106278 to 107.6106278
                lat1 = lat1.Insert(2, ".");
                lon1 = lon1.Insert(3, ".");
                lat2 = lat2.Insert(2, ".");
                lon2 = lon2.Insert(3, ".");
                Console.WriteLine("Lat1:" + lat1 + " Lon1:" + lon1);
                Console.WriteLine("Lat2:" + lat2 + " Lon2:" + lon2);
                
                // add marker for source
                GMapOverlay markersOverlayFrom = new GMapOverlay(lFrom[0].getWayName());
                GMarkerGoogle markerFrom = new GMarkerGoogle(new PointLatLng(Convert.ToDouble(lat1), Convert.ToDouble(lon1)), GMarkerGoogleType.green);
                markersOverlayFrom.Markers.Add(markerFrom);
                gMapControl.Overlays.Add(markersOverlayFrom);

                // add marker for destination
                GMapOverlay markersOverlayDest = new GMapOverlay(lDestination[0].getWayName());
                GMarkerGoogle markerDest = new GMarkerGoogle(new PointLatLng(Convert.ToDouble(lat2), Convert.ToDouble(lon2)), GMarkerGoogleType.red);
                markersOverlayDest.Markers.Add(markerDest);
                gMapControl.Overlays.Add(markersOverlayDest);

                // getCenterMap from 2 location
                PointLocation point = new PointLocation();
                MapUtils mapUtil = new MapUtils();
                point = mapUtil.calculateMidPointLocations(Convert.ToDouble(lat1), Convert.ToDouble(lon1), Convert.ToDouble(lat2), Convert.ToDouble(lon2));
                double bearing = mapUtil.calculateBearing(Convert.ToDouble(lat1), Convert.ToDouble(lon1), Convert.ToDouble(lat2), Convert.ToDouble(lon2));
                double distance = mapUtil.calculateDistanceKM(Convert.ToDouble(lat1), Convert.ToDouble(lon1), Convert.ToDouble(lat2), Convert.ToDouble(lon2));
                Console.WriteLine("Lat3:" + point.getLatitude() + " Lon3:" + point.getLongitude());
                Console.WriteLine("Bearing:" + bearing);
                Console.WriteLine("Distance:" + distance);
                Console.WriteLine("Pixel Distance:" + CentimeterToPixel(distance * 10));
                
                // draw path
                GMapOverlay polyOverlay = new GMapOverlay("path"); 
                List<PointLatLng> points = new List<PointLatLng>();
                points.Add(new PointLatLng(Convert.ToDouble(lat1), Convert.ToDouble(lon1)));
                points.Add(new PointLatLng(Convert.ToDouble(lat2), Convert.ToDouble(lon2)));
                GMapPolygon polygon = new GMapPolygon(points, "path_poly");
                polygon.Stroke = new Pen(Color.Blue, 7);
                polyOverlay.Polygons.Add(polygon);
                gMapControl.Overlays.Add(polyOverlay);

                gMapControl.Position = new GMap.NET.PointLatLng(point.getLatitude(), point.getLongitude());
                gMapControl.Zoom = 16;
                int width = gMapControl.Width;
                int height = gMapControl.Height;
                Console.WriteLine("[MapControl] Width: " + width + " Height:" + height);

                /*
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
                    phantom.Navigate().GoToUrl("http://www.google.com");
                    
                    // grab the snapshot
                    Screenshot sh = phantom.GetScreenshot();
                    sh.SaveAsFile(pathScreenshots + "test.png", ImageFormat.Png);
                    phantom.Quit();
                     
                } catch (Exception error) {
                    Console.WriteLine(error.Message);
                }
                */
                
            }
            else {
                MessageBox.Show("Please input address first!");                        
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