using System.Drawing;
using System.Windows.Forms;

namespace MonitoringSystem
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
