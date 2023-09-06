using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Monitoring_System
{
    public class SplashScreen : Form
    {
        public SplashScreen()
        {
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.None;

            Label loadingLabel = new Label();
            loadingLabel.Text = "Application is starting up...";
            loadingLabel.Dock = DockStyle.Fill;
            loadingLabel.TextAlign = ContentAlignment.MiddleCenter;

            this.Controls.Add(loadingLabel);
        }
    }
}
