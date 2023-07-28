using GMap.NET.MapProviders;
using GMap.NET;
using GMap.NET.WindowsForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GMap.NET.WindowsForms.Markers;
using static GMap.NET.Entity.OpenStreetMapRouteEntity;
using static System.Windows.Forms.AxHost;

namespace GMapAddingMarkersOnMouseClick
{
  
    public partial class Form1 : Form
    {
        private List<PointLatLng> _points;
        //private GMapControl gmap;
        //private GMapOverlay imagesOverlay;
        private GMapOverlay markersOverlay;
        private GMapOverlay linesOverlay;
        private GMapOverlay polygonsOverlay;

        private bool boolDropPin = false;
        private bool boolDrawLine = false;
        private bool boolDrawPolygon = false;

        private GMapOverlay imageOverlay;
        private Bitmap bmp;
        private int sizeScale = 4;
        public Form1()
        {
            InitializeComponent();
            _points = new List<PointLatLng>();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            gmap.Bearing = 0;
            gmap.CanDragMap = true;
            gmap.DragButton = MouseButtons.Left;
            gmap.GrayScaleMode = true;
            gmap.MarkersEnabled = true;
            gmap.MaxZoom = 18;
            gmap.MinZoom = 2;
            //gmap.MouseWheelZoomType = GMap.NET.MouseWheelZoomType.MousePositionWithoutCenter;

            gmap.ShowCenter = false; //to remove Red color plus in the centre

            gmap.NegativeMode = false;
            gmap.PolygonsEnabled = true;
            gmap.RoutesEnabled = true;
            gmap.ShowTileGridLines = false;
            gmap.Zoom = 10;
            // Set the initial position and zoom level
            gmap.Position = new PointLatLng(37.0902, -95.7129); // Example: United States
            //gmap.Zoom = 5;

            // Set the map provider (you can choose from various providers)
            gmap.MapProvider = GMapProviders.GoogleMap;
            GMap.NET.GMaps.Instance.Mode = GMap.NET.AccessMode.ServerAndCache;
            // Enable the built-in map drag and zoom controls
            gmap.DragButton = MouseButtons.Left;
            gmap.MouseWheelZoomEnabled = true;
            /*
            double anchorLat = 37.0902;
            double anchorLng = -95.7129;
            // Add the GMapControl to the form
            //gmap.Dock = DockStyle.Fill;
            //Controls.Add(gmap);
            
            //Create image overlays
            bmp = (Bitmap)Image.FromFile("img/polygon_top.png");
           // Bitmap rotatedScaledBmp = RotateAndScaleImage(bmp, -30.0f, 0.3f);

            GMapMarker imageMarker = new GMarkerGoogle(new PointLatLng(anchorLat, anchorLng), bmp);
            imageMarker.Size = new Size(bmp.Width / sizeScale, bmp.Height / sizeScale);
            imageMarker.Offset = new Point(-bmp.Width / sizeScale, -bmp.Height / (sizeScale * 2));
            // Create overlays for markers, lines, and polygons
            imageOverlay = new GMapOverlay("imagesOverlay");
            imageOverlay.Markers.Add(imageMarker);
            */
            CreatePinLinPolygonMarkers();
        }
        private void CreatePinLinPolygonMarkers()
        {
            markersOverlay = new GMapOverlay("markersOverlay");
            linesOverlay = new GMapOverlay("linesOverlay");
            polygonsOverlay = new GMapOverlay("polygonsOverlay");

            // Add the overlays to the map
            gmap.Overlays.Add(markersOverlay);
            gmap.Overlays.Add(linesOverlay);
            gmap.Overlays.Add(polygonsOverlay);
            // gmap.Overlays.Add(imagesOverlay);

            // Subscribe to the map's MouseClick event
            gmap.MouseClick += Gmap_MouseClick;
            // Event handler for the map's zoom changes
            //gmap.OnMapZoomChanged += Gmap_OnMapZoomChanged;
        }
        private void Gmap_OnMapZoomChanged()
        {
            // Adjust the position of the image overlay to keep it anchored to the same point on the map
            double anchorLat = 40.7128;
            double anchorLng = -74.0060;

            GMapMarker imageMarker = (GMapMarker)imageOverlay.Markers[0];
            imageMarker.Position = new PointLatLng(anchorLat, anchorLng);
            imageMarker.Offset = new Point(-bmp.Width / sizeScale, -bmp.Height / (sizeScale * 2));

            // Refresh the map to update the image overlay position
            gmap.Refresh();
        }
        private void Gmap_MouseClick(object sender, MouseEventArgs e)
        {
            // Get the clicked map position
            PointLatLng clickedPosition = gmap.FromLocalToLatLng(e.X, e.Y);
            
            // Check the mouse button that was clicked
            if (e.Button == MouseButtons.Left)

            {
                if (boolDropPin)
                {
                    // Add a marker pin at the clicked position
                    GMarkerGoogle marker = new GMarkerGoogle(clickedPosition, GMarkerGoogleType.red);
                 
                    markersOverlay.Markers.Add(marker);
                }
                else if(boolDrawLine || boolDrawPolygon)
                {

                    _points.Add(clickedPosition);
                }

            }
            
            if (e.Button == MouseButtons.Right)
            {
                if (boolDrawLine)
                {
                    AddLines(_points);
                }
                else if (boolDrawPolygon)
                {
                    AddPolygon(_points);
                }
                _points.Clear();
            }

            // Update the map
            //gmap.Refresh();
            
           
        }

