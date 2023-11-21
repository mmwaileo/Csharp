using System;
using System.Windows.Forms;

namespace MonitoringSystem
{
    public partial class UserControl2 : UserControl
    {
        public UserControl2()
        {
            InitializeComponent();
        }
        public string SetDimension_S
        {
            get => txtSetting2S.Text;
            set => txtSetting2S.Text = value;
        }
        public string SetDimension_T
        {
            get => txtSetting2T.Text;
            set => txtSetting2T.Text = value;
        }
        public string SetDimension_U
        {
            get => txtSetting2U.Text;
            set => txtSetting2U.Text = value;
        }
        private void btnSetDimension_Click(object sender, EventArgs e)
        {
            SetDimension_Click?.Invoke(this, e);
        }
        public event EventHandler SetDimension_Click;


    }
}
