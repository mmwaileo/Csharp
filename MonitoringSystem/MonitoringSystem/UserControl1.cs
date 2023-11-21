using System;
using System.Windows.Forms;

namespace MonitoringSystem
{
    public partial class UserControl1 : UserControl
    {
        public UserControl1()
        {
            InitializeComponent();
        }
        public string SetDimension_A
        {
            get => txtSetting1A.Text;
            set => txtSetting1A.Text = value;
        }
        public string SetDimension_B
        {
            get => txtSetting1B.Text;
            set => txtSetting1B.Text = value;
        }
        public string SetDimension_C
        {
            get => txtSetting1C.Text;
            set => txtSetting1C.Text = value;
        }
        public string SetDimension_D
        {
            get => txtSetting1D.Text;
            set => txtSetting1D.Text = value;
        }
        public string SetDimension_E
        {
            get => txtSetting1E.Text;
            set => txtSetting1E.Text = value;
        }
        public string SetDimension_F
        {
            get => txtSetting1F.Text;
            set => txtSetting1F.Text = value;
        }
        public string SetDimension_G
        {
            get => txtSetting1G.Text;
            set => txtSetting1G.Text = value;
        }
        public string SetDimension_H
        {
            get => txtSetting1H.Text;
            set => txtSetting1H.Text = value;
        }
        public string SetDimension_I
        {
            get => txtSetting1I.Text;
            set => txtSetting1I.Text = value;
        }
        public string SetDimension_J
        {
            get => txtSetting1J.Text;
            set => txtSetting1J.Text = value;
        }
        public string SetDimension_K
        {
            get => txtSetting1K.Text;
            set => txtSetting1K.Text = value;
        }
        public string SetDimension_L
        {
            get => txtSetting1L.Text;
            set => txtSetting1L.Text = value;
        }
        public string SetDimension_M
        {
            get => txtSetting1M.Text;
            set => txtSetting1M.Text = value;
        }
        public string SetDimension_N
        {
            get => txtSetting1N.Text;
            set => txtSetting1N.Text = value;
        }
        public string SetDimension_O
        {
            get => txtSetting1O.Text;
            set => txtSetting1O.Text = value;
        }
        public string SetDimension_P
        {
            get => txtSetting1P.Text;
            set => txtSetting1P.Text = value;
        }
        public string SetDimension_Q
        {
            get => txtSetting1Q.Text;
            set => txtSetting1Q.Text = value;
        }
        public string SetDimension_R
        {
            get => txtSetting1R.Text;
            set => txtSetting1R.Text = value;
        }
        public string SetDimension_FWPort //forward port
        {
            get => txtSetting1FWPort.Text;
            set => txtSetting1FWPort.Text = value;
        }
        public string SetDimension_FWSTB //forward starboard
        {
            get => txtSetting1FWSTB.Text;
            set => txtSetting1FWSTB.Text = value;
        }
        public string SetDimension_AFTPort //aft port
        {
            get => txtSetting1AFTPort.Text;
            set => txtSetting1AFTPort.Text = value;
        }
        public string SetDimension_AFTSTB //aft starboard
        {
            get => txtSetting1AFTSTB.Text;
            set => txtSetting1AFTSTB.Text = value;
        }
        private void btnSetDimension_Click(object sender, EventArgs e)
        {
            SetDimension_Click?.Invoke(this, e);
        }
        public event EventHandler SetDimension_Click;
        private void btnSetDraft_Click(object sender, EventArgs e)
        {
            SetDraft_Click?.Invoke(this, e);
        }
        public event EventHandler SetDraft_Click;


    }
}
