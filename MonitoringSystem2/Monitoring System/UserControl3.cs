using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Monitoring_System
{
    public partial class UserControl3 : UserControl
    {
        public UserControl3()
        {
            InitializeComponent();
            //lblSetting3Time.Text = DateTime.Now.ToString();
                        
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
            get => lblSetting3GPS1z.Text;
            set => lblSetting3GPS1z.Text = value;
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
            get => lblSetting3x1.Text;
            set => lblSetting3x1.Text = value;
        }
        public string Y2
        {
            get => lblSetting3y1.Text;
            set => lblSetting3y1.Text = value;
        }
        public string X3
        {
            get => lblSetting3x1.Text;
            set => lblSetting3x1.Text = value;
        }
        public string Y3
        {
            get => lblSetting3y1.Text;
            set => lblSetting3y1.Text = value;
        }
        public string X4
        {
            get => lblSetting3x1.Text;
            set => lblSetting3x1.Text = value;
        }
        public string Y4
        {
            get => lblSetting3y1.Text;
            set => lblSetting3y1.Text = value;
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
        private void btnSetGauge_Click(object sender, EventArgs e)
        {
            SetGauge_Click?.Invoke(this, e);
        }
        public event EventHandler SetGauge_Click;
    }
}
