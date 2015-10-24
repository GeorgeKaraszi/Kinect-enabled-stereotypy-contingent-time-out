using System;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Gestures.HMMs
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
        public void Insert(float fvalue)
        {
            InsertToChart("chartKinectRaw", fvalue);

            if (MainForm.Recording)
            {
                InsertToChart("chartPattern", fvalue);
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
        private void InsertToChart(string chartName, float fvalue)
        {
            Chart chart    = Application.OpenForms["MainForm"]?
                             .Controls["panel"]
                             .Controls[chartName] as Chart;
            int index      = -1;      //Latest Index place in the plot table
            int convertedY = -1;      //Converts all float decimals points to whole number

            if (chart != null)       //Did we find our object?
            {
                index      = chart.Series[0].Points.Count;

                //Kinect value is between 0 & 1. Float can have 9 decimal places. Thus 
                //multiply by 100k to strip the decimal to create whole numbers.
                convertedY = Convert.ToInt32(fvalue * 1E3);

                //Since kinect is 0-1 values. The conversion can only be between 0-10,000
                if (convertedY > 1e4 || convertedY < 0)
                    return;

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
                chart.Series[0].Points.AddXY(index, convertedY);
                chart.Refresh();
            }
        }
    }
}