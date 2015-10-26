using System;
using System.Collections.Generic;

namespace CaptureUtil.Algorithms
{
    public class HillBuilding
    {
        private double PeakValleyDistance { get; } = 0.15;

        public HillBuilding() { }

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
            var hills = new List<Tuple<int, int>>();
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
                        }
                        start = end = i-1;
                    }
                    continue;
                }
            }

            return hills;
        }
    }
}