using GMap.NET.WindowsForms;
using GMap.NET;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GMap.NET.WindowsPresentation;
using GMapMarker = GMap.NET.WindowsForms.GMapMarker;
using GMap.NET.WindowsForms.Markers;

namespace GMapVehicleIconChangeAccordingToAzimuth
{
    public partial class Form1 : Form
    {
        private GMapOverlay VehicleMarkerOverlay;
        private VehicleMarker vehicleMarker;
        private Timer updateTimer;
        private double currentAzimuth = -10.0; // Replace this with the initial azimuth value
        private double iconWidth, iconHeight;
        private double iconWidthHalf, iconHeightHalf;
        private int offsetX, offsetY;
        private double zoomRatio;

        private GMapMarker imageMarker;

        private void gmap_OnMapZoomChanged()
        {
            zoomRatio = 18 / gmap.Zoom;
        }

        public Form1()
        {
            InitializeComponent();
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

            gmap.NegativeMode = false;
            gmap.PolygonsEnabled = true;
            gmap.RoutesEnabled = true;
            gmap.ShowTileGridLines = false;
            gmap.Zoom = 10;

            // Initialize the map control
            gmap.MapProvider = GMap.NET.MapProviders.GoogleSatelliteMapProvider.Instance;
            GMaps.Instance.Mode = AccessMode.ServerAndCache;
            gmap.Position = new PointLatLng(1.2657409598221026, 103.8607280276164); // Set the initial position of the map (New York City, USA)
                                                                                 // gmap.Zoom = 10; // Set the initial zoom level of the map
            CreateVehicleMarker();
        }
        private void CreateVehicleMarker()
        {
            // Create an VehicleMarkerOverlay for the vehicle marker
            VehicleMarkerOverlay = new GMapOverlay("VehicleMarkerOverlay");
            gmap.Overlays.Add(VehicleMarkerOverlay);
            /*
            // Load the vehicle icon to be used as an overlay
            Image vehicleIcon = Image.FromFile("img/polygon_top.png"); // Replace with the actual path to your vehicle icon

            // Set the latitude and longitude where you want to place the vehicle
            double vehicleLatitude = 40.7128; // Replace with the latitude of the vehicle
            double vehicleLongitude = -74.0060; // Replace with the longitude of the vehicle

            // Create a custom marker with the vehicle icon at the specified location
            imageMarker = new GMarkerGoogle(new PointLatLng(vehicleLatitude, vehicleLongitude), (Bitmap)vehicleIcon);
            imageMarker.Size = new Size(vehicleIcon.Width / 4, vehicleIcon.Height / 4);
            imageMarker.Offset = new Point(-vehicleIcon.Width, -vehicleIcon.Height / 2); //move the centre to right centre point

            overlay.Markers.Add(imageMarker);
            */

            // Load the vehicle icon to be used as an VehicleMarkerOverlay
            Image originalImage = Image.FromFile("img/polygon_top_415.png"); // Replace with the actual path to your vehicle icon

            // Calculate the new width and height (50% of the original size)
            int newWidth = originalImage.Width / 4;
            int newHeight = originalImage.Height / 4;

            // Create a new Bitmap with the new dimensions
            Bitmap vehicleIcon = new Bitmap(newWidth, newHeight);

            // Create a Graphics object to draw on the new Bitmap
            using (Graphics graphics = Graphics.FromImage(vehicleIcon))
            {
                // Set the interpolation mode to high quality for better resizing
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

                // Draw the original image on the new Bitmap with resizing
                graphics.DrawImage(originalImage, 0, 0, newWidth, newHeight);
            }

            iconWidth = (int)(vehicleIcon.Width);
            iconHeight = (int)(vehicleIcon.Height);
            iconWidthHalf = iconWidth / 2;
            iconHeightHalf = iconHeight / 2;
            offsetX = (int)(iconWidthHalf * Math.Cos(currentAzimuth * (Math.PI) / 180));
            offsetY = (int)(iconWidthHalf * Math.Sin(currentAzimuth * (Math.PI) / 180));

            //vehicleIcon.Size = new Size(vehicleIcon.Width / 4, vehicleIcon.Height / 4);
            // Set the latitude and longitude where you want to place the vehicle
            double vehicleLatitude = 1.2657409598221026; // Replace with the latitude of the vehicle
            double vehicleLongitude = 103.8607280276164; // Replace with the longitude of the vehicle

            // Create a custom marker with the vehicle icon at the specified location
            vehicleMarker = new VehicleMarker(new PointLatLng(vehicleLatitude, vehicleLongitude), vehicleIcon);
            //vehicleMarker.Size = new Size(vehicleIcon.Width, vehicleIcon.Height);
            vehicleMarker.Offset = new Point(-(int)(iconWidthHalf + offsetX), -(int)(iconHeightHalf + offsetY)); //move the centre to right centre point
            //vehicleMarker.Offset = new Point(-(int)iconWidth, -(int)iconHeightHalf); //move the centre to right centre point
            VehicleMarkerOverlay.Markers.Add(vehicleMarker);

            /*
            // Set up the timer to update the vehicle's bearing (azimuth)
            updateTimer = new Timer();
            updateTimer.Interval = 100; // Set the update interval in milliseconds
            updateTimer.Tick += UpdateTimer_Tick;
            updateTimer.Start();
            */
            vehicleMarker.SetDirection(currentAzimuth);
        }
        /*
        private void UpdateTimer_Tick(object sender, EventArgs e)
        {
            // Simulate changing azimuth for demonstration purposes
            currentAzimuth += 2.0;
            if (currentAzimuth >= 360)
                currentAzimuth -= 360;

            // Update the vehicle's bearing (azimuth)
            vehicleMarker.SetDirection(currentAzimuth);
            //gmap.Refresh(); // Refresh the map to apply the changes
        }
        */
    }
    // Custom marker class that extends GMapMarker
    public class VehicleMarker : GMapMarker
    {
        private Image _vehicleIcon;
        private double _directionAngle;

        public VehicleMarker(PointLatLng pos, Image vehicleIcon) : base(pos)
        {
            _vehicleIcon = vehicleIcon;
            _directionAngle = 0.0; // Default direction angle is 0 degrees (facing north)
            //Size = new System.Drawing.Size(vehicleIcon.Width, vehicleIcon.Height);
        }

        public void SetDirection(double angleInDegrees)
        {
            _directionAngle = angleInDegrees;
            if (_directionAngle < 0)
                _directionAngle += 360; // Normalize the angle to be in the range [0, 360)
            if (_directionAngle >= 360)
                _directionAngle -= 360;
            //update();
            
        }

        public override void OnRender(Graphics g)
        {
            
            // Rotate the vehicle icon before drawing
            Image rotatedIcon = RotateImage(_vehicleIcon, (float)_directionAngle);

            g.DrawImage(rotatedIcon, LocalPosition.X, LocalPosition.Y, rotatedIcon.Width, rotatedIcon.Height);
        }

        private Image RotateImage(Image image, float angle)
        {
            Bitmap rotatedImage = new Bitmap(image.Width, image.Height);
            using (Graphics g = Graphics.FromImage(rotatedImage))
            {
                // Set the rotation point to the center of the image
                g.TranslateTransform(image.Width/2, image.Height/2);
                g.RotateTransform(angle);
                g.TranslateTransform(-image.Width/2, -image.Height/2);
                g.DrawImage(image, new Point(0, 0));
            }
            return rotatedImage;
        }
    }
}
