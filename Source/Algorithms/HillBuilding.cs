using System;
using System.Collections.Generic;

namespace WesternMichgian.SeniorDesign.KinectProject.Algorithms
{
    public class HillBuilding
    {
        private double PeakValleyDistance = 0.15;
        private List<Tuple<int, int>> hills;
        private List<double> wave;
        private double confidence;
        // Length of consecutive hills.
        private int seriesLength;

        public HillBuilding()
        {
            hills = null;
            wave = null;
            confidence = 0;
            seriesLength = 0;
        }

        /// <summary>
        /// Locates ranges over given wave in which:
        ///     every element is larger than every preceding element, or
        ///     every element is smaller than every preceding element,
        /// so long as the difference between the first and last element
        /// is greater than given threshold.
        /// </summary>
        /// <param name="wave">Data points that make up the wave recorded</param>
        /// <returns>
        /// List of hills, where
        /// Item1: index of start frame of hill
        /// Item2: index of end frame of hill
        /// Note:Return can be in ether Peak Valley Peak or Valley Peak Valley format.
        /// </returns>
        public List<Tuple<int, int>> BuildHills(List<double> wave)
        {
            // List of hills.
            hills = new List<Tuple<int, int>>();
            // Set the wave variable that can be accessed outside this method.
            this.wave = wave;

            // Start and end frames for each hill.
            int start = 0;
            int end = 0;

            if (wave == null)
                return null;
            
            for (int i = 0; i < wave.Count; i++)
            {
                // If start and end frames are same, auto-add i to hill.
                if (start == end)
                {
                    end = i;
                    continue;
                }
                // If hill slopes downward,
                if (wave[start] > wave[end])
                {
                    // If hill continues to slope downward, add to hill.
                    if (wave[end] >= wave[i])
                    {
                        end = i;
                    }
                    // If hill ceases to slope downward, save it if deep enough, reset.
                    else
                    {
                        if (wave[start] - wave[end] >= PeakValleyDistance)
                        {
                            hills.Add(new Tuple<int, int>(start, end));
                            seriesLength += (end - start - 1);
                            CalculateHill();
                        }
                        start = end = i-1;
                    }
                    continue;
                }
                // If hill slopes upward,
                if (wave[start] < wave[end])
                {
                    // If hill continues to slope upward, add to hill.
                    if (wave[end] < wave[i])
                    {
                        end = i;
                    }
                    // If hill ceases to slope upward, save it if tall enough, reset.
                    else
                    {
                        if (wave[end] - wave[start] >= PeakValleyDistance)
                        {
                            hills.Add(new Tuple<int, int>(start, end));
                            seriesLength += (end - start - 1);
                            CalculateHill();
                        }
                        start = end = i-1;
                    }
                }
            }

            return hills;
        }

        // Produce a confidence value for whether a stim has been detected in the hills.
        private void CalculateHill()
        {
            if (hills.Count == 0)
                return;

            if (wave == null)
                return;
            
            // Get last hill.
            Tuple<int, int> hill = hills[hills.Count - 1];


            // Possibly decay confidence value.
            if (hills.Count > 1)
            {
                // If there is a gap in between this hill and last,
                // decay confidence value by an appropriate amount,
                // corresponding to the length of the last hill.
                Tuple<int, int> lastHill = hills[hills.Count - 2];
                int gap = hill.Item1 - lastHill.Item2;
                double decay = ( seriesLength - gap ) / (double) seriesLength;
                confidence *= decay;
                seriesLength -= gap;
                if (confidence < 0)
                {
                    confidence = 0;
                    seriesLength = 0;
                }
            }

            // Start and end frames of hill.
            int start = hill.Item1;
            int end = hill.Item2;

            // Height and length of hill. Note: height can be negative.
            double height = wave[end] - wave[start];
            int length = end - start + 1;

            // The difference in height between each frame.
            double[] slopes = new double[length - 1];
            double avgSlope = 0;

            for (int i = 0; i < length - 1; i++)
            {
                slopes[i] = wave[start + i + 1] - wave[start + i];
                avgSlope += slopes[i];
            }
            avgSlope = avgSlope / (length - 1);

            // Number of top slopes to consider in slope calculation.
            int n = slopes.Length / 4 + 1;
            double topnSlopes = 0;
            double highSlope = 1;
            
            // Add up the highest n slopes and divide by n to find a high average.
            for (int i = 0; i < n; i++)
            {
                highSlope = NextLargestSlope(slopes, highSlope);
                topnSlopes += highSlope;
            }
            double highAvgSlope = topnSlopes / n;

            // Calculate boost to confidence from slope * height.
            double boost = highAvgSlope * height;

            confidence += boost;

            Console.WriteLine("wave[{0}] = {1}\n" +
                                     "wave[{2}] = {3}\n" +
                                     "height = {4}\n" +
                                     "average slope = {5}\n" +
                                     "high average slope = {6}\n" +
                                     "confidence boost = {7}\n" +
                                     "confidence = {8}\n\n",
                                     start, wave[start], end, wave[end],
                                     height, avgSlope, highAvgSlope, boost, confidence);
        }

        // Find the highest slope that is not higher than high.
        private double NextLargestSlope(double[] slopes, double limit)
        {
            double high_slope = 0;

            for (int i = 0; i < slopes.Length; i++)
            {
                if (Math.Abs(slopes[i]) > Math.Abs(high_slope) &&
                    Math.Abs(slopes[i]) < Math.Abs(limit))
                {
                    high_slope = slopes[i];
                }
            }

            return high_slope;
        }
    }
}