        private void btnDropPin_Click(object sender, EventArgs e)
        {
            boolDropPin = (boolDropPin == true)? false : true;
            if (boolDropPin)
            {
                btnDropPin.BackColor = Color.Green;
                btnDrawLine.Enabled = false;
                btnDrawPolygon.Enabled = false;
            }
            else
            {
                btnDropPin.BackColor = Color.Black;
                btnDrawLine.Enabled = true;
                btnDrawPolygon.Enabled = true;
            }
            
        }

        private void btnDrawLine_Click(object sender, EventArgs e)
        {
            boolDrawLine = (boolDrawLine == true)? false : true;
            if (boolDrawLine)
            {
                btnDrawLine.BackColor = Color.Green;
                btnDropPin.Enabled = false;
                btnDrawPolygon.Enabled = false;
            }
            else
            {
                btnDrawLine.BackColor = Color.Black;
                btnDropPin.Enabled = true;
                btnDrawPolygon.Enabled = true;
            }
        }

        private void btnDrawPolygon_Click(object sender, EventArgs e)
        {
            boolDrawPolygon = (boolDrawPolygon == true)? false : true;
            if (boolDrawPolygon)
            {
                btnDrawPolygon.BackColor = Color.Green;
                btnDrawLine.Enabled = false;
                btnDropPin.Enabled = false;
            }
            else
            {
                btnDrawPolygon.BackColor = Color.Black;
                btnDrawLine.Enabled = true;
                btnDropPin.Enabled = true;
            }
        }
        private void AddMarkers()
        {
            // Create a marker at a specific location
            PointLatLng markerPosition = new PointLatLng(37.7749, -122.4194); // Example: San Francisco, CA

            // Create a marker object with a tooltip
            GMarkerGoogle marker = new GMarkerGoogle(markerPosition, GMarkerGoogleType.red);
            marker.ToolTipText = "San Francisco";
            marker.ToolTipText = "testing";
            marker.ToolTipMode = MarkerTooltipMode.Always;
            // Add the marker to the map's overlay
            //GMapOverlay markersOverlay = new GMapOverlay("markersOverlay");
            markersOverlay.Markers.Add(marker);
            gmap.Overlays.Add(markersOverlay);
        }

        private void AddLines(List<PointLatLng> pointsList)
        {
            // Create a line object
            GMapRoute line = new GMapRoute(pointsList, "line");

            // Set the route's stroke color and width
            line.Stroke = new Pen(Color.Red, 3);

            // Add the line to the map's overlay
            //GMapOverlay linesOverlay = new GMapOverlay("linesOverlay");
            linesOverlay.Routes.Add(line);
          //  gmap.Overlays.Add(linesOverlay);

            // Update the map
            gmap.Refresh();
        }

        private void AddPolygon(List<PointLatLng> pointsList)
        {
            // Create a polygon object
            GMapPolygon polygon = new GMapPolygon(pointsList, "polygon");

            // Set the polygon's fill color and stroke color
            polygon.Fill = new SolidBrush(Color.FromArgb(255, Color.Red));
            polygon.Stroke = new Pen(Color.Red, 3);

            // Add the polygon to the map's overlay
            //GMapOverlay polygonsOverlay = new GMapOverlay("polygonsOverlay");
            polygonsOverlay.Polygons.Add(polygon);
           // gmap.Overlays.Add(polygonsOverlay);
           
            // Update the map
            gmap.Refresh();
        }

        private void btnClearMarkers_Click(object sender, EventArgs e)
        {
            markersOverlay.Clear();
            linesOverlay.Clear();
            polygonsOverlay.Clear();
            //imagesOverlay.Clear();
            gmap.Refresh();
            
        }
       
       
    }
}
