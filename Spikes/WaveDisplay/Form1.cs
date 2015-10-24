using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace WaveDisplay
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void DisplayToChart(List<double> entry, Series series)
        {
            for (int i = 0; i < entry.Count; i++)
            {
                series.Points.AddXY(i, entry[i]);
            }
        }

        private void DisplayPointChart(Tuple<int, double>[] points, Series series)
        {
            foreach (var tup in points)
            {
                series.Points.AddXY(tup.Item1, tup.Item2);
            }
        }

        private void openFileDialog_FileOk(object sender, CancelEventArgs e)
        {
            List<double> wavePoints = new List<double>();
            WaveUtilities wu = new WaveUtilities();
            WaveAnalysis wa = new WaveAnalysis();

            using (var stream = new StreamReader(openFileDialog.OpenFile()))
            {
                string line;
                while ((line = stream.ReadLine()) != null)
                {
                    wavePoints.Add(Convert.ToDouble(line));
                }
            }


            List<double> StdDev = wu.HandleDeviation(wavePoints.ToArray());
            List<double> smoothedDev = wu.HandleDeviation(wavePoints.ToArray());
            smoothedDev = wu.SmoothGraph(smoothedDev);

            var justSmoothed = wu.SmoothGraph(wavePoints);

            var smoothedPV = wa.SmoothPeaksAndValleys(justSmoothed);
           // smoothedPV = wa.SmoothPeaksAndValleys(smoothedPV);
            //smoothedPV = wu.SmoothGraph(smoothedPV);


            chart1.Series[0].Points.Clear();
            chart1.Series[1].Points.Clear();

            chart2.Series[0].Points.Clear();
            chart2.Series[1].Points.Clear();

            chart3.Series[0].Points.Clear();
            chart3.Series[1].Points.Clear();

            var wavePoint = wa.FindPeaksAndValleys(wavePoints);
            var stdPoint = wa.FindPeaksAndValleys(StdDev);
            var smoothPoint = wa.FindPeaksAndValleys(justSmoothed);
            var newSmoothedPoints = wa.FindPeaksAndValleys(smoothedPV);

            DisplayToChart(wavePoints, chart1.Series[0]);
            DisplayToChart(justSmoothed, chart2.Series[0]);
            DisplayToChart(smoothedPV, chart3.Series[0]);

            DisplayPointChart(wavePoint, chart1.Series[1]);
            DisplayPointChart(smoothPoint, chart2.Series[1]);
            DisplayPointChart(newSmoothedPoints, chart3.Series[1]);
        }

        private void btnLoadFile_Click(object sender, EventArgs e)
        {
            openFileDialog.ShowDialog();
        }
    }
}
