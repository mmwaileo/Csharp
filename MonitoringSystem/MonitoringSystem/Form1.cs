using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//using static GMap.NET.Entity.OpenStreetMapGraphHopperGeocodeEntity;
using Point = System.Drawing.Point;

namespace MonitoringSystem
{
    public partial class Form1 : Form
    {
        System.Windows.Forms.Timer t_1Sec = new System.Windows.Forms.Timer();
        System.Windows.Forms.Timer t_5Sec = new System.Windows.Forms.Timer();
        System.Windows.Forms.Timer t_1Min = new System.Windows.Forms.Timer();

        private List<PointLatLng> _points;
        private GMapOverlay pinMarkerOverlay;
        private bool boolDropPin = false;
     
        GMapOverlay vehicleOverlay; // Overlay for the vehicle marker
        GMapOverlay compassOverlay; // Overlay for the compass marker
        GMapMarkerImage vehicleMarker;
        GMapMarkerImage compassMarker;
        int compassOffsetX, compassOffsetY;
        int compassWidth, compassHeight;
        public Form1()
        {
            InitializeComponent();
            InitializeGMap();
                       
        }

        private void InitializeGMap()
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
            gmap.ShowCenter = false;

            // Initialize the map control
            //gmap.MapProvider = GMap.NET.MapProviders.GoogleSatelliteMapProvider.Instance;
            gmap.MapProvider = GMapProviders.GoogleMap; // Set the map provider
            GMaps.Instance.Mode = AccessMode.ServerAndCache;
            gmap.Position = new PointLatLng(1.2657409598221026, 103.8607280276164); // Set the initial position of the map

            // Initialize overlays
            vehicleOverlay = new GMapOverlay("vehicleOverlay");
            compassOverlay = new GMapOverlay("compassOverlay");

            // Add overlays to the GMap control
            gmap.Overlays.Add(vehicleOverlay);
            gmap.Overlays.Add(compassOverlay);

            // Call methods to add the vehicle and compass markers
            AddVehicleMarker();
            AddCompassMarker();
            /*
            // Start a timer to update the markers every few milliseconds (adjust the interval as needed)
            Timer timer = new Timer();
            timer.Interval = 100; // Update every 100 milliseconds
            timer.Tick += Timer_Tick;
            timer.Start();
            */
            // Initialize the marker overlay
            pinMarkerOverlay = new GMapOverlay("markerOverlay");

            // Add the overlay to the GMap control
            gmap.Overlays.Add(pinMarkerOverlay);
            // Handle the OnMapClick event to drop pins (markers) when the mouse is clicked on the map
            gmap.OnMapClick += GMapControl_OnMapClick;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            //userControl11.Hide();
            //userControl21.Hide();
            //userControl31.Hide();

            //timer for 1 sec
            t_1Sec.Interval = 1000;    //in millisecond
            t_1Sec.Tick += new EventHandler(this.t_Tick_1Sec);
            t_1Sec.Start();

            //timer for 5 seconds
            t_5Sec.Interval = 5000;    //in millisecond
            t_5Sec.Tick += new EventHandler(this.t_Tick_5Sec);
            // t_5Sec.Start();

            //timer for 1 minute
            t_1Min.Interval = 60000;    //in millisecond
            t_1Min.Tick += new EventHandler(this.t_Tick_1Min);
            //t_1Min.Start();
            btnMonitoring.BackColor = Color.Green;
        }
        private void t_Tick_1Sec(object sender, EventArgs e)
        {
            //1 second timer

            // Get the current date
            DateTime currentDate = DateTime.Today;

            DateTime currentTime = DateTime.Parse(DateTime.Now.ToString());

            //lblDate.Text = currentDate.ToString("yyyy-MM-dd");

            lblDate.Text = currentDate.ToString("yyyy-MM-dd");
            lblTime.Text = currentTime.ToString("h:mm tt");
        }

        private void t_Tick_5Sec(object sender, EventArgs e)
        {
            //5 seconds timer

        }

        private void t_Tick_1Min(object sender, EventArgs e)
        {
            // 1 minute timer

        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            t_1Sec.Stop();
            t_5Sec.Stop();
            t_1Min.Stop();
        }

