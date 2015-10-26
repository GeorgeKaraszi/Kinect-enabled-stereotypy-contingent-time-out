using System;
using System.Collections.Generic;
using System.Linq;

namespace CaptureUtil.GraphTools
{
    public static class SMath
    {
        //--------------------------------------------------------------------------------
        /// <summary>
        /// Averages the sum total of all float values in the given list
        /// </summary>
        /// <param name="value">List object containing float values</param>
        /// <returns>Average sum</returns>
        public static double AverageArray(List<double> value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            return AverageArray(value.ToArray());
        }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// Averages the sum total of all float values in the given list
        /// </summary>
        /// <param name="value">List object containing float values</param>
        /// <returns>Average sum</returns>
        public static double AverageArray(double[] value)
        {
            if (value == null || value.Length <= 0)
                throw new Exception("value array input must contain entries");

            if (value.Contains(double.NaN))
                throw new ArithmeticException(nameof(value) + " contains NaN values");

            return (value.Sum() / value.Length);
        }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// Calculates the standard deviation of an array set of numbers
        /// </summary>
        /// <param name="values"></param>
        /// <returns>The standard deviation of the array</returns>
        public static double CaculateStdDev(IEnumerable<double> values)
        {
            double sdReturn = 0;
            double avg = 0;
            double sum = 0;
            var enumerable = values as double[] ?? values.ToArray();

            if (enumerable.Any())
            {
                avg = enumerable.Average();
                sum = enumerable.Sum(d => Math.Pow(d - avg, 2));
                sdReturn = Math.Sqrt((sum) / (enumerable.Count() - 1));
                
            }

            return sdReturn;
        }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// Checks to see if the start has reached the compare value or the compare value 
        /// is between the start and end value set.
        /// </summary>
        /// <param name="start">Starting value</param>
        /// <param name="end">Ending value</param>
        /// <param name="compare">Value to what start and end compare to</param>
        /// <returns>True if it's the same as start or between start and end</returns>
        public static bool IntervalRange(double start, double end, double compare)
        {
            if (Math.Abs(start - compare) <= Double.Epsilon)
                return true;

            if (Math.Abs(end - compare) <= Double.Epsilon)
                return false;

            if (start <= compare && end >= compare)
                return true;

            return false;
        }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// Finds the smallest value in the given array.
        /// </summary>
        /// <param name="values">Array containing the values to be compared</param>
        /// <returns>index of the smallest value</returns>
        public static int FindSmallestValueIndex(double[] values)
        {
            int index = 0;

            for (int i = 0; i < values.Length; i++)
            {
                if (values[index] > values[i])
                {
                    index = i;
                }
            }

            return index;
        }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// Finds the largest value in the given array.
        /// </summary>
        /// <param name="values">Array containing the values to be compared</param>
        /// <returns>index of the largest value</returns>
        public static int FindLargestValueIndex(double[] values)
        {
            int index = 0;

            for (int i = 0; i < values.Length; i++)
            {
                if (values[index] < values[i])
                {
                    index = i;
                }
            }

            return index;
        }

    }
}