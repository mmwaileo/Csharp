using System;
using System.Windows.Forms;

namespace MonitoringSystem
{
    public partial class UserControl3 : UserControl
    {
        public UserControl3()
        {
            InitializeComponent();
        }
        public string Time
        {
            get => lblSetting3Time.Text;
            set => lblSetting3Time.Text = value;
        }

        public string GPS1xValue
        {
            get => lblSetting3GPS1x.Text;
            set => lblSetting3GPS1x.Text = value;
        }

        public string GPS1yValue
        {
            get => lblSetting3GPS1y.Text;
            set => lblSetting3GPS1y.Text = value;
        }
        public string GPS1zValue
        {
            get => lblSetting3GPS1z.Text;
            set => lblSetting3GPS1z.Text = value;
        }

        public string GPS1Mode
        {
            get => lblSetting3GPS1Mode.Text;
            set => lblSetting3GPS1Mode.Text = value;
        }
        public string GPS2xValue
        {
            get => lblSetting3GPS2x.Text;
            set => lblSetting3GPS2x.Text = value;
        }

        public string GPS2yValue
        {
            get => lblSetting3GPS2y.Text;
            set => lblSetting3GPS2y.Text = value;
        }
        public string GPS2zValue
        {
            get => lblSetting3GPS2z.Text;
            set => lblSetting3GPS2z.Text = value;
        }

        public string GPS2Mode
        {
            get => lblSetting3GPS2Mode.Text;
            set => lblSetting3GPS2Mode.Text = value;
        }
        public string Heading
        {
            get => lblSetting3Heading.Text;
            set => lblSetting3Heading.Text = value;
        }

        public string WorkingArea
        {
            get => lblSetting3WorkingArea.Text;
            set => lblSetting3WorkingArea.Text = value;
        }
        public string X1
        {
            get => lblSetting3x1.Text;
            set => lblSetting3x1.Text = value;
        }
        public string Y1
        {
            get => lblSetting3y1.Text;
            set => lblSetting3y1.Text = value;
        }
        public string X2
        {
            get => lblSetting3x2.Text;
            set => lblSetting3x2.Text = value;
        }
        public string Y2
        {
            get => lblSetting3y2.Text;
            set => lblSetting3y2.Text = value;
        }
        public string X3
        {
            get => lblSetting3x3.Text;
            set => lblSetting3x3.Text = value;
        }
        public string Y3
        {
            get => lblSetting3y3.Text;
            set => lblSetting3y3.Text = value;
        }
        public string X4
        {
            get => lblSetting3x4.Text;
            set => lblSetting3x4.Text = value;
        }
        public string Y4
        {
            get => lblSetting3y4.Text;
            set => lblSetting3y4.Text = value;
        }
        public string P1
        {
            get => lblSetting3P1.Text;
            set => lblSetting3P1.Text = value;
        }
        public string P2
        {
            get => lblSetting3P2.Text;
            set => lblSetting3P2.Text = value;
        }
        public string P3
        {
            get => lblSetting3P3.Text;
            set => lblSetting3P3.Text = value;
        }
        public string SS1
        {
            get => lblSetting3SS1.Text;
            set => lblSetting3SS1.Text = value;
        }
        public string SS2
        {
            get => lblSetting3SS2.Text;
            set => lblSetting3SS2.Text = value;
        }
        public string Trim
        {
            get => lblSetting3Trim.Text;
            set => lblSetting3Trim.Text = value;
        }
        public string Heel
        {
            get => lblSetting3Heel.Text;
            set => lblSetting3Heel.Text = value;
        }
        public string TideGauge
        {
            get => lblSetting3TideGauge.Text;
            set => lblSetting3TideGauge.Text = value;
        }
        public string LevelSensor
        {
            get => lblSetting3LevelSensor.Text;
            set => lblSetting3LevelSensor.Text = value;
        }
        public string Autolet
        {
            get => lblSetting3Autolet.Text;
            set => lblSetting3Autolet.Text = value;
        }
        public string PressureMax
        {
            get => txtSetting3PMax.Text;
            set => txtSetting3PMax.Text = value;
        }
        public string PressureMin
        {
            get => txtSetting3PMin.Text;
            set => txtSetting3PMin.Text = value;
        }
        public string PressureDangerousLimit
        {
            get => txtSetting3PDLimit.Text;
            set => txtSetting3PDLimit.Text = value;
        }
        public string ElevationMax
        {
            get => txtSetting3ElevationMax.Text;
            set => txtSetting3ElevationMax.Text = value;
        }
        public string ElevationMin
        {
            get => txtSetting3ElevationMin.Text;
            set => txtSetting3ElevationMin.Text = value;
        }
        public string ElevationInterval
        {
            get => txtSetting3ElevationInterval.Text;
            set => txtSetting3ElevationInterval.Text = value;
        }
        public string StrokeSensorMax
        {
            get => txtSetting3SSMax.Text;
            set => txtSetting3SSMax.Text = value;
        }
        public string StrokeSensorMin
        {
            get => txtSetting3SSMin.Text;
            set => txtSetting3SSMin.Text = value;
        }
        public string StrokeSensorInterval
        {
            get => txtSetting3SSInterval.Text;
            set => txtSetting3SSInterval.Text = value;
        }
        private void btnSetGauge_Click(object sender, EventArgs e)
        {
            SetGauge_Click?.Invoke(this, e);
        }
        public event EventHandler SetGauge_Click;

        private void txtSetting3SSMin_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtSetting3SSMax_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtSetting3ElevationInterval_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtSetting3ElevationMin_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtSetting3ElevationMax_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtSetting3PDLimit_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtSetting3PMin_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtSetting3PMax_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void label16_Click(object sender, EventArgs e)
        {

        }

        private void label18_Click(object sender, EventArgs e)
        {

        }

        private void label20_Click(object sender, EventArgs e)
        {

        }

        private void label21_Click(object sender, EventArgs e)
        {

        }

        private void label22_Click(object sender, EventArgs e)
        {

        }

        private void label24_Click(object sender, EventArgs e)
        {

        }

        private void label26_Click(object sender, EventArgs e)
        {

        }

        private void label27_Click(object sender, EventArgs e)
        {

        }

        private void label28_Click(object sender, EventArgs e)
        {

        }

        private void txtSetting3SSInterval_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
