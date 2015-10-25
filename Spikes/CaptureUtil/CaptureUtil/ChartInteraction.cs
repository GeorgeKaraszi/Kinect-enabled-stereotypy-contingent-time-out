using System;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace CaptureUtil
{
    public class ChartInteraction
    {
        private int PlotPointLimit { get; }

        public ChartInteraction(int plotPointLimit)
        {
            PlotPointLimit = plotPointLimit;
        }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// Inserts values into the GUI charts
        /// </summary>
        /// <param name="fvalue">Value to be inserted</param>
        public void Insert(double fvalue)
        {
            InsertToChart("chartKinectRaw", fvalue);

            if (MainForm.Recording)
            {
                InsertToChartRecord("chartRecording", fvalue);
            }
        }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// Inserts a float value into the GUI chart. This float value is converted to an
        ///  integer by multiplying the value by 1E3 to keep the floats numbers beyond 
        /// the decimal place.
        /// </summary>
        /// <param name="chartName">GUI chart name</param>
        /// <param name="fvalue">Value to be inserted</param>
        private void InsertToChart(string chartName, double fvalue)
        {
            Chart chart    = Application.OpenForms["MainForm"]?
                             .Controls["panel2"]
                             .Controls[chartName] as Chart;
            int index      = -1;      //Latest Index place in the plot table

            if (chart != null)       //Did we find our object?
            {
                index      = chart.Series[0].Points.Count;

                //Make sure we done plot beyond our limit
                if (index > PlotPointLimit)
                {
                    //Remove the oldest plot point and fix the index counter
                    chart.Series[0].Points.RemoveAt(0);
                    index--;

                    foreach (DataPoint dp in chart.Series[0].Points)
                        dp.XValue -= 1;
                }

                //Finally add the point to our graph for safe keeping
                chart.Series[0].Points.AddXY(index, fvalue);
                chart.Refresh();
            }
        }

        private void InsertToChartRecord(string chartName, double fvalue)
        {
            Chart chart = Application.OpenForms["MainForm"]?
                            .Controls["panel2"]
                            .Controls[chartName] as Chart;
            int index = -1;      //Latest Index place in the plot table

            if (chart != null)       //Did we find our object?
            {
                index = chart.Series[0].Points.Count;

                //Finally add the point to our graph for safe keeping
                chart.Series[0].Points.AddXY(index, fvalue);
                chart.Refresh();
            }
        }
    }
}