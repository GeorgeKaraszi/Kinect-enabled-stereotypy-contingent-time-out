using System;
using System.Collections.Generic;
using System.Linq;

namespace CaptureUtil.GraphTools
{
    public class SmoothIteration
    {

        //--------------------------------------------------------------------------------
        /// <summary>
        /// All in one solution to smoothing out crowed graphing peaks and valleys
        /// </summary>
        /// <param name="points">
        ///     Initial value of graph points needing to be corrected
        /// </param>
        /// <param name="level">
        /// 1 = Default
        /// 2 = Average:Increment by 4:3
        /// 3 = Average:Increment by 5:3
        /// </param>
        /// <returns>
        ///     List of new graph points that have been averaged and corrected
        /// </returns>
        public List<double> SmoothGraph(List<double> points, int level = 1)
        {
            List<double> smoothedGraph = null;       //Final list of plot points

            if (points == null)
                throw new ArgumentNullException(nameof(points));

            switch (level)
            {
                case 2:
                    smoothedGraph = AveragePlotPoints(points, 4, 3);
                    smoothedGraph = SmoothRange(smoothedGraph);
                    break;
                case 3:
                    smoothedGraph = AveragePlotPoints(points, 5, 3);
                    smoothedGraph = SmoothRange(smoothedGraph);
                    break;
                default:
                    smoothedGraph = AveragePlotPoints(points);
                    smoothedGraph = SmoothRange(smoothedGraph);
                    break;
            }

            return smoothedGraph;
        }

