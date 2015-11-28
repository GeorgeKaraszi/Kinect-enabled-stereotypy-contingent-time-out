using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CaptureReportTool
{
    public partial class CRTForm : Form
    {
        private UtilWindow _utilWindow;
        public int TrackedBodies { get; set; }

        public CRTForm()
        {
            List<string> GestureNames = new List<string>
                                        {
                                            "HandFlapping",
                                            "HandSideToSide",
                                            "Rocking"
                                        };
            TrackedBodies = 0;
            _utilWindow = new UtilWindow(GestureNames.ToArray());
            //_utilWindow.OnGestureTargetChange += GestureTargetChange;
            InitializeComponent();
        }

        private void GestureTargetChange(object source,
                                         UtilTargetGestArgs utilTargetGestArgs)
        {
            gestureLbl.Text = utilTargetGestArgs.GetBodyId() + @" : " + 
                              utilTargetGestArgs.GetGestureName();
        }

        private void openWndBtn_Click(object sender, EventArgs e) { _utilWindow.Show(); }

        private void trackBtn_Click(object sender, EventArgs e)
        {
            TrackedBodies = Int32.Parse(activeTracking.Text);
        }

        private void activeTracking_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown numb = (NumericUpDown) sender;
            _utilWindow.UpdateTracking(Convert.ToInt32(numb.Value), true);
        }

        private void rmv_topbtn_Click(object sender, EventArgs e)
        {
            _utilWindow.UpdateTracking(1, false);
        }

        private void rmv_midbtn_Click(object sender, EventArgs e)
        {
            _utilWindow.UpdateTracking(2, false);
        }

        private void rmv_btmbtn_Click(object sender, EventArgs e)
        {
            _utilWindow.UpdateTracking(3, false);
        }
    }
}