        private void btnMonitoring_Click(object sender, EventArgs e)
        {
            //Setting11.Hide();
            //Setting21.Hide();
            //Setting31.Hide();
            btnMonitoring.BackColor = Color.Green;
            btnSetting1.BackColor = Color.Black;
            btnSetting2.BackColor = Color.Black;
            btnSetting3.BackColor = Color.Black;
        }

        private void btnSetting1_Click(object sender, EventArgs e)
        {
            //Setting11.Hide();
            //Setting21.Hide();
            //Setting31.Hide();
            btnMonitoring.BackColor = Color.Black;
            btnSetting1.BackColor = Color.Green;
            btnSetting2.BackColor = Color.Black;
            btnSetting3.BackColor = Color.Black;
        }

        private void btnSetting2_Click(object sender, EventArgs e)
        {
            //Setting11.Hide();
            //Setting21.Hide();
            //Setting31.Hide();
            btnMonitoring.BackColor = Color.Black;
            btnSetting1.BackColor = Color.Black;
            btnSetting2.BackColor = Color.Green;
            btnSetting3.BackColor = Color.Black;
        }

        private void btnSetting3_Click(object sender, EventArgs e)
        {
            //Setting11.Hide();
            //Setting21.Hide();
            //Setting31.Hide();
            btnMonitoring.BackColor = Color.Black;
            btnSetting1.BackColor = Color.Black;
            btnSetting2.BackColor = Color.Black;
            btnSetting3.BackColor = Color.Green;
        }
        
        private void btnDropPin_Click(object sender, EventArgs e)
        {
            boolDropPin = (boolDropPin == true) ? false : true;
            if (boolDropPin)
            {
                btnDropPin.BackColor = Color.Green;
                
            }
            else
            {
                btnDropPin.BackColor = Color.Black;

            }
        }
        private void GMapControl_OnMapClick(PointLatLng point, MouseEventArgs e)
        {
            if (boolDropPin)
            {
                // Drop a new marker at the clicked location
                GMarkerGoogle newMarker = new GMarkerGoogle(point, GMarkerGoogleType.red);
                pinMarkerOverlay.Markers.Add(newMarker);

                // Refresh the GMap control to show the newly added marker
                gmap.Refresh();
            }

        }
        private void btnClear_Click(object sender, EventArgs e)
        {
            pinMarkerOverlay.Clear();
            gmap.Refresh();
        }

        private void gmap_MouseClick(object sender, MouseEventArgs e)
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

