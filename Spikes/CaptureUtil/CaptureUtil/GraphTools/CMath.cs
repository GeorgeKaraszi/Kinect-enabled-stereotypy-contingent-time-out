using System;
using System.Collections.Generic;
using System.Linq;

namespace CaptureUtil.GraphTools
{
    public static class CMath
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
                sum = enumerable.Sum(d => CMath.Pow(d - avg, 2));
                sdReturn = Math.Sqrt((sum) / (enumerable.Count() - 1));
                
            }

            return sdReturn;
        }
    }
}