using GMap.NET.WindowsForms;
using GMap.NET;
using GMap.NET.WindowsForms.Markers;
using GMap.NET.MapProviders;
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
using Sharp7;
using static UTMLatLngConversionLibrary.LatLngUTMConvert;
using System.Net.Sockets;
using System.IO;
using Microsoft.VisualBasic.FileIO;
using System.Drawing.Drawing2D;
using System.Windows.Forms.DataVisualization.Charting;
using System.Threading;
//using System.Windows.Controls;
//using System.Windows.Controls;
//using System.Windows;

namespace Monitoring_System
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
        List<PointLatLng> totalPolygonPoints = new List<PointLatLng>();
        List<PointLatLng> objectLatLngList = new List<PointLatLng>();
        //List<ObjectLatLng> objectDataList = new List<ObjectLatLng>();
        List<GridLineData> gridLineDataList = new List<GridLineData>();
        GridLineData currentWorkingGridLineData = new GridLineData();
        PointLatLng currentGPSPoint;

        double currentGPS1Latitude, currentGPS1Longitude;
        double currentGPS2Latitude, currentGPS2Longitude;
        string workingAreaName = "";
        bool boolUpdateGPS1GUI =false, boolUpdateGPS2GUI = false, boolUpdateSensorGUI = false;
        public bool boolIsGPS1Up = false, boolIsGPS2Up = false, boolIsSensorUp = false;
        string GPS1Mode = "", GPS2Mode = "";

        //for plotting live chart
        private Random random = new Random();
        private List<double> p1 = new List<double>();
        private List<double> p2 = new List<double>();
        private List<double> p3 = new List<double>();
        private int movingAverageWindowSize = 10;
        double dangerousPValue = 8;
        System.Windows.Forms.Timer t_1SecPlotChart = new System.Windows.Forms.Timer();
        System.Windows.Forms.Timer t_1Sec_GPS1GUI = new System.Windows.Forms.Timer();
        System.Windows.Forms.Timer t_1Sec_GPS2GUI = new System.Windows.Forms.Timer();
        System.Windows.Forms.Timer t_1Sec_SensorGUI = new System.Windows.Forms.Timer();

        System.Windows.Forms.Timer t_1Sec = new System.Windows.Forms.Timer();
        System.Windows.Forms.Timer t_5Sec = new System.Windows.Forms.Timer();
        System.Windows.Forms.Timer t_1Min = new System.Windows.Forms.Timer();

        private List<PointLatLng> _points;
        private GMapOverlay pinMarkerOverlay;
        private bool boolDropPin = false;

        PointLatLng GPS1CurrentLocation, GPS2CurrentLocation;
        double currentBearing;
        double GPS2zHeight;

        GMapOverlay vehicleOverlay; // Overlay for the vehicle marker
        GMapOverlay compassOverlay; // Overlay for the compass marker
        GMapMarkerImage vehicleMarker;
        GMapMarkerImage compassMarker;
        int compassOffsetX, compassOffsetY;
        int compassWidth, compassHeight;

        private Image originalImage;
        private Image polygonSideImage;
        private Bitmap polygonScaledImage;
        /*
        //for plotting chart
        int[] xValues = new int[60];
        double[] dangerousValue = new double[60];
        double[] P1ReadingA = new double[60];
        double[] P2ReadingA = new double[60];
        double[] P3ReadingA = new double[60];
        double[] P1ReadingB = new double[60];
        double[] P2ReadingB = new double[60];
        double[] P3ReadingB = new double[60];
        bool isGroupA = true;
        int readingCount = 0;
        */
        bool isAutolet = true;
        bool OnlyNumberInTxtBearing = false;

        double autolet;
        double levelSensor;
        double inclinometer1, inclinometer2;
        double p1SensorReading;
        double p2SensorReading;
        double p3SensorReading;
        int strokeSensor1, strokeSensor2;

        double utmSPUD_X, utmSPUD_Y;
        double utmDMX, utmDMY, utmDMZ;
        double utmGPS1X, utmGPS1Y, utmGPS1Z;
        double utmGPS2X, utmGPS2Y, utmGPS2Z;
        int GPS1FixQuality, GPS2FixQuality;

        string bargeName = "AMOS16";
        string startArea = "";//"C6";
        string completeArea = "D7";
        string startDMX = "1111111";
        string startDMY = "2222222";
        string completeDMX = "3333333";
        string completeDMY = "4444444";
        string finalElevation = "1.5";
        string startTime = "1030";

        string completeTime = "1630";

        double setting1R = 5.538;
        double setting1H = 7.5;
        double setting1J = 2.65;
        double setting1F = 3.395;
        double setting1N = 2.497;
        double setting1M = 1.73;
        double setting2T = 1.5;   //no information yet
        double setting2U = 2;   // no information yet

        private UserControl1 userControl1;
        private UserControl2 userControl2;
        private UserControl3 userControl3;
        public Form1()
        {
            InitializeComponent();
            InitializeGMap();
            LoadImageIntoPictureBox();
            //InitXData();
            InitializeChart();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            
            /*
            //timer for 1 sec to plot chart
            t_1SecPlotChart.Interval = 1000;    //in millisecond
            t_1SecPlotChart.Tick += new EventHandler(this.timer1_TickPlotChart);
            t_1SecPlotChart.Start();
            */
            
            //timer for 1 sec
            t_1Sec.Interval = 1000;    //in millisecond
            t_1Sec.Tick += new EventHandler(this.t_Tick_1Sec);
            t_1Sec.Start();
            
            /*
            string ipAddress1 = "192.168.0.2"; // GPS1 Reading
            int port1 = 5017; // GPS1 port through RS232 to ethernet converter
            bool isAvailable1 = IsIpAddressAndPortAvailable(ipAddress1, port1);

            string ipAddress2 = "192.168.0.7"; // GPS2 Reading direct from receiver through switch. 
            int port2 = 23; // GPS2 port through 
            bool isAvailable2 = IsIpAddressAndPortAvailable(ipAddress2, port2);

            if (isAvailable1 && isAvailable2)
            {
                //timer for 5 seconds
                t_5Sec.Interval = 5000;    //in millisecond
                t_5Sec.Tick += new EventHandler(this.t_Tick_5Sec);
                t_5Sec.Start();

                //timer for 1 minute
                t_1Min.Interval = 60000;    //in millisecond
                t_1Min.Tick += new EventHandler(this.t_Tick_1Min);
                //t_1Min.Start();
                //Console.WriteLine($"The IP address {ipAddress} and port {port} is available.");
            }
            else
            {
                MessageBox.Show(@"The IP address {ipAddress} and port {port} is not available.");
                //Console.WriteLine($"The IP address {ipAddress} and port {port} is not available.");
            }
            */
            
            //timer for 5 seconds
            t_5Sec.Interval = 5000;    //in millisecond
            t_5Sec.Tick += new EventHandler(this.t_Tick_5Sec);
            t_5Sec.Start();
            /*
            //timer for 1 minute
            t_1Min.Interval = 60000;    //in millisecond
            t_1Min.Tick += new EventHandler(this.t_Tick_1Min);
            //t_1Min.Start();
            */

            verticalBar1.targetValue = 75;
            verticalBar1.tolerance = 10;
            btnMonitoring.BackColor = Color.Green;

            // Create an instance of the user control and add it to the main form
            userControl1 = new UserControl1();
            userControl1.Dock = DockStyle.Fill;
            userControl1.SetDimension_Click += UserControl1_SetDimensionClicked;
            userControl1.SetDraft_Click += UserControl1_SetDraftClicked;
            Controls.Add(userControl1);
            //userControl1.BringToFront();
            
            userControl2 = new UserControl2();
            userControl2.Dock = DockStyle.Fill;
            userControl2.SetDimension_Click += UserControl2_SetDimensionClicked;
            Controls.Add(userControl2);
           // userControl2.BringToFront();

            userControl3 = new UserControl3();
            userControl3.Dock = DockStyle.Fill;
            userControl3.SetGauge_Click += UserControl3_SetGaugeClicked;
            Controls.Add(userControl3);
           // userControl3.BringToFront();
            
            userControl1.Hide();
            userControl2.Hide();
            userControl3.Hide();

            BringToFrontAndActivate();
        }
        private void BringToFrontAndActivate()
        {
            this.WindowState = FormWindowState.Minimized;  // Minimize first
            this.Show();
            this.WindowState = FormWindowState.Normal;     // Then restore

            this.TopMost = true;  // Bring form to top
            this.Activate();      // Activate the form
            this.TopMost = false; // Optional: Set TopMost back to false if you don't want it to stay on top
        }
        private void StartGPS1Timer()
        {
            if (boolIsGPS1Up && !t_1Sec_GPS1GUI.Enabled)
            {
                //timer for 1 sec
                t_1Sec_GPS1GUI.Interval = 1000;    //in millisecond
                t_1Sec_GPS1GUI.Tick += new EventHandler(this.t_Tick_1Sec_GPS1GUI);
                t_1Sec_GPS1GUI.Start();
            }
            else if(!boolIsGPS1Up && t_1Sec_GPS1GUI.Enabled)
            {
                t_1Sec_GPS1GUI.Stop();
            }
        }
        private void StartGPS2Timer()
        {
            if (boolIsGPS2Up && !t_1Sec_GPS2GUI.Enabled)
            {
                //timer for 1 sec
                t_1Sec_GPS2GUI.Interval = 1000;    //in millisecond
                t_1Sec_GPS2GUI.Tick += new EventHandler(this.t_Tick_1Sec_GPS2GUI);
                t_1Sec_GPS2GUI.Start();
            }
            else if (!boolIsGPS2Up && t_1Sec_GPS2GUI.Enabled)
            {
                t_1Sec_GPS2GUI.Stop();
            }
        }
        private void StartSensorTimer()
        {
            if (boolIsSensorUp && !t_1Sec_SensorGUI.Enabled)
            {
                //timer for 1 sec
                t_1Sec_SensorGUI.Interval = 1000;    //in millisecond
                t_1Sec_SensorGUI.Tick += new EventHandler(this.t_Tick_1Sec_SensorGUI);
                t_1Sec_SensorGUI.Start();
            }
            else if (!boolIsSensorUp && t_1Sec_SensorGUI.Enabled)
            {
                t_1Sec_SensorGUI.Stop();
            }
            
        }
        private void StartChartPlottingTimer()
        {
            if (boolIsSensorUp && !t_1Sec_SensorGUI.Enabled)
            {
                //timer for 1 sec to plot chart
                t_1SecPlotChart.Interval = 1000;    //in millisecond
                t_1SecPlotChart.Tick += new EventHandler(this.timer1_TickPlotChart);
                t_1SecPlotChart.Start();
            }
            else if (!boolIsSensorUp && t_1Sec_SensorGUI.Enabled)
            {
                t_1SecPlotChart.Stop();
            }

            
        }
        private void t_Tick_1Sec_GPS1GUI(object sender, EventArgs e)
        {
            //1 second timer for GPS1 GUI
            ReadGPS1();
            UpdateGPS1GUI();
        }
        private void t_Tick_1Sec_GPS2GUI(object sender, EventArgs e)
        {
            //1 second timer for GPS2 GUI
            ReadGPS2();
            UpdateGPS2GUI();
        }
        private void t_Tick_1Sec_SensorGUI(object sender, EventArgs e)
        {
            //1 second timer for Sensor GUI
            ReadSensor();
            UpdateSensorDataGUI();
        }
        private void t_Tick_1Sec(object sender, EventArgs e)
        {
            //1 second timer
            ShowDateTime();

        }

        private void t_Tick_5Sec(object sender, EventArgs e)
        {
            StartGPS1Timer();
            StartGPS2Timer();
            StartSensorTimer();
            StartChartPlottingTimer();
            GetCurrentWorkingAreaName(currentGPSPoint);
            UpdateUserControl3GUI();
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
            t_1SecPlotChart.Stop();
        }
        private void UserControl1_SetDraftClicked(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void UserControl1_SetDimensionClicked(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void UserControl2_SetDimensionClicked(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        //event handler for set gauge event in userControl3 (setting 3)
        private void UserControl3_SetGaugeClicked(object sender, EventArgs e)
        {
           // MessageBox.Show("Setting 3 Event show");

        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            // Specify the line's properties
            int lineY = (pictureBox1.Height) / 2; // Y-coordinate for the horizontal line
            int lineWidth = pictureBox1.Width; // Width of the line
            Pen linePen = new Pen(Color.Blue, 4); // Pen for drawing the line

            // Draw the horizontal line on the PictureBox
            e.Graphics.DrawLine(linePen, 0, lineY, lineWidth, lineY);

            int startX = (pictureBox1.Width - pictureBox1.Image.Width) / 2;
            // Define points for the curve
            PointF start = new PointF(startX, lineY - 40);
            PointF control = new PointF(100, lineY);
            PointF end = new PointF(startX, lineY + 40);
            // Draw the Bezier curve
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.DrawBezier(Pens.Black, start, control, control, end);
            // Draw arrowheads
            DrawArrowhead(e.Graphics, start, control, false);
            DrawArrowhead(e.Graphics, end, control, false);

            // Dispose of the Pen to release resources
            linePen.Dispose();
        }
        private void DrawArrowhead(Graphics g, PointF location, PointF directionFrom, bool isStart)
        {
            float arrowAngle = (float)Math.Atan2(location.Y - directionFrom.Y, location.X - directionFrom.X);
            float arrowLength = 10;
            // float arrowWidth = 5;

            PointF[] arrowPoints = new PointF[3];

            if (isStart)
            {
                arrowPoints[0] = location;
                arrowPoints[1] = new PointF(location.X + arrowLength * (float)Math.Cos(arrowAngle + Math.PI / 6),
                                            location.Y + arrowLength * (float)Math.Sin(arrowAngle + Math.PI / 6));
                arrowPoints[2] = new PointF(location.X + arrowLength * (float)Math.Cos(arrowAngle - Math.PI / 6),
                                            location.Y + arrowLength * (float)Math.Sin(arrowAngle - Math.PI / 6));
            }
            else
            {
                arrowPoints[0] = location;
                arrowPoints[1] = new PointF(location.X - arrowLength * (float)Math.Cos(arrowAngle - Math.PI / 6),
                                            location.Y - arrowLength * (float)Math.Sin(arrowAngle - Math.PI / 6));
                arrowPoints[2] = new PointF(location.X - arrowLength * (float)Math.Cos(arrowAngle + Math.PI / 6),
                                            location.Y - arrowLength * (float)Math.Sin(arrowAngle + Math.PI / 6));
            }

            g.FillPolygon(Brushes.Black, arrowPoints);
        }
        /*
        public void InitXData()
        {
            for (int i = 0; i < 60; i++)
            {
                xValues[i] = (i - 59);
                dangerousValue[i] = 8;
            }
        }
        */
        public bool IsGPS1Up()
        {
            return IsTCPServerUp("192.168.0.7", 23, 1000); //GPS1 TCP server and port
        }
        public bool IsGPS2Up()
        {
            return IsTCPServerUp("192.168.0.2", 5017, 1000); //GPS2 TCP server and port
        }
        /*
        public static bool IsPLCUp()
        {
            return IsTCPServerUp("192.168.0.7", 23, 1000); //PLC TCP server and port
        }
        */
        static bool IsIpAddressAndPortAvailable(string ipAddress, int port)
        {
            using (TcpClient tcpClient = new TcpClient())
            {
                try
                {
                    // Attempt to connect to the IP address and port

                    tcpClient.Connect(ipAddress, port);
                    return true;
                }
                catch (SocketException)
                {
                    return false;
                }
            }
        }
        public static bool IsTCPServerUp(string server, int port, int timeout)
        {
            bool isUp;

            try
            {
                using (TcpClient tcp = new TcpClient())
                {
                    IAsyncResult ar = tcp.BeginConnect(server, port, null, null);
                    WaitHandle wh = ar.AsyncWaitHandle;

                    try
                    {
                        if (!wh.WaitOne(TimeSpan.FromMilliseconds(timeout), false))
                        {
                            tcp.EndConnect(ar);
                            tcp.Close();
                            throw new SocketException();
                        }

                        isUp = true;
                        tcp.EndConnect(ar);
                    }
                    finally
                    {
                        wh.Close();
                    }
                }
            }
            catch (SocketException e)
            {
                MessageBox.Show(string.Format("TCP connection to server {0} failed.", server));
                isUp = false;
            }

            return isUp;
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
        private void InitializeChart()
        {
            chart1.Series.Clear();
            chart1.Series.Add("P1");
            chart1.Series.Add("P2");
            chart1.Series.Add("P3");
            chart1.Series.Add("D Value");

            chart1.Series["P1"].ChartType = SeriesChartType.Line;
            chart1.Series["P2"].ChartType = SeriesChartType.Line;
            chart1.Series["P3"].ChartType = SeriesChartType.Line;
            chart1.Series["D Value"].ChartType = SeriesChartType.Line;
            chart1.Series["D Value"].BorderWidth = 3;
            chart1.Series["D Value"].Color = Color.Red;

            // Set Title and axis labels
            chart1.ChartAreas[0].AxisX.Title = "Seconds";
            chart1.ChartAreas[0].AxisY.Title = "Pressure(bar)";
        }

        private void timer1_TickPlotChart(object sender, EventArgs e)
        {
            /*
            double newDataPoint = random.NextDouble() * 10; // Generate random data
            p1.Add(newDataPoint);
            double newDataPoint2 = random.NextDouble() * 9.5; // Generate random data
            p2.Add(newDataPoint2);
            double newDataPoint3 = random.NextDouble() * 9; // Generate random data
            p3.Add(newDataPoint3);

            userControl3.P1 = Convert.ToString(newDataPoint);
            userControl3.P2 = Convert.ToString(newDataPoint2);
            userControl3.P3 = Convert.ToString(newDataPoint3);
            */
            
            p1.Add(p1SensorReading);
            p2.Add(p2SensorReading);
            p3.Add(p3SensorReading);
            
            if (p1.Count > movingAverageWindowSize)
                p1.RemoveAt(0);
            if (p2.Count > movingAverageWindowSize)
                p2.RemoveAt(0);
            if (p3.Count > movingAverageWindowSize)
                p3.RemoveAt(0);

            double movingAverageP1 = CalculateMovingAverage(p1);
            double movingAverageP2 = CalculateMovingAverage(p2);
            double movingAverageP3 = CalculateMovingAverage(p3);

            chart1.Series["P1"].Points.AddY(movingAverageP1);
            chart1.Series["P2"].Points.AddY(movingAverageP2);
            chart1.Series["P3"].Points.AddY(movingAverageP3);
            chart1.Series["D Value"].Points.AddY(dangerousPValue);

            if (chart1.Series["P1"].Points.Count > 60) // Keep a reasonable number of points on the chart
            {
                chart1.Series["P1"].Points.RemoveAt(0);
                chart1.Series["P2"].Points.RemoveAt(0);
                chart1.Series["P3"].Points.RemoveAt(0);
                chart1.Series["D Value"].Points.RemoveAt(0);
            }
        }

        private double CalculateMovingAverage(List<double> values)
        {
            double sum = 0;
            foreach (double value in values)
            {
                sum += value;
            }
            return sum / values.Count;
        }
        /*
        private void PlotChart()
        {
            if (isGroupA)
            {
                /*
                for (int i = 0; i < 60; i++)
                {
                    P1ReadingA[i] = random.Next(10, 20);
                    P2ReadingA[i] = random.Next(10, 20);
                    P3ReadingA[i] = random.Next(10, 20);
                }
                */
        /*
                chart1.Series[0].Points.DataBindXY(xValues, P1ReadingA);
                chart1.Series[1].Points.DataBindXY(xValues, P2ReadingA);
                chart1.Series[2].Points.DataBindXY(xValues, P3ReadingA);
                chart1.Series[3].Points.DataBindXY(xValues, dangerousValue);

                chart1.Series[0].Name = "P1";
                chart1.Series[1].Name = "P2";
                chart1.Series[2].Name = "P3";
                chart1.Series[3].Name = "D Value";

                // Set axis labels
                chart1.ChartAreas[0].AxisX.Title = "X-Axis";
                chart1.ChartAreas[0].AxisY.Title = "Y-Axis";

                isGroupA = !isGroupA;
            }
            else
            {
                /*
                for (int i = 0; i < 60; i++)
                {
                    P1ReadingB[i] = random.Next(5, 10);
                    P2ReadingB[i] = random.Next(7, 15);
                    P3ReadingB[i] = random.Next(3, 6);
                }
                */
        /*
                chart1.Series[0].Points.DataBindXY(xValues, P1ReadingB);
                chart1.Series[1].Points.DataBindXY(xValues, P2ReadingB);
                chart1.Series[2].Points.DataBindXY(xValues, P3ReadingB);
                chart1.Series[3].Points.DataBindXY(xValues, dangerousValue);

                chart1.Series[0].Name = "P1";
                chart1.Series[1].Name = "P2";
                chart1.Series[2].Name = "P3";
                chart1.Series[3].Name = "D Value";

                // Set axis labels
                chart1.ChartAreas[0].AxisX.Title = "X-Axis";
                chart1.ChartAreas[0].AxisY.Title = "Y-Axis";

                isGroupA = !isGroupA;
            }
        }
        */
        private async void ReadGPS1()
        {
            //ReadGPS1Data();
            // Run the time-consuming operation on a background thread
            bool success = await Task.Run(() => ReadGPS1Data());

            if (success)
            {
                lblStatus.Text = "Connected to GPS1 successfully!";
                boolIsGPS1Up = true;
            }
            else
            {
                lblStatus.Text = "Connection to GPS1 failed!";
                boolIsGPS1Up = false;
            }
        }
        private async void ReadGPS2()
        {
            //ReadGPS2Data();
            // Run the time-consuming operation on a background thread
            bool success = await Task.Run(() => ReadGPS2Data());

            if (success)
            {
                lblStatus.Text = "Connected to GPS2 successfully!";
                boolIsGPS2Up = true;
            }
            else
            {
                lblStatus.Text = "Connection to GPS2 failed!";
                boolIsGPS2Up = false;
            }
        }
        private async void ReadSensor()
        {
            //ReadSensorsData();
            // Run the time-consuming operation on a background thread
            bool success = await Task.Run(() => ReadSensorsData());

            if (success)
            {
                lblStatus.Text = "Reading of Sensor Data successful!";
                boolIsSensorUp = true;
            }
            else
            {
                lblStatus.Text = "Reading of Sensor Data failed!";
                boolIsSensorUp = false;
            }
        }

        public bool ReadGPS1Data()
        {
            // Set the server IP address and port
            // direct readin from GPS1 receiver "169.254.1.0" port 5017

            string serverIP = "192.168.0.2"; //192.168.0.2 
            int serverPort = 5017;//direct reading from GPS receiver port //

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
                if (fields.Length >= 7 && fields[0] == "$GNGGA") //$GNGGA, $GPGGA
                {
                    string utcTime = fields[1];
                    double latitude = Convert.ToDouble(fields[2]);
                    string latDirection = fields[3];//N or S for North and South
                    double longitude = Convert.ToDouble(fields[4]);
                    string lonDirection = fields[5]; //E or W for west and East
                    GPS1FixQuality = Convert.ToInt32(fields[6]); //DGPS if 1 or 2, RTK if 4 or 5, GPS fail if others
                    double zHeight = Convert.ToDouble(fields[9]); // Z axis Height in Meters

                    // Convert latitude and longitude to decimal degrees format
                    currentGPS1Latitude = ConvertToDecimalDegrees(latitude, latDirection);
                    currentGPS1Longitude = ConvertToDecimalDegrees(longitude, lonDirection);

                    //convert to UTM coordinate
                    UTMResult utmResult = utmLatLngConvert.convertLatLngToUtm(latitude, longitude);

                    utmGPS1X = Math.Round(utmResult.Northing, 2);
                    utmGPS1Y = Math.Round(utmResult.Easting, 2);
                    utmGPS1Z = Math.Round(zHeight, 2);
                   // utmGPS1Z = zHeight;

                    
                }

                // Close the client connection
                client.Close();
                // Console.WriteLine("Disconnected from server.");
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
                // Console.WriteLine("An error occurred: " + ex.Message);
                boolIsGPS2Up = false;
                return false;
            }

        }
        public bool ReadGPS2Data()
        {
            // Set the server IP address and port
            // direct readin from GPS1 receiver "169.254.1.0"
            //reading through RS232 to Ethernet converter "192.168.0.7"
            string serverIP = "192.168.0.7"; //192.168.0.7 
            int serverPort = 23;//reading through converter //23 //RS232 to Ethernet converter port

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
                if (fields.Length >= 7 && fields[0] == "$GNGGA") //"$GPGGA"
                {
                    string utcTime = fields[1];
                    double latitude = Convert.ToDouble(fields[2]);
                    string latDirection = fields[3];//N or S for North and South
                    double longitude = Convert.ToDouble(fields[4]);
                    string lonDirection = fields[5]; //E or W for west and East
                    GPS2FixQuality = Convert.ToInt32(fields[6]); //DGPS if 1 or 2, RTK if 4 or 5, GPS fail if others
                    GPS2zHeight = Convert.ToDouble(fields[9]); // Z axis Height in Meters

                    // Convert latitude and longitude to decimal degrees format
                    currentGPS2Latitude = ConvertToDecimalDegrees(latitude, latDirection);
                    currentGPS1Longitude = ConvertToDecimalDegrees(longitude, lonDirection);
                    currentGPSPoint = new PointLatLng(currentGPS2Latitude, currentGPS1Longitude);
                    //convert to UTM coordinate
                    UTMResult utmResult = utmLatLngConvert.convertLatLngToUtm(latitude, longitude);
                    utmGPS2X = Math.Round(utmResult.Northing, 2);
                    utmGPS2Y = Math.Round(utmResult.Easting, 2);
                    utmGPS2Z = Math.Round(GPS2zHeight, 2);
                }
                // Close the client connection
                client.Close();
                // Console.WriteLine("Disconnected from server.");
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
                //Console.WriteLine("An error occurred: " + ex.Message);
                boolIsGPS2Up = false;
                return false;
            }

        }
        static double ConvertToDecimalDegrees(double value, string direction)
        {
            double degrees = Math.Floor(value / 100);
            double minutes = value - (degrees * 100);

            double decimalDegrees = degrees + (minutes / 60);

            if (direction == "S" || direction == "W")
            {
                decimalDegrees = -decimalDegrees;
            }

            return Math.Round(decimalDegrees, 6);
        }
        /*
         if(GPS1CurrentLocation.Lat != 0 && GPS2CurrentLocation.Lat != 0)
        {
            currentBearing = CalculateBearing(GPS1CurrentLocation, GPS2CurrentLocation);
        }
        */
        //private double CalculateBearing(double lat1, double lon1, double lat2, double lon2)
        private double CalculateBearing(PointLatLng GPS1Location, PointLatLng GPS2Location)
        {
            double lat1 = GPS1Location.Lat; //GPS1CurrentLocation
            double lon1 = GPS1Location.Lng; //GPS1CurrentLocation
            double lat2 = GPS2Location.Lat; //GPS2CurrentLocation
            double lon2 = GPS2Location.Lng; //GPS2CurrentLocation

            const double toRadians = Math.PI / 180.0;
            const double toDegrees = 180.0 / Math.PI;

            double dLon = (lon2 - lon1) * toRadians;

            double y = Math.Sin(dLon) * Math.Cos(lat2 * toRadians);
            double x = Math.Cos(lat1 * toRadians) * Math.Sin(lat2 * toRadians) - Math.Sin(lat1 * toRadians) * Math.Cos(lat2 * toRadians) * Math.Cos(dLon);

            double initialBearing = Math.Atan2(y, x) * toDegrees;

            // Convert the initial bearing to the range [0, 360)
            initialBearing = (initialBearing + 360) % 360;

            return Math.Round(initialBearing, 2);
        }
        public bool ReadSensorsData()
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
                    //return;
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
                    //return;
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
                autolet = S7.GetRealAt(readBuffer, 0); //0-25meters declare global
                p1SensorReading = S7.GetRealAt(readBuffer, 4); //0-10bars
                p2SensorReading = S7.GetRealAt(readBuffer, 8); //0-10bars
                p3SensorReading = S7.GetRealAt(readBuffer, 12); //0-10bars
                strokeSensor1 = S7.GetDIntAt(readBuffer, 16);//0-6000mm
                strokeSensor2 = S7.GetDIntAt(readBuffer, 20);//0-6000mm
                levelSensor = S7.GetRealAt(readBuffer, 24); //0-10meters declare global
                inclinometer1 = S7.GetRealAt(readBuffer, 28); //Y-Axis -2 to +2 degree Heel
                inclinometer2 = S7.GetRealAt(readBuffer, 32); //X-axis  -5 to +5 degree Trim

                // Disconnect the client connection
                client.Disconnect();

                autolet = Math.Round(autolet, 2);
                p1SensorReading = Math.Round(p1SensorReading, 2);
                p2SensorReading = Math.Round(p2SensorReading, 2);
                p3SensorReading = Math.Round(p3SensorReading, 2);
                levelSensor = Math.Round(levelSensor, 2);
                inclinometer1 = Math.Round(inclinometer1, 2);
                inclinometer2 = Math.Round(inclinometer2, 2);
                boolIsSensorUp = true;
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
                boolIsSensorUp = false;
                return false;
            }
        }
        private void UpdateGPS1GUI()
        {
            try
            {
                //update to display GPS1
                lblGPS1X.Text = "X: " + Convert.ToString(utmGPS1X);
                lblGPS1Y.Text = "Y: " + Convert.ToString(utmGPS1Y);
                lblGPS1Z.Text = "Z: " + Convert.ToString(utmGPS1Z);

                /*
                //update to display
                lblGPS1X.Text = "X: " + Convert.ToString(latitude);
                lblGPS1Y.Text = "Y: " + Convert.ToString(longitude);
                lblGPS1Z.Text = "Z: " + Convert.ToString(zHeight);
                */
                GPS1CurrentLocation = new PointLatLng(currentGPS1Latitude, currentGPS1Longitude);

                if (GPS1FixQuality == 1 || GPS1FixQuality == 2)
                {
                    lblGPS1RTK.BackColor = Color.Yellow;
                    lblGPS1RTK.Text = "DGPS";
                    GPS1Mode = "DGPS";
                }
                else if (GPS1FixQuality == 4 || GPS1FixQuality == 5)
                {
                    lblGPS1RTK.BackColor = Color.Green;
                    lblGPS1RTK.Text = "RTK";
                    GPS1Mode = "RTK";
                }
                else
                {
                    lblGPS1RTK.BackColor = Color.Red;
                    lblGPS1RTK.Text = "Fail";
                    GPS1Mode = "Fail";
                }
                //Invalidate();
                Refresh();
            }
            catch(Exception ex) 
            {
                MessageBox.Show(ex.Message, "Error");
            }

        }
        private void UpdateGPS2GUI()
        {
            try
            {
                //update to display
                lblGPS2X.Text = "X: " + Convert.ToString(utmGPS2X);
                lblGPS2Y.Text = "Y: " + Convert.ToString(utmGPS2Y);
                lblGPS2Z.Text = "Z: " + Convert.ToString(GPS2zHeight);

                /*
                //update to display
                lblGPS2X.Text = "X:" + Convert.ToString(latitude);
                lblGPS2Y.Text = "Y:" + Convert.ToString(longitude);
                lblGPS2Z.Text = "Z:" + Convert.ToString(zHeight);
                */
                GPS2CurrentLocation = new PointLatLng(currentGPS2Latitude, currentGPS2Longitude);

                if (GPS2FixQuality == 1 || GPS2FixQuality == 2)
                {
                    lblGPS2RTK.BackColor = Color.Yellow;
                    lblGPS2RTK.Text = "DGPS";
                    GPS2Mode = "DGPS";
                }
                else if (GPS2FixQuality == 4 || GPS2FixQuality == 5)
                {
                    lblGPS2RTK.BackColor = Color.Green;
                    lblGPS2RTK.Text = "RTK";
                    GPS2Mode = "RTK";
                }
                else
                {
                    lblGPS2RTK.BackColor = Color.Red;
                    lblGPS2RTK.Text = "Fail";
                    GPS2Mode = "Fail";
                }
                //Invalidate();
                Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }
        private void UpdateSensorDataGUI()
        {
            try
            {
                if (isAutolet)
                {
                    if (autolet >= 0 && autolet <= 25)
                    {
                        lblElevation.Text = Convert.ToString(autolet) + " [m]";
                        verticalBar1.Value = (int)(4 * autolet); // scaling since vertical bar has 0-100 and autolet has 0-25 meters
                    }

                }
                else
                {
                    if (levelSensor >= 0 && levelSensor <= 10)
                    {
                        lblElevation.Text = Convert.ToString(levelSensor) + " [m]";
                        verticalBar1.Value = (int)(10 * levelSensor); // scaling since vertical bar has 0-100 and autolet has 0-10 meters
                    }

                }
                if (p1SensorReading >= 0 && p1SensorReading <= 10)
                {
                    lblP1Gauge.Text = Convert.ToString(p1SensorReading) + " bar";
                    lblP2Gauge.Text = Convert.ToString(p2SensorReading) + " bar";
                    lblP3Gauge.Text = Convert.ToString(p3SensorReading) + " bar";
                }

                lblLadderStroke1.Text = Convert.ToString(strokeSensor1) + " [mm]"; //stroke sensor 1 0-6000mm
                lblLadderStroke2.Text = Convert.ToString(strokeSensor2) + " [mm]"; //stroke sensor 2

                lblTrim.Text = Convert.ToString(inclinometer1) + " deg";
                lblHeel.Text = Convert.ToString(inclinometer2) + " deg";

                if (p1SensorReading >= 0 && p1SensorReading <= 10)
                {
                    gaugeControl1.Value = (int)(10 * p1SensorReading); // scaling since gauge control has 0-100 and reading has 0-10 bars
                    lblP1Normal.BackColor = Color.LightGreen;
                    lblP1Normal.Text = "Normal";
                }
                else
                {
                    lblP1Normal.BackColor = Color.Red;
                    lblP1Normal.Text = "Fail";
                }

                if (p2SensorReading >= 0 && p2SensorReading <= 10)
                {
                    gaugeControl2.Value = (int)(10 * p2SensorReading); // scaling since gauge control has 0-100 and reading has 0-10 bars
                    lblP2Normal.BackColor = Color.LightGreen;
                    lblP2Normal.Text = "Normal";
                }
                else
                {
                    lblP2Normal.BackColor = Color.Red;
                    lblP2Normal.Text = "Fail";
                }

                if (p3SensorReading >= 0 && p3SensorReading <= 10)
                {
                    gaugeControl3.Value = (int)(10 * p3SensorReading); // scaling since gauge control has 0-100 and reading has 0-10 bars
                    lblP3Normal.BackColor = Color.LightGreen;
                    lblP3Normal.Text = "Normal";
                }
                else
                {
                    lblP3Normal.BackColor = Color.Red;
                    lblP3Normal.Text = "Fail";
                }

                if (strokeSensor1 >= 0 && strokeSensor1 <= 6000)
                {
                    verticalBarWithoutTolerance1.Value = (int)(strokeSensor1 / 60);// scaling since gauge control has 0-100 and ladder stroke has 0-6000 mm
                    lblSS1Normal.BackColor = Color.LightGreen;
                    lblSS1Normal.Text = "Normal";
                }
                else
                {
                    lblSS1Normal.BackColor = Color.Red;
                    lblSS1Normal.Text = "Fail";
                }

                if (strokeSensor2 >= 0 && strokeSensor2 <= 6000)
                {
                    verticalBarWithoutTolerance2.Value = (int)(strokeSensor2 / 60);// scaling since gauge control has 0-100 and ladder stroke has 0-6000 mm
                    lblSS2Normal.BackColor = Color.LightGreen;
                    lblSS2Normal.Text = "Normal";
                }
                else
                {
                    lblSS2Normal.BackColor = Color.Red;
                    lblSS2Normal.Text = "Fail";
                }
                /*
                if (isGroupA)
                {
                    P1ReadingA[readingCount] = p1SensorReading;
                    P2ReadingA[readingCount] = p2SensorReading;
                    P3ReadingA[readingCount++] = p3SensorReading;
                }
                else
                {
                    P1ReadingB[readingCount] = p1SensorReading;
                    P2ReadingB[readingCount] = p2SensorReading;
                    P3ReadingB[readingCount++] = p3SensorReading;
                }
                if (readingCount >= 59)
                {
                    //PlotChart();
                    readingCount = 0;
                }
                */
                // Rotate the image
                Image rotatedImage = RotateImage(polygonSideImage, (int)inclinometer1);
                // Display the rotated image in the PictureBox control
                pictureBox1.Image = rotatedImage;

                //Invalidate();
                Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }
        private void UpdateUserControl3GUI()
        {
            userControl3.Time = DateTime.Now.ToString();
            userControl3.GPS1xValue = Convert.ToString(utmGPS1X);
            userControl3.GPS1yValue = Convert.ToString(utmGPS1Y);
            userControl3.GPS1Mode = Convert.ToString(GPS1Mode);

            userControl3.GPS2xValue = Convert.ToString(utmGPS2X);
            userControl3.GPS2yValue = Convert.ToString(utmGPS2Y);
            userControl3.GPS2Mode = Convert.ToString(GPS2Mode);

            userControl3.Heading = txtBearing.Text;

            userControl3.WorkingArea = workingAreaName;

            userControl3.X1 = Convert.ToString(currentWorkingGridLineData.x1);
            userControl3.Y1 = Convert.ToString(currentWorkingGridLineData.y1);
            userControl3.X2 = Convert.ToString(currentWorkingGridLineData.x2);
            userControl3.Y2 = Convert.ToString(currentWorkingGridLineData.y2);
            userControl3.X3 = Convert.ToString(currentWorkingGridLineData.x3);
            userControl3.Y3 = Convert.ToString(currentWorkingGridLineData.y3);
            userControl3.X4 = Convert.ToString(currentWorkingGridLineData.x4);
            userControl3.Y4 = Convert.ToString(currentWorkingGridLineData.y4);

            userControl3.P1 = Convert.ToString(p1SensorReading);
            userControl3.P2 = Convert.ToString(p2SensorReading);
            userControl3.P3 = Convert.ToString(p3SensorReading);

            userControl3.SS1 = Convert.ToString(strokeSensor1);
            userControl3.SS2 = Convert.ToString(strokeSensor2);
            userControl3.Trim = Convert.ToString(inclinometer2);
            userControl3.Heel = Convert.ToString(inclinometer1);
            userControl3.TideGauge = Convert.ToString(2); //need to add when available now not included yet
            userControl3.LevelSensor = Convert.ToString(levelSensor);
            userControl3.Autolet = Convert.ToString(autolet);
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
        private void GetCurrentWorkingAreaName(PointLatLng currentGPSPoint)
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
                        currentWorkingGridLineData = gridLineDataList[i];
                        break;
                    }
                }
            }
            lblWorkingArea.Text = workingAreaName;
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

        private void SaveCSVFiles()
        {
            SaveFileDialog sfd = new SaveFileDialog()
            {
                Title = "Save CSV File",
                Filter = "csv files(*.csv)|*.csv",
                FileName = "data.csv",
                //CheckFileExists = true,
                //CheckPathExists = true,
                ValidateNames = true
            };

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                string csvFilePath = sfd.FileName;
                // CSV file heading
                string heading = "Barge_Name,Start_Area,Complete_Area,Start_DMX,Start_DMY,Complete_DMX,Complete_DMY,Final_Elevation,Start_Time,Complete_Time";

                // Data to be written to the CSV file
                string dataToSave = $"{bargeName},{startArea},{completeArea},{startDMX},{startDMY},{completeDMX},{completeDMY},{finalElevation},{startTime},{completeTime}";

                try
                {
                    // Check if the heading exists, if not, write the heading
                    if (!DoesHeadingExist(csvFilePath))
                    {
                        using (StreamWriter writer = new StreamWriter(csvFilePath, false))
                        {
                            writer.WriteLine(heading);
                        }
                    }

                    // Append data to the CSV file
                    using (StreamWriter writer = new StreamWriter(csvFilePath, true))
                    {
                        writer.WriteLine(dataToSave);
                    }

                    MessageBox.Show(@"Data saved to CSV file.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error");
                }
            }
        }
        private bool DoesHeadingExist(string csvFilePath)
        {
            if (File.Exists(csvFilePath))
            {
                using (StreamReader reader = new StreamReader(csvFilePath))
                {
                    string firstLine = reader.ReadLine();
                    if (firstLine != null)
                    {
                        string[] headings = firstLine.Split(',');
                        if (headings.Length >= 10 && headings[0] == "Barge_Name" && headings[1] == "Start_Area" && headings[2] == "Complete_Area"
                            && headings[3] == "Start_DMX" && headings[4] == "Start_DMY" && headings[5] == "Complete_DMX"
                            && headings[6] == "Complete_DMY" && headings[7] == "Final_Elevation" && headings[8] == "Start_Time"
                            && headings[9] == "Complete_Time")
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private void SPUDCalculation()
        {
            utmSPUD_X = utmGPS1X - setting1R;   //GPS1X - R
            utmSPUD_Y = utmGPS1Y - (setting1H - setting1J + setting1F); //GPS1Y - (H-J+F)
        }

        private void DischargeMouthCalculation()
        {
            utmDMX = utmGPS2Y + setting1N;    //GPS2X + N
            utmDMY = utmGPS2Y + setting1M;   //GPS2Y + M
            utmDMZ = utmGPS2Z - (setting2T + setting2U);   //GPS2Z - (T+U) 
        }

        private void InitializeGMap()
        {
            gmap.Bearing = 0;
            gmap.CanDragMap = true;
            gmap.DragButton = MouseButtons.Left;
            gmap.GrayScaleMode = true;
            gmap.MarkersEnabled = true;
            gmap.MaxZoom = 22;
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
                //convert to UTM coordinate
                UTMResult utmResult = utmLatLngConvert.convertLatLngToUtm(point.Lat, point.Lng);
                double utmX = Math.Round(utmResult.Northing, 2);
                double utmY = Math.Round(utmResult.Easting, 2);

                // Drop a new marker at the clicked location
                GMarkerGoogle newMarker = new GMarkerGoogle(point, GMarkerGoogleType.red);
                newMarker.ToolTipText = $"X: {utmX} \nY: {utmY}";
                newMarker.ToolTipMode = MarkerTooltipMode.OnMouseOver;
                //newMarker.ToolTipMode = MarkerTooltipMode.Always;
                pinMarkerOverlay.Markers.Add(newMarker);

                // Refresh the GMap control to show the newly added marker
                gmap.Refresh();
                btnClear.Enabled = true;
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
        private void btnMonitoring_Click(object sender, EventArgs e)
        {
            
            userControl1.Hide();
            userControl2.Hide();
            userControl3.Hide();
            
            btnMonitoring.BackColor = Color.Green;
            btnSetting1.BackColor = Color.LightSkyBlue;
            btnSetting2.BackColor = Color.LightSkyBlue;
            btnSetting3.BackColor = Color.LightSkyBlue;
        }

        private void btnSetting1_Click(object sender, EventArgs e)
        {
            userControl1.Show();
            userControl1.BringToFront();

            btnMonitoring.BackColor = Color.LightSkyBlue;
            btnSetting1.BackColor = Color.Green;
            btnSetting2.BackColor = Color.LightSkyBlue;
            btnSetting3.BackColor = Color.LightSkyBlue;
        }

        private void btnSetting2_Click(object sender, EventArgs e)
        {
            userControl2.Show();
            userControl2.BringToFront();

            btnMonitoring.BackColor = Color.LightSkyBlue;
            btnSetting1.BackColor = Color.LightSkyBlue;
            btnSetting2.BackColor = Color.Green;
            btnSetting3.BackColor = Color.LightSkyBlue;
        }
        private void btnSetting3_Click(object sender, EventArgs e)
        {
            userControl3.Show();
            userControl3.BringToFront();

            btnMonitoring.BackColor = Color.LightSkyBlue;
            btnSetting1.BackColor = Color.LightSkyBlue;
            btnSetting2.BackColor = Color.LightSkyBlue;
            btnSetting3.BackColor = Color.Green;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            lblRecording.BackColor = Color.Red;
            lblRecording.Text = "Now Recording";
            btnStart.BackColor = Color.Green;

            //need to change to get actual name and numbers
            bargeName = textBox_bargeName.Text;
            startArea = workingAreaName;
            startDMX = Convert.ToString(utmDMX);
            startDMY = Convert.ToString(utmDMY);
            DateTime currentTime = DateTime.Parse(DateTime.Now.ToString());
            startTime = currentTime.ToString("HHmm");
            btnComplete.Enabled = true;
            textBox_bargeName.Enabled = false;
        }

        private void btnComplete_Click(object sender, EventArgs e)
        {
            lblRecording.BackColor = Color.LightSkyBlue;
            lblRecording.Text = "Record";
            btnStart.BackColor = Color.LightSkyBlue;

            //need to change to get actual name and numbers
            completeArea = workingAreaName;
            completeDMX = Convert.ToString(utmDMX);
            completeDMY = Convert.ToString(utmDMY);
            finalElevation = Convert.ToString(autolet);
            DateTime currentTime = DateTime.Parse(DateTime.Now.ToString());
            completeTime = currentTime.ToString("HHmm");

            SaveCSVFiles();
            btnComplete.Enabled = false;
            textBox_bargeName.Enabled = true;
        }

        private void btnSetObject_Click(object sender, EventArgs e)
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
                    isObjectLineDrawn = true;
                    AddLines(objectLatLngList, objectColor);
                    PointLatLng center = CalculatePolygonCenter(objectLatLngList);
                    gmap.Position = center;
                    gmap.Zoom = 17;
                    gmap.Refresh();
                    btnShowObject.Enabled = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error");
                    //Console.WriteLine("An error occurred: " + ex.Message);
                }
            }
        }

        private void btnSetGrid_Click(object sender, EventArgs e)
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
                        btnShowGrid.Enabled = true;
                    }

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
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error");
                    //Console.WriteLine("An error occurred: " + ex.Message);
                }
            }
        }

        private void txtBearing_TextChanged(object sender, EventArgs e)
        {
            //if (System.Text.RegularExpressions.Regex.IsMatch(txtBearing.Text, "[^0-9]"))
            if (System.Text.RegularExpressions.Regex.IsMatch(txtBearing.Text, "[^0-9.]+"))
            {
                MessageBox.Show("Please enter only numbers.");
                txtBearing.Text = txtBearing.Text.Remove(txtBearing.Text.Length - 1);
            }
            if (txtBearing.Text != "")
            {
                gmap.Bearing = (float)Convert.ToDouble(txtBearing.Text);
                gmap.Refresh();
            }
        }

        private void btnShowObject_Click(object sender, EventArgs e)
        {
            if (objectLatLngList.Count > 0)
            {
                //draw object line
                if (!isObjectLineDrawn)
                {
                    AddLines(objectLatLngList, objectColor);
                    isObjectLineDrawn = true;
                    btnShowObject.Enabled = true;
                    btnShowObject.Text = "Hide Object";
                }
                else
                {
                    linesOverlay.Clear();
                    gmap.Refresh();
                    isObjectLineDrawn = false;
                    btnShowObject.Text = "Show Object";
                }
            }
        }

        private void btnShowGrid_Click(object sender, EventArgs e)
        {
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

                    btnShowGrid.Text = "Hide Grid";
                }
                else
                {
                    gridOverlay.Polygons.Clear();
                    gridOverlay.Markers.Clear();
                    gmap.Refresh();
                    isGridLineDrawn = false;

                    btnShowGrid.Text = "Show Grid";
                }
            }
        }
        private void btnScreenShot_Click(object sender, EventArgs e)
        {
            try
            {
                // Get the bounds of the primary screen
                //   Rectangle bounds = Screen.PrimaryScreen.Bounds;

                // Get the bounds of entire screen
                Rectangle bounds = Screen.GetBounds(Point.Empty);
                int scale = (int)(Screen.PrimaryScreen.Bounds.Width / System.Windows.SystemParameters.PrimaryScreenWidth);

                // Get the bounds of all connected screens
                // Rectangle bounds = GetCombinedScreenBounds();

                // Create a bitmap to hold the captured image
                //using (Bitmap screenshot = new Bitmap((int)(bounds.Width* scalingFactor), (int)(bounds.Height* scalingFactor)))
                using (Bitmap screenshot = new Bitmap(bounds.Width * scale, bounds.Height * scale))
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
                btnDropPin.BackColor = Color.LightSkyBlue;

            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            pinMarkerOverlay.Clear();
            gmap.Refresh();
            btnClear.Enabled = false;
        }

        private void btnAutolet_Click(object sender, EventArgs e)
        {
            isAutolet = true;
            double trueElevation = GPS2zHeight - 1.5 - levelSensor;//GPS2(z) - T - Level Sensor data
            lblElevation.Text = Convert.ToString(trueElevation) + " [m]";
            if (autolet > 0 && autolet < 25)
            {
                verticalBar1.Value = (int)(4 * autolet); // scaling since vertical bar has 0-100 and autolet has 0-25 meters
            }

            btnAutolet.BackColor = Color.YellowGreen;
            btnLevelSensor.BackColor = Color.SandyBrown;
        }

        private void btnLevelSensor_Click(object sender, EventArgs e)
        {
            isAutolet = false;
            double trueElevation = GPS2zHeight - 1.5 - levelSensor;//GPS2(z) - T - Level Sensor data
            lblElevation.Text = Convert.ToString(trueElevation) + " [m]";
            if (levelSensor > 0 && levelSensor < 10)
            {
                verticalBar1.Value = (int)(10 * levelSensor); // scaling since vertical bar has 0-100 and autolet has 0-10 meters
            }

            btnAutolet.BackColor = Color.SandyBrown;
            btnLevelSensor.BackColor = Color.YellowGreen;
        }
    }
}