        //--------------------------------------------------------------------------------
        /// <summary>
        ///  Calculates the standard deviation of sections within the wave. If a section 
        /// doesn't meet's it's threshold, it'll attempt to smooth the detect section out 
        /// by averaging the given values.
        /// </summary>
        /// <param name="wave">Captured wave to be analyzed</param>
        /// <returns>
        /// Null if the overall wave doesn't meet a threshold on standard deviation.
        /// Otherwise a new wave that has been modified is returned.
        /// </returns>
        public List<double> HandleDeviation(double[] wave)
        {
            //Copy the wave into a new buffer
            List<double> newWave = new List<double>(wave);
            //Find how many elements a section of six holds
            int sections = (int)wave.Count() / 6;

            //Check to see if the wave is even worth the time working on
            if (CMath.CaculateStdDev(newWave) < 0.2)
            {
                return null;
            }

            for (int i = 0; i < 6; i++)
            {
                var datapoints = newWave.Skip(i * sections).Take(sections);

                if (CMath.CaculateStdDev(datapoints) < 0.23)
                {
                    var avg = datapoints.Min();
                    for (int j = 0; j < sections; j++)
                    {
                        newWave[i * sections + j] = avg;
                    }
                }
            }

            return newWave;

        }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// Does a single iteration through floating points to see if a given range of 
        /// points contain similar values that match a designated rage caliber.
        /// If values are found to be matched within the checked range, they are averaged 
        /// out to create a new data point. Otherwise will return the given list to the 
        /// final result.
        /// </summary>
        /// <param name="points">List of float valued plot points</param>
        /// <param name="smoothBy">
        /// Points to range check during each iteration
        /// Recommend: 2:1 or 3:2 ratio of smoothBy:incrementBy
        /// </param>
        /// <param name="incrementBy">
        /// How far to continue to increment after each check
        /// Recommend: 2:1 or 3:2 ratio of smoothBy:incrementBy
        /// </param>
        /// <param name="epsilonRage">Range value used for element comparison</param>
        /// <returns>Finalize list of smoothed values</returns>
        private List<double> SmoothRange(List<double> points, int smoothBy = 3,
            int incrementBy = 2, float epsilonRage = 0.01f)
        {

            //Argument error checking
            if (points == null)
                throw new ArgumentNullException(nameof(points));
            if (smoothBy < 2)
                throw new ArgumentException("smoothBy must be 2 or greater");
            if (incrementBy < 1)
                throw new ArgumentException("incrementBy must be 1 or greater");

            List<double> smoothPoints = new List<double>(); //Final graph points
            bool doneSmoothing = false;     //Signal flag of finished operation
            int index = 0;         //Argument(points) index
            int incRange = Math.Abs(smoothBy - incrementBy);

            while (doneSmoothing == false)          //Are we done with the operation?
            {
                //List that will contain a segment rage of List Argument(point)
                List<double> tempList = new List<double>(smoothBy);
                //Signal that temp list holds similar values

                //Continue to loop through the Argument(point) coping segments
                for (int i = 0; i < smoothBy; i++)
                {
                    if ((index + i) == points.Count)//Check if we have reached the end
                    {
                        doneSmoothing = true;
                        break;
                    }

                    tempList.Add(points[index + i]);//Gather data points for comparison
                }

                index += incrementBy;

                //Compare all held values in tempList to determine if there are any 
                //  similar values by a given range
                bool matchFound = tempList.ToArray()
                    .Select(compareValue => tempList.FindAll(delegate(double value)
                {
                    //IF compareValue == (checking value)
                    if (Math.Abs(value - compareValue) < double.Epsilon)
                        return true;

                    //IF (checking value)  is < and > (compareValue + Range)
                    if (value <= ( compareValue + epsilonRage ) && 
                         value >= ( compareValue - epsilonRage ))
                        return true;

                    return false;
                })).Any(tempList2 => tempList2.Count >= 2);

                if (matchFound)                     //if true, average all temp points
                {
                    smoothPoints.Add(CMath.AverageArray(tempList));
                }
                else                 //Otherwise add all points as they are clear and good
                {
                    if ((index - incrementBy) != 0)
                    {
                        smoothPoints.AddRange(tempList.GetRange(incRange,
                                                                tempList.Count - incRange));
                    }
                    else
                    {
                        smoothPoints.AddRange(tempList);
                    }
                }

            }

            return smoothPoints;
        }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// Averages the graph plot points to help correct crowed peaks and valley points.
        /// This is most useful for smoothing out initial points on a graph.
        /// </summary>
        /// <param name="points">List of float valued plot points</param>
        /// <param name="averageBy">
        /// How many points to average by while acceding the plot points argument
        /// </param>
        /// <param name="incrementBy">
        /// The rate of which the plot points will increment after each average operation 
        /// is performed.
        /// </param>
        /// <returns>List of new graph points that have been averaged out</returns>
        private List<double> AveragePlotPoints(List<double> points, int averageBy = 3,
             int incrementBy = 2)
        {

            //Argument error checking
            if (points == null)
                throw new ArgumentNullException(nameof(points));
            if (averageBy < 2)
                throw new ArgumentException("averageBy must be 2 or greater");
            if (incrementBy < 1)
                throw new ArgumentException("incrementBy must be 1 or greater");


            List<double> averageFloats = new List<double>(); //Final list of averaged points
            bool doneAveraging = false;      //Flag for signaling opt. complete
            int index = 0;                   //Argument(points) index value

            while (doneAveraging == false)   //Are we done averaging points?
            {
                double averageTemp = 0;     //Tally sum of plot points
                int count = 0;              //index incrementor and deviser

                //Add the sum total amount of plot points determined by 
                // Argument(averageBy)
                while (count < averageBy)
                {
                    //Check to see if we are within proper array range
                    if ((index + count) == points.Count)
                    {
                        doneAveraging = true;
                        count -= 1;                 //Fix any type of invalid increments
                        break;
                    }

                    averageTemp += points[index + count++];
                }

                //Increment index of plot points by Argument(incrementBy)
                index += incrementBy;

                //Average the final amount by the amount counted from the above sum total 
                averageTemp = (averageTemp / (count <= 0 ? 1 : count));
                averageFloats.Add(averageTemp);     //Add result to new plot graph
            }

            return averageFloats;
        }
    }
}