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
using UTMLatLngConversionLibrary;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms.Markers;
using static UTMLatLngConversionLibrary.LatLngUTMConvert;
using Microsoft.VisualBasic.FileIO;

namespace System_Monitoring
{
    public partial class Form1 : Form
    {
        LatLngUTMConvert utmLatLngConvert = new LatLngUTMConvert("WGS 84");
        string objectColor;
        bool isObjectCSVFileOpen = false;
        bool isGridCSVFileOpen = false;
        bool isObjectLineDrawn = false;
        bool isGridLineDrawn = false;
        private GMapOverlay linesOverlay;
        private GMapOverlay gridOverlay;
        List<PointLatLng> objectLatLngList = new List<PointLatLng>();
        //List<ObjectLatLng> objectDataList = new List<ObjectLatLng>();
        List<GridLineData> gridLineDataList = new List<GridLineData>();

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

        private Image originalImage;
        private Image polygonSideImage;
        private Bitmap polygonScaledImage;

        private void Form1_Load(object sender, EventArgs e)
        {
            //setting11.Hide();
            //setting21.Hide();
            //setting31.Hide();

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

        public Form1()
        {
            InitializeComponent();
            InitializeGMap();
            LoadImageIntoPictureBox();
        }
        private void t_Tick_1Sec(object sender, EventArgs e)
        {
            //1 second timer

            // Get the current date
            DateTime currentDate = DateTime.Today;

            DateTime currentTime = DateTime.Parse(DateTime.Now.ToString());

            //lblDate.Text = currentDate.ToString("yyyy-MM-dd");

            lblDate.Text = currentDate.ToString("yyyy-MM-dd");
            //lblTime.Text = currentTime.ToString("h:mm tt");
            lblTime.Text = currentTime.ToString("HHmm");
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
        private void LoadImageIntoPictureBox()
        {
            // Load the image file (replace "path_to_your_image.png" with the actual path to your image file)
            polygonSideImage = Image.FromFile("img_files/polygon_side.png");
            // Calculate the new width and height (50% of the original size)
            /*
            int newWidth = polygonSideImage.Width / 3;
            int newHeight = polygonSideImage.Height / 3;

            // Create a new Bitmap with the new dimensions
            Bitmap polygonScaledImage = new Bitmap(newWidth, newHeight);

            // Create a Graphics object to draw on the new Bitmap
            using (Graphics graphics = Graphics.FromImage(polygonScaledImage))
            {
                // Set the interpolation mode to high quality for better resizing
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

                // Draw the original image on the new Bitmap with resizing
                graphics.DrawImage(polygonSideImage, 0, 0, newWidth, newHeight);
            }
            */
            // Display the original image in the PictureBox control
            // pictureBox1.Location = new Point(pictureBox1.Left+100, pictureBox1.Top+50);
            pictureBox1.Image = polygonSideImage;

            int a = pictureBox1.Width - pictureBox1.Image.Width;
            int b = pictureBox1.Height - pictureBox1.Image.Height;

            //int a = pictureBox1.Width - scaledImage.Width;
            //int b = pictureBox1.Height - scaledImage.Height;
            Padding p = new System.Windows.Forms.Padding();
            p.Left = a / 2;
            p.Top = b / 3;
            pictureBox1.Padding = p;
            //pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
        }
        /*
        private void btnRotate_Click(object sender, EventArgs e)
        {
            // Rotate the image by 45 degrees clockwise (you can change the angle as desired)
            Image rotatedImage = RotateImage(polygonSideImage, -2);

            // Display the rotated image in the PictureBox control
            pictureBox1.Image = rotatedImage;
        }
        */
        // Method to rotate an image by a specified angle
        public Image RotateImage(Image image, float angle)
        {
            // Create a new bitmap with the same width and height to hold the rotated image
            Bitmap rotatedBitmap = new Bitmap(image.Width, image.Height);

            // Set the resolution of the new bitmap to match the original image
            rotatedBitmap.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            // Create a Graphics object from the new bitmap to draw the rotated image
            using (Graphics graphics = Graphics.FromImage(rotatedBitmap))
            {
                // Set the interpolation mode to high quality for smoother rotation
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBilinear;

                // Translate the rotation point to the center of the image
                graphics.TranslateTransform(image.Width / 6f, image.Height / 3f);

                // Rotate the image by the specified angle
                graphics.RotateTransform(angle);

                // Draw the rotated image back to the original position
                graphics.DrawImage(image, -image.Width / 6f, -image.Height / 3f, image.Width, image.Height);
            }

            return rotatedBitmap;
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
            linesOverlay = new GMapOverlay("linesOverlay");
            // Create the overlay for polygons and markers
            gridOverlay = new GMapOverlay("Grids");


            // Add the overlay to the GMap control
            gmap.Overlays.Add(pinMarkerOverlay);
            gmap.Overlays.Add(linesOverlay);
            gmap.Overlays.Add(gridOverlay);
            // Handle the OnMapClick event to drop pins (markers) when the mouse is clicked on the map
            gmap.OnMapClick += GMapControl_OnMapClick;
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
            Image originalCompassImage = Image.FromFile("img_files/compass.png");

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

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            // Specify the line's properties
            int lineY = (pictureBox1.Height) / 2; // Y-coordinate for the horizontal line
            int lineWidth = pictureBox1.Width; // Width of the line
            Pen linePen = new Pen(Color.Blue, 4); // Pen for drawing the line

            // Draw the horizontal line on the PictureBox
            e.Graphics.DrawLine(linePen, 0, lineY, lineWidth, lineY);

            // Dispose of the Pen to release resources
            linePen.Dispose();
        }

        private void btnMonitoring_Click(object sender, EventArgs e)
        {
            //setting11.Hide();
            //Setting21.Hide();
            //setting31.Hide();
            btnMonitoring.BackColor = Color.Green;
            btnSetting1.BackColor = Color.Black;
            btnSetting2.BackColor = Color.Black;
            btnSetting3.BackColor = Color.Black;
        }

        private void btnSetting1_Click(object sender, EventArgs e)
        {
            //setting11.Show();
            //setting21.Hide();
            //setting31.Hide();
            btnMonitoring.BackColor = Color.Black;
            btnSetting1.BackColor = Color.Green;
            btnSetting2.BackColor = Color.Black;
            btnSetting3.BackColor = Color.Black;
        }

        private void btnSetting2_Click(object sender, EventArgs e)
        {
            //setting11.Hide();
            //setting21.Hide();
            //setting31.Hide();
            btnMonitoring.BackColor = Color.Black;
            btnSetting1.BackColor = Color.Black;
            btnSetting2.BackColor = Color.Green;
            btnSetting3.BackColor = Color.Black;
        }

        private void btnSetting3_Click(object sender, EventArgs e)
        {
            //setting11.Hide();
            //setting21.Hide();
            //setting31.Show();
            
            btnMonitoring.BackColor = Color.Black;
            btnSetting1.BackColor = Color.Black;
            btnSetting2.BackColor = Color.Black;
            btnSetting3.BackColor = Color.Green;
        }

        private void btnSetObject_Click(object sender, EventArgs e)
        {
            if (!isObjectCSVFileOpen)
            {
                OpenFileDialog ofd = new OpenFileDialog()
                {
                    Title = "Open CSV File",
                    Filter = "csv files(*.csv)|*.csv",
                    //CheckFileExists = true,
                    //CheckPathExists = true
                };

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        //List<ObjectLatLng> objectDataList = new List<ObjectLatLng>();

                        using (TextFieldParser parser = new TextFieldParser(ofd.FileName))
                        {
                            parser.SetDelimiters(",");
                            //parser.ReadLine(); // Skip the first header line
                            string[] temFields = parser.ReadFields();
                            objectColor = temFields[0];
                            while (!parser.EndOfData)
                            {
                                string[] fields = parser.ReadFields();
                                if (fields.Length >= 2)
                                {
                                    LatLng tempLatLng1 = utmLatLngConvert.convertUtmToLatLng(double.Parse(fields[0]), double.Parse(fields[1]), 48, "S");

                                    objectLatLngList.Add(new PointLatLng(tempLatLng1.Lat, tempLatLng1.Lng));
                                }

                            }
                        }
                        isObjectCSVFileOpen = true;

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error");
                        //Console.WriteLine("An error occurred: " + ex.Message);
                    }
                }
            }
            if (objectLatLngList.Count > 0)
            {
                //draw object line
                if (!isObjectLineDrawn)
                {
                    AddLines(objectLatLngList, objectColor);
                    isObjectLineDrawn = true;
                }
                else
                {
                    linesOverlay.Clear();
                    gmap.Refresh();
                    isObjectLineDrawn = false;
                }
            }
        }