                    pinMarkerOverlay.Markers.Add(marker);
                }
                
            }
        }
        private void AddVehicleMarker()
        {
            // Replace the vehicleCoordinates with the actual GPS coordinates of your vehicle
            PointLatLng vehicleCoordinates = new PointLatLng(1.2657409598221026, 103.8607280276164);

            // Load the vehicle image (replace 'vehicle.png' with the path to your vehicle image)
            Image originalVehicleImage = Image.FromFile("img_files/polygon_top_415.png");

            // Calculate the new width and height (50% of the original size)
            int newWidth = originalVehicleImage.Width / 4;
            int newHeight = originalVehicleImage.Height / 4;

            // Create a new Bitmap with the new dimensions
            Bitmap vehicleImage = new Bitmap(newWidth, newHeight);

            // Create a Graphics object to draw on the new Bitmap
            using (Graphics graphics = Graphics.FromImage(vehicleImage))
            {
                // Set the interpolation mode to high quality for better resizing
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

                // Draw the original image on the new Bitmap with resizing
                graphics.DrawImage(originalVehicleImage, 0, 0, newWidth, newHeight);
            }

            // Create a new GMapMarkerImage instance for the vehicle marker
            vehicleMarker = new GMapMarkerImage(vehicleCoordinates, vehicleImage);
            vehicleMarker.ToolTipText = "Vehicle"; // Tooltip for the vehicle marker

            vehicleMarker.Bearing = 0;
            /*
            int vehicleIconWidth = (vehicleImage.Width);
            int vehicleIconHeight = (vehicleImage.Height);
            vehicleIconWidthHalf = vehicleIconWidth / 2;
            vehicleIconHeightHalf = vehicleIconHeight / 2;

            vehicleOffsetX = (int)(vehicleIconWidthHalf * (1 - Math.Cos(vehicleMarker.Bearing * (Math.PI) / 180)));
            vehicleOffsetY = (int)(vehicleIconWidthHalf * Math.Sin(vehicleMarker.Bearing * (Math.PI) / 180));

            //vehicleMarker.Offset = new Point(-vehicleIconWidth, -vehicleIconHeightHalf);
            //   vehicleMarker.Offset = new Point(-(int)(vehicleIconWidth - vehicleOffsetX), -(int)(vehicleIconHeightHalf + vehicleOffsetY));
            //   vehicleMarker.Offset = new Point(-(int)(vehicleIconWidthHalf + vehicleOffsetX), -(int)(vehicleIconHeightHalf));
            //  vehicleMarker.Offset = new Point(-(int)(vehicleIconWidthHalf), -(int)(vehicleIconHeightHalf));
            */


            // Add the marker to the vehicle overlay
            vehicleOverlay.Markers.Add(vehicleMarker);
        }

        private void AddCompassMarker()
        {
            // Replace the compassCoordinates with the actual GPS coordinates of your compass
            // PointLatLng compassCoordinates = new PointLatLng(37.7849, -122.4294);

            // Load the compass image (replace 'compass.png' with the path to your compass image)
            Image originalCompassImage = Image.FromFile("img_files/image.ico");

            // Calculate the new width and height (50% of the original size)
            int newWidth = originalCompassImage.Width / 4;
            int newHeight = originalCompassImage.Height / 4;

            // Create a new Bitmap with the new dimensions
            Bitmap compassImage = new Bitmap(newWidth, newHeight);

            // Create a Graphics object to draw on the new Bitmap
            using (Graphics graphics = Graphics.FromImage(compassImage))
            {
                // Set the interpolation mode to high quality for better resizing
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

                // Draw the original image on the new Bitmap with resizing
                graphics.DrawImage(originalCompassImage, 0, 0, newWidth, newHeight);
            }

            // Create a new GMapMarkerImage instance for the compass marker
            //compassMarker = new GMapMarkerImage(compassCoordinates, compassImage);
            compassMarker = new GMapMarkerImage(gmap.ViewArea.LocationRightBottom, compassImage);
            compassMarker.ToolTipText = "Compass"; // Tooltip for the compass marker

            compassWidth = compassImage.Width;
            compassHeight = compassImage.Height;
            compassOffsetX = (int)(compassWidth * Math.Cos(compassMarker.Bearing));
            compassOffsetY = (int)(compassHeight * Math.Sin(compassMarker.Bearing));
            compassMarker.Offset = new Point(-(compassWidth), -(compassHeight));

            // Add the marker to the compass overlay
            compassOverlay.Markers.Add(compassMarker);
            // Subscribe to the map control's OnPositionChanged event
            gmap.OnPositionChanged += gMapControl1_OnPositionChanged;
            gmap.OnMapZoomChanged += gMapControl1_OnMapZoomChanged;
        }
        private void gMapControl1_OnPositionChanged(PointLatLng point)
        {
            // Update the custom marker position to the bottom right corner of the screen
            compassMarker.Position = gmap.ViewArea.LocationRightBottom;//new PointLatLng(gmap.ViewArea.Right, gmap.ViewArea.Bottom); 
            compassMarker.Offset = new Point(-(compassWidth + compassOffsetY), -(compassHeight - compassOffsetY));

            // Update the custom marker rotation based on the current map bearing
            compassMarker.Bearing = -(float)gmap.Bearing;
        }

        private void gMapControl1_OnMapZoomChanged()
        {
            // Update the custom marker position when the map zoom changes

            // Update the custom marker position to the bottom right corner of the screen
            //compassMarker.Position = gmap.ViewArea.LocationRightBottom;//new PointLatLng(gmap.ViewArea.Right, gmap.ViewArea.Bottom); 

            //Update the custom marker rotation based on the current map bearing
            //compassMarker.Rotation = (float)gmap.Bearing;
        }
        private void DrawPolygonWithCenterText(List<PointLatLng> points, string text)
        {
            // Create the overlay for polygons and markers
            GMapOverlay gridOverlay = new GMapOverlay("Grids");
            gmap.Overlays.Add(gridOverlay);

            // Add the polygon to the overlay
            GMapPolygon polygon = new GMapPolygon(points, text);
            gridOverlay.Polygons.Add(polygon);

            // Calculate the center of the polygon
            PointLatLng center = CalculatePolygonCenter(points);

            // Add text marker at the center of the grid
            GMarkerText marker = new GMarkerText(center, text, new Font(FontFamily.GenericSansSerif, 12), Brushes.Red)
            {
                //Offset = new Point(-20, -20)
                Offset = new Point(0, 0)
            };
            gridOverlay.Markers.Add(marker);
            gmap.Refresh();
        }

        private PointLatLng CalculatePolygonCenter(List<PointLatLng> points)
        {
            double lat = 0;
            double lng = 0;

            foreach (var point in points)
            {
                lat += point.Lat;
                lng += point.Lng;
            }

            int count = points.Count;
            return new PointLatLng(lat / count, lng / count);
        }
    }
    public class GMarkerText : GMapMarker
    {
        public string Text { get; set; }
        public Font Font { get; set; }
        public Brush TextBrush { get; set; }

        public GMarkerText(PointLatLng pos, string text, Font font, Brush textBrush)
            : base(pos)
        {
            Text = text;
            Font = font;
            TextBrush = textBrush;
        }

        public override void OnRender(Graphics g)
        {
            // Get the size of the text
            SizeF textSize = g.MeasureString(Text, Font);

            // Calculate the position for the text
            float x = LocalPosition.X - textSize.Width / 2;
            float y = LocalPosition.Y - textSize.Height / 2;

            // Draw the text on the marker
            g.DrawString(Text, Font, TextBrush, x, y);
        }
    }
    public class GMapMarkerImage : GMapMarker
    {
        private Image markerImage; // The image used for the marker
        private float bearing; // The rotation angle in degrees
        private SizeF size; // The size of the marker

        public GMapMarkerImage(PointLatLng pos, Image image) : base(pos)
        {
            // Initialize the marker image, bearing, and size
            markerImage = image;
            bearing = 0.0f; // Default rotation angle is 0 degrees
            size = new SizeF(markerImage.Width, markerImage.Height); // Default size is the original image size
        }

        public Image MarkerImage
        {
            get { return markerImage; }
            set
            {
                markerImage = value;
                // Update the marker size based on the new image size
                size = new SizeF(markerImage.Width, markerImage.Height);

                // Redraw the marker with the updated image
                if (Overlay != null && Overlay.Control != null)
                    Overlay.Control.Invalidate();
            }
        }

        public float Bearing
        {
            get { return bearing; }
            set
            {
                // Normalize the rotation angle to the range [0, 360) degrees
                bearing = value % 360;
                if (bearing < 0)
                    bearing += 360;

                // Redraw the marker with the updated rotation
                if (Overlay != null && Overlay.Control != null)
                    Overlay.Control.Invalidate();
            }
        }
        /*
        public SizeF Size
        {
            get { return size; }
            set
            {
                size = value;
                // Redraw the marker with the updated size
                if (Overlay != null && Overlay.Control != null)
                    Overlay.Control.Invalidate();
            }
        }
        */
        public override void OnRender(Graphics g)
        {
            if (markerImage != null)
            {
                // Save the current state of the graphics object
                GraphicsState state = g.Save();

                // Apply the rotation and translation to the graphics object
                g.TranslateTransform((float)LocalPosition.X, (float)LocalPosition.Y);
                g.RotateTransform(bearing);
                g.TranslateTransform(-size.Width, -size.Height / 2);

                // Draw the marker's image at the marker's position with the specified size
                //g.DrawImage(markerImage, new RectangleF(0, 0, size.Width, size.Height));
                //  g.DrawImage(markerImage, markerImage.Width, markerImage.Height, markerImage.Width, markerImage.Height);
                g.DrawImage(markerImage, new Point(0, 0));

                // Restore the graphics object to its original state
                g.Restore(state);
            }
        }

        public override void Dispose()
        {
            // Dispose of the marker image and other resources (if any)

            base.Dispose();
        }
    }

}
