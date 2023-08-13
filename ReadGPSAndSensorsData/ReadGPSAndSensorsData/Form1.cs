using GMap.NET;
using Sharp7;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ReadGPSAndSensorsData
{
    public partial class Form1 : Form
    {
        private Image polygonSideImage;

        System.Windows.Forms.Timer t_1Sec = new System.Windows.Forms.Timer();
        System.Windows.Forms.Timer t_5Sec = new System.Windows.Forms.Timer();
        System.Windows.Forms.Timer t_1Min = new System.Windows.Forms.Timer();

        List<GridLineData> gridLineDataList = new List<GridLineData>();
        PointLatLng currentGPSPoint;
        string workingAreaName = "";

        Random random = new Random();
        // Set the sample data for the chart
        //List<double> yValues1, yValues2, yValues3;
        int[] xValues = new int[60];
        int[] P1ReadingA = new int[60];
        int[] P2ReadingA = new int[60];
        int[] P3ReadingA = new int[60];
        int[] P1ReadingB = new int[60];
        int[] P2ReadingB = new int[60];
        int[] P3ReadingB = new int[60];
        bool isGroupA = true;
        public Form1()
        {
            InitializeComponent();
            LoadImageIntoPictureBox();
            InitXData();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            //timer for 1 sec
            t_1Sec.Interval = 1000;    //in millisecond
            t_1Sec.Tick += new EventHandler(this.t_Tick_1Sec);
            t_1Sec.Start();

            //timer for 5 seconds
            t_5Sec.Interval = 5000;    //in millisecond
            t_5Sec.Tick += new EventHandler(this.t_Tick_5Sec);
            t_5Sec.Start();

            //timer for 1 minute
            t_1Min.Interval = 60000;    //in millisecond
            t_1Min.Tick += new EventHandler(this.t_Tick_1Min);
            //t_1Min.Start();
            // btnMonitoring.BackColor = Color.Green;
        }
        private void t_Tick_1Sec(object sender, EventArgs e)
        {
            //1 second timer
            ShowDateTime();
        }

        private void t_Tick_5Sec(object sender, EventArgs e)
        {
            //5 seconds timer
            PlotChart();
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
        private void ShowDateTime()
        {
            // Get the current date
            DateTime currentDate = DateTime.Today;
            DateTime currentTime = DateTime.Parse(DateTime.Now.ToString());

            //lblDate.Text = currentDate.ToString("yyyy-MM-dd");
            lblDate.Text = currentDate.ToString("yyyy-MM-dd");
            //lblTime.Text = currentTime.ToString("h:mm tt");
            lblTime.Text = currentTime.ToString("HHmm");
        }
        public void InitXData()
        {
            for (int i = 0; i < 60; i++)
            {
                xValues[i] = (i - 59);
            }
        }
        private void PlotChart()
        {
            if (isGroupA)
            {
                for (int i = 0; i < 60; i++)
                {
                    P1ReadingA[i] = random.Next(10, 20);
                    P2ReadingA[i] = random.Next(10, 20);
                    P3ReadingA[i] = random.Next(10, 20);

                }
                chart1.Series[0].Points.DataBindXY(xValues, P1ReadingA);
                chart1.Series[1].Points.DataBindXY(xValues, P2ReadingA);
                chart1.Series[2].Points.DataBindXY(xValues, P3ReadingA);

                chart1.Series[0].Name = "P1";
                chart1.Series[1].Name = "P2";
                chart1.Series[2].Name = "P3";
                isGroupA = !isGroupA;
            }
            else
            {
                for (int i = 0; i < 60; i++)
                {
                    P1ReadingB[i] = random.Next(5, 10);
                    P2ReadingB[i] = random.Next(7, 15);
                    P3ReadingB[i] = random.Next(3, 6);
                }
                chart1.Series[0].Points.DataBindXY(xValues, P1ReadingB);
                chart1.Series[1].Points.DataBindXY(xValues, P2ReadingB);
                chart1.Series[2].Points.DataBindXY(xValues, P3ReadingB);

                chart1.Series[0].Name = "P1";
                chart1.Series[1].Name = "P2";
                chart1.Series[2].Name = "P3";
                isGroupA = !isGroupA;
            }
        }
        public void ReadGPS1Data()
        {
            // Set the server IP address and port
            // direct readin from GPS1 receiver "169.254.1.0" port 5017
            //reading through RS232 to Ethernet converter "192.168.0.7"
            string serverIP = "192.168.0.7"; //192.168.0.7 
            int serverPort = 23;//direct reading port //23 //RS232 to Ethernet converter port

            try
            {
                // Create a TCP client
                TcpClient client = new TcpClient(serverIP, serverPort);
                // Console.WriteLine("Connected to server.");

                // Get the client stream for reading and writing
                NetworkStream stream = client.GetStream();
                /*
                // Send data to the server
                string message = "Hello, server!";
                byte[] data = Encoding.ASCII.GetBytes(message);
                stream.Write(data, 0, data.Length);
                */
                // Receive response from the server
                byte[] buffer = new byte[1024];
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                string response = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                //Console.WriteLine("Response from server: " + response);

                // Assuming NMEA data format: $GPGGA,123519,4807.038,N,01131.000,E,1,08,0.9,545.4,M,46.9,M,,*47
                string[] fields = response.Split(',');
                if (fields.Length >= 7 && fields[0] == "$GPGGA")
                {
                    string utcTime = fields[1];
                    string latitude = fields[2];
                    string latDirection = fields[3];//N or S for North and South
                    string longitude = fields[4];
                    string lonDirection = fields[5]; //E or W for west and East
                    string fixQuality = fields[6]; //DGPS if 1 or 2, RTK if 4 or 5, GPS fail if others
                }

                // Close the client connection
                client.Close();
               // Console.WriteLine("Disconnected from server.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
               // Console.WriteLine("An error occurred: " + ex.Message);
            }
        }
        public void ReadGPS2Data()
        {
            // Set the server IP address and port
            // direct readin from GPS1 receiver "169.254.1.0"
            //reading through RS232 to Ethernet converter "192.168.0.7"
            string serverIP = "169.254.1.0"; //192.168.0.7 
            int serverPort = 5017;//direct reading port //23 //RS232 to Ethernet converter port

            try
            {
                // Create a TCP client
                TcpClient client = new TcpClient(serverIP, serverPort);
                // Console.WriteLine("Connected to server.");

                // Get the client stream for reading and writing
                NetworkStream stream = client.GetStream();
                /*
                // Send data to the server
                string message = "Hello, server!";
                byte[] data = Encoding.ASCII.GetBytes(message);
                stream.Write(data, 0, data.Length);
                */
                // Receive response from the server
                byte[] buffer = new byte[1024];
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                string response = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                // Console.WriteLine("Response from server: " + response);

                // Assuming NMEA data format: $GPGGA,123519,4807.038,N,01131.000,E,1,08,0.9,545.4,M,46.9,M,,*47
                string[] fields = response.Split(',');
                if (fields.Length >= 7 && fields[0] == "$GPGGA")
                {
                    string utcTime = fields[1];
                    string latitude = fields[2];
                    string latDirection = fields[3];//N or S for North and South
                    string longitude = fields[4];
                    string lonDirection = fields[5]; //E or W for west and East
                    string fixQuality = fields[6]; //DGPS if 1 or 2, RTK if 4 or 5, GPS fail if others
                }

                // Close the client connection
                client.Close();
               // Console.WriteLine("Disconnected from server.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
                //Console.WriteLine("An error occurred: " + ex.Message);
            }
        }
       
        private double CalculateBearing(double lat1, double lon1, double lat2, double lon2)
        {
            const double toRadians = Math.PI / 180.0;
            const double toDegrees = 180.0 / Math.PI;

            double dLon = (lon2 - lon1) * toRadians;

            double y = Math.Sin(dLon) * Math.Cos(lat2 * toRadians);
            double x = Math.Cos(lat1 * toRadians) * Math.Sin(lat2 * toRadians) - Math.Sin(lat1 * toRadians) * Math.Cos(lat2 * toRadians) * Math.Cos(dLon);

            double initialBearing = Math.Atan2(y, x) * toDegrees;

            // Convert the initial bearing to the range [0, 360)
            initialBearing = (initialBearing + 360) % 360;

            return initialBearing;
        }
        public void ReadSensorsData()
        {
            try
            {
                //-------------- Create and connect the client
                var client = new S7Client();
                int result = client.ConnectTo("192.168.0.1", 0, 1); //for S7-1200/1500 rack and slot should be rack 0 and slot 1.
                if (result == 0)
                {
                    // Console.WriteLine("Connected to 192.168.0.1");
                }
                else
                {
                    //  Console.WriteLine(client.ErrorText(result));
                    //Console.ReadKey();
                    return;
                }

                //-------------- Read db1
                // Console.WriteLine("\n---- Read DB 1");

                byte[] readBuffer = new byte[36];
                result = client.DBRead(1, 0, readBuffer.Length, readBuffer);
                if (result == 0)
                {
                    //  Console.WriteLine("Reading DB1 is OK");
                }
                else
                {
                    //   Console.WriteLine("Error in Reading DB1");
                    //Console.ReadKey();
                    return;
                }
                /*
                //Reading for Siemens PLC S7-1200 DB1 data base 
                bool db1dbx00 = S7.GetBitAt(readBuffer, 0, 0); //reading bit value at byte 0, bit 0
                bool db1dbx01 = S7.GetBitAt(readBuffer, 0, 1);
                int db1dbw2 = S7.GetIntAt(readBuffer, 2);
                double db1dbd4 = S7.GetRealAt(readBuffer, 4);
                int db1dbd8 = S7.GetDIntAt(readBuffer, 8);
                uint db1dbd12 = S7.GetDWordAt(readBuffer, 12);
                ushort db1dbd16 = S7.GetWordAt(readBuffer, 16);
                */
                double autolet = S7.GetRealAt(readBuffer, 0);
                double pressureSensor1 = S7.GetRealAt(readBuffer, 4);
                double pressureSensor2 = S7.GetRealAt(readBuffer, 8);
                double pressureSensor3 = S7.GetRealAt(readBuffer, 12);
                double strokeSensor1 = S7.GetDIntAt(readBuffer, 16);
                double strokeSensor2 = S7.GetDIntAt(readBuffer, 20);
                double levelSensor = S7.GetRealAt(readBuffer, 24);
                double inclinometer1 = S7.GetRealAt(readBuffer, 28);
                double inclinometer2 = S7.GetRealAt(readBuffer, 32);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
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
        private void GetCurrentWorkingAreaName()
        {
            if (gridLineDataList.Count > 0)
            { 
                //bool isInside;
                for (int i = 0; i < gridLineDataList.Count; i++)
                {
                    List<PointLatLng> pointsList = new List<PointLatLng>
                            {
                                gridLineDataList[i].gridPoint1,
                                gridLineDataList[i].gridPoint2,
                                gridLineDataList[i].gridPoint3,
                                gridLineDataList[i].gridPoint4,
                            };
                    //isInside = IsPointInsidePolygon(currentGPSPoint, pointsList);
                    if (IsPointInsidePolygon(currentGPSPoint, pointsList))
                    {
                        workingAreaName = gridLineDataList[i].area;
                        break;
                    }
                }
            }

        }
        static bool IsPointInsidePolygon(PointLatLng currentGPSPoint, List<PointLatLng> polygon)
        {
            int vertexCount = polygon.Count;
            bool isInside = false;

            for (int i = 0, j = vertexCount - 1; i < vertexCount; j = i++)
            {
                if ((polygon[i].Lng > currentGPSPoint.Lng) != (polygon[j].Lng > currentGPSPoint.Lng) &&
                    (currentGPSPoint.Lat < (polygon[j].Lat - polygon[i].Lat) * (currentGPSPoint.Lng - polygon[i].Lng) / (polygon[j].Lng - polygon[i].Lng) + polygon[i].Lat))
                {
                    isInside = !isInside;
                }
            }

            return isInside;
        }

        private void btnGetCurrentWorkingArea_Click(object sender, EventArgs e)
        {
            GetCurrentWorkingAreaName();
            lblWorkingArea.Text = workingAreaName;
        }
    }
    
}