        private void btnSetGrid_Click(object sender, EventArgs e)
        {
            List<PointLatLng> totalPolygonPoints = new List<PointLatLng>();
            if (!isGridCSVFileOpen)
            {
                OpenFileDialog ofd = new OpenFileDialog()
                {
                    Title = "Open CSV File",
                    Filter = "csv files(*.csv)|*.csv",
                    //CheckFileExists = true,
                    //CheckPathExists = true
                };

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        //List<GridLineData> gridLineDataList = new List<GridLineData>();

                        using (TextFieldParser parser = new TextFieldParser(ofd.FileName))
                        {
                            parser.SetDelimiters(",");
                            parser.ReadLine(); // Skip the first header line

                            while (!parser.EndOfData)
                            {
                                string[] fields = parser.ReadFields();

                                if (fields.Length >= 11)
                                {
                                    LatLng tempLatLng1 = utmLatLngConvert.convertUtmToLatLng(double.Parse(fields[3]), double.Parse(fields[4]), 48, "S");
                                    LatLng tempLatLng2 = utmLatLngConvert.convertUtmToLatLng(double.Parse(fields[5]), double.Parse(fields[6]), 48, "S");
                                    LatLng tempLatLng3 = utmLatLngConvert.convertUtmToLatLng(double.Parse(fields[7]), double.Parse(fields[8]), 48, "S");
                                    LatLng tempLatLng4 = utmLatLngConvert.convertUtmToLatLng(double.Parse(fields[9]), double.Parse(fields[10]), 48, "S");

                                    List<double> tempLatList = new List<double> { tempLatLng1.Lat, tempLatLng2.Lat, tempLatLng3.Lat, tempLatLng4.Lat };
                                    List<double> tempLngList = new List<double> { tempLatLng1.Lng, tempLatLng2.Lng, tempLatLng3.Lng, tempLatLng4.Lng };

                                    GridLineData gridData = new GridLineData

                                    {
                                        area = fields[0],
                                        target_height = double.Parse(fields[1]),
                                        tolerance = double.Parse(fields[2]),
                                        x1 = tempLatLng1.Lat,
                                        y1 = tempLatLng1.Lng,
                                        x2 = tempLatLng2.Lat,
                                        y2 = tempLatLng2.Lng,
                                        x3 = tempLatLng3.Lat,
                                        y3 = tempLatLng3.Lng,
                                        x4 = tempLatLng4.Lat,
                                        y4 = tempLatLng4.Lng,
                                        //min_lat = tempLatList.Min(),
                                        //max_lat = tempLatList.Max(),
                                        //min_lng = tempLngList.Min(),
                                        //max_lng = tempLngList.Max(),
                                        //gridPointsList = { new PointLatLng(tempLatLng1.Lat, tempLatLng1.Lng), 
                                        //     new PointLatLng(tempLatLng2.Lat, tempLatLng2.Lng), new PointLatLng(tempLatLng3.Lat, tempLatLng3.Lng), 
                                        //     new PointLatLng(tempLatLng4.Lat, tempLatLng4.Lng) },
                                        gridPoint1 = new PointLatLng(tempLatLng1.Lat, tempLatLng1.Lng),
                                        gridPoint2 = new PointLatLng(tempLatLng2.Lat, tempLatLng2.Lng),
                                        gridPoint3 = new PointLatLng(tempLatLng3.Lat, tempLatLng3.Lng),
                                        gridPoint4 = new PointLatLng(tempLatLng4.Lat, tempLatLng4.Lng),
                                    };
                                    gridLineDataList.Add(gridData);
                                }
                            }
                        }

                        isGridCSVFileOpen = true;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error");
                        //Console.WriteLine("An error occurred: " + ex.Message);
                    }
                }
            }
            if (gridLineDataList.Count > 0)
            {
                //draw polygon and insert text
                if (!isGridLineDrawn)
                {
                    for (int i = 0; i < gridLineDataList.Count; i++)
                    {
                        List<PointLatLng> points = new List<PointLatLng>
                        {
                            gridLineDataList[i].gridPoint1,
                            gridLineDataList[i].gridPoint2,
                            gridLineDataList[i].gridPoint3,
                            gridLineDataList[i].gridPoint4,
                        };
                        DrawPolygonWithCenterText(points, gridLineDataList[i].area);
                        totalPolygonPoints.AddRange(points);
                    }
                    PointLatLng center = CalculatePolygonCenter(totalPolygonPoints);
                    gmap.Position = center;
                    gmap.Zoom = 17;
                    isGridLineDrawn = true;
                }
                else
                {
                    gridOverlay.Polygons.Clear();
                    gridOverlay.Markers.Clear();
                    gmap.Refresh();
                    isGridLineDrawn = false;
                }
            }
        }

        private void btnScreenShot_Click(object sender, EventArgs e)
        {
            try
            {
                // Get the bounds of the primary screen
                // Rectangle bounds = Screen.PrimaryScreen.Bounds;

                // Get the bounds of entire screen
                Rectangle bounds = Screen.GetBounds(Point.Empty);

                // Get the bounds of all connected screens
                // Rectangle bounds = GetCombinedScreenBounds();

                // Create a bitmap to hold the captured image
                //using (Bitmap screenshot = new Bitmap((int)(bounds.Width* scalingFactor), (int)(bounds.Height* scalingFactor)))
                using (Bitmap screenshot = new Bitmap(bounds.Width, bounds.Height))
                {
                    // Create a Graphics object from the bitmap
                    using (Graphics graphics = Graphics.FromImage(screenshot))
                    {
                        // Copy the screen contents to the bitmap
                        graphics.CopyFromScreen(bounds.X, bounds.Y, 0, 0, bounds.Size);
                    }

                    // Show a SaveFileDialog to choose the save location
                    SaveFileDialog saveFileDialog = new SaveFileDialog
                    {
                        Filter = "PNG Image|*.png",
                        Title = "Save Entire Screen Screenshot",
                        FileName = "monitor_screen_screenshot.png"
                    };

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        // Save the bitmap to the selected file
                        screenshot.Save(saveFileDialog.FileName, System.Drawing.Imaging.ImageFormat.Png);
                        MessageBox.Show("Screenshot saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error while saving screenshot: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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

        private void btnClear_Click(object sender, EventArgs e)
        {
            pinMarkerOverlay.Clear();
            gmap.Refresh();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {

        }

        private void btnComplete_Click(object sender, EventArgs e)
        {

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

        private void btnAutolet_Click(object sender, EventArgs e)
        {

        }

        private void btnLevelSensor_Click(object sender, EventArgs e)
        {

        }
        private void AddLines(List<PointLatLng> pointsList, string color)
        {
            string tempColor = color.ToLower();
            Color lineColor = new Color();
            switch (tempColor)
            {
                case "magenta":
                    //code block
                    lineColor = Color.Magenta;
                    break;
                case "red":
                    //code block
                    lineColor = Color.Red;
                    break;
                case "blue":
                    //code block
                    lineColor = Color.Blue;
                    break;
                default:
                    lineColor = Color.Magenta;
                    break;
            }
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
        private void DrawPolygonWithCenterText(List<PointLatLng> points, string areaName)
        {
            /*
            // Create the overlay for polygons and markers
            GMapOverlay gridOverlay = new GMapOverlay("Grids");
            gmap.Overlays.Add(gridOverlay);
            */
            // Add the polygon to the overlay
            GMapPolygon polygon = new GMapPolygon(points, areaName);
            gridOverlay.Polygons.Add(polygon);

            // Calculate the center of the polygon
            PointLatLng center = CalculatePolygonCenter(points);

            // Add text marker at the center of the grid
            GMarkerText marker = new GMarkerText(center, areaName, new Font(FontFamily.GenericSansSerif, 12), Brushes.Red)
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
}
