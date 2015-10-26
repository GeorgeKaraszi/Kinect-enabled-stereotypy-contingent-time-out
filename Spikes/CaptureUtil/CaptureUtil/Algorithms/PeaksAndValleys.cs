using System;
using System.Collections.Generic;
using System.Linq;
using CaptureUtil.GraphTools;

namespace CaptureUtil.Algorithms
{
    public class PeaksAndValleys
    {
        private double AmplitudeThreshold { get; } = 0.15;
        private double PeakValleyDistance { get; } = 0.15;
        private int FrequencyThresholdMin { get; } = 5;
        private int FrequencyThresholdMax { get; } = 26;
        private int PeriodThreshold { get; }       = 3;

        public PeaksAndValleys() { }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// Finds all the peaks and valleys of a given wave data points, that fit a 
        /// threshold
        /// </summary>
        /// <param name="wave">Data points that make up the wave recorded</param>
        /// <returns>
        /// List of tuples
        /// Item1:index of found peak/valley
        /// Item2:value of peak/valley
        /// Note:Return can be in ether Peak Valley Peak or Valley Peak Valley format.
        /// </returns>
        public List<Tuple<int, double>> FindPeaksAndValleys(List<double> wave)
        {
            List<Tuple<int, double>> pvList = new List<Tuple<int, double>>();
            double peak   = 0;
            double valley = 1;
            int peakset   = -1;
            int valset    = -1;

            for (int i = 0; i < wave.Count; i++)
            {
                if (wave[i] <= peak && wave[i] >= valley)
                {
                    // Peaks and valleys must be > 0.15 apart.
                    if (peak - valley < PeakValleyDistance)
                    {
                        continue;
                    }
                    // If valset is oldest, save valley to list, reset valley, and continue.
                    if (valset < peakset)
                    {
                        //Be sure the valley is less then the last captured peak
                        if (pvList.Count > 0 && valley >= pvList.Last().Item2)
                        {
                            valset = i;
                            valley = wave[i];
                            continue;
                        }
                        
                        pvList.Add(new Tuple<int, double>(valset, valley));
                        valley = 1;
                        continue;
                    }

                    // If peakset is oldest, save peak to list, reset peak, and continue.
                    if (peakset < valset)
                    {
                        //Be sure the peak is higher then the last captured valley
                        if (pvList.Count > 0 && peak <= pvList.Last().Item2)
                        {
                            peakset = i;
                            peak = wave[i];
                            continue;
                        }
                        
                        pvList.Add(new Tuple<int, double>(peakset, peak));
                        peak = 0;
                        continue;
                    }
                }

                if (wave[i] > peak)
                {
                    peakset = i;
                    peak = wave[i];
                }

                if (wave[i] < valley)
                {
                    valset = i;
                    valley = wave[i];
                }
            }

            //PostProcessPvList(pvList);
            return pvList;
        }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// Processes and fixes the Peaks and Valley list of duplicates and 
        /// incorrect entries.
        /// </summary>
        /// <param name="pvList">List containing tuples of indexes and values</param>
        private void PostProcessPvList(List<Tuple<int, double>> pvList)
        {
            // Must first look for points that are not peaks nor valleys.
            Tuple<int, double> prev = new Tuple<int, double>(-1, -1.0);
            for (int i = 0; i < pvList.Count; i++)
            {
                Tuple<int, double> point = pvList[i];
                Tuple<int, double> next = new Tuple<int, double>(-1, -1.0);
                // If i is not the last element, set next to proceeding element.
                if (i + 1 < pvList.Count)
                {
                    next = pvList[i + 1];
                }
                // If there are indeed previous and next elements,
                if (prev.Item2 != -1.0 && next.Item2 != -1.0)
                {
                    // If a data point is in the middle of the two on either side, remove it.
                    if ((point.Item2 > prev.Item2 &&
                         point.Item2 < next.Item2) ||
                        (point.Item2 < prev.Item2 &&
                         point.Item2 > next.Item2))
                    {
                        pvList.RemoveAt(i);
                        i--;
                        continue;
                    }
                }

                // Set previous to current node.
                prev = pvList[i];
            }
        }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// Calculates the periods in a given graph wave based on its peaks and valleys
        /// </summary>
        /// <param name="wave">Data points that make the core of the wave</param>
        /// <param name="pv">Detected peaks and valleys of the wave</param>
        /// <param name="clearInconsistantPeriods">
        /// Clear the period counter if periods are inconsistent found next together
        /// </param>
        /// <param name="clearReturnTheshold">
        /// If the clear inconsistent flag is true and the period counted is equal or 
        /// greater then the threshold. The function will return back to the caller with 
        /// the amount of consistent periods found if an inconsistent portion is found.
        /// </param>
        /// <returns>The amount of periods found within the wave</returns>
        public int CalculateThePeriod(List<double> wave, 
                                      List<Tuple<int, double>> pv, 
                                      bool clearInconsistantPeriods = true, 
                                      int clearReturnTheshold = 3)
        {
            var pvPairs                     = GetPVPairs(pv,wave);
            int period                      = 0;    //Amount of periods found
            int frequency                   = 0;    //Frequency of analyzed wave
            int incrementBy                 = 0;    //Number of elements to increase by
            Tuple<double, double> midAmp    = null; //Mid line and amplitude
            Tuple<double, double> nxtMidAmp = null; //Neighbors mid line and amplitude

            if (pvPairs == null)
                return 0;

            for (int i = 0; i < pvPairs.Count; i = incrementBy)
            {
                incrementBy = i + 1;
                midAmp = MidlineAmp(pvPairs[i], wave);

                if (midAmp.Item2 < AmplitudeThreshold)
                {
                    continue;
                }

                do
                {
                    //Verify that we do not run off the end of the list
                    if (incrementBy >= pvPairs.Count)
                    {

                        nxtMidAmp = null;
                        break;
                    }

                    //Get neighboring set's middle line and amplitude values
                    nxtMidAmp = MidlineAmp(pvPairs[incrementBy++], wave);

                    //Verify it meets the designated threshold
                } while (nxtMidAmp.Item2 < AmplitudeThreshold);


                //No partner was found, we've reached end of pvPairs
                if (nxtMidAmp == null)
                    continue;

                //Obtain the frequency between the two neighboring pairs
                frequency = FrequencyBetween(midAmp.Item1,
                                             nxtMidAmp.Item1,
                                             pvPairs[i],
                                             pvPairs[incrementBy-1],
                                             wave);

                //If the frequency requirements are reached, count it as a period.
                if (frequency >= FrequencyThresholdMin && 
                    frequency <= FrequencyThresholdMax)
                {
                    ++period;
                }
                else if (clearInconsistantPeriods)
                {
                    if (clearReturnTheshold != 0 && period >= clearReturnTheshold)
                    {
                        return period;
                    }

                    period = 0;
                }

                incrementBy--;
            }


            return period;
        }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// Build a list of index's that are pairs of Valley, Peak, Valley sets
        /// </summary>
        /// <param name="pv">List of PV's</param>
        /// <param name="wave">Data point that make the core of the wave</param>
        /// <returns>
        /// Returns a list of tuple containing
        /// Item1: Left side valley point
        /// Item2: Peak index
        /// Item3: Right side valley point
        /// </returns>
        private List<Tuple<int, int, int>> GetPVPairs(List<Tuple<int, double>> pv, 
                                                      List<double> wave)
        {
            var pvPairs = new List<Tuple<int,int,int>>();
            int i = 0;

            if (pv == null || pv.Count < 2)
            {
                return null;
            }

            //Check to see if the frist index is a peak or valley, skip if it's peak.
            //--->This is a quick and dirty way of having valid pattern (VPV).
            if (pv[i].Item2 > pv[i + 1].Item2)
                i++;

            //Gather all pairs of Valley_Peak_Valley's in the PV list
            for (; i < pv.Count; i += 2)
            {
                if (i + 2 < pv.Count)
                {
                    var tuple3 = new Tuple<int,int,int>(pv[i].Item1, 
                                                        pv[i+1].Item1, 
                                                        pv[i+2].Item1);
                    pvPairs.Add(tuple3);
                }
                else if(i + 1 < pv.Count)
                {
                    var subArray      = wave.Skip(pv[i + 1].Item1).ToArray();
                    var smallestIndex = SMath.FindSmallestValueIndex(subArray) + 
                                        pv[i + 1].Item1;

                    var tuple2 = new Tuple<int, int, int>(pv[i].Item1,
                                                          pv[i + 1].Item1,
                                                          smallestIndex);
                    pvPairs.Add(tuple2);
                }
            }

            return pvPairs;
        }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// Calculates the mid line and amplitude of a given set of valley and peaks
        /// </summary>
        /// <param name="focus">Valley, Peak, Valley focus set of indexes to PV</param>
        /// <param name="wave">
        /// Data points that are used to produce the wave being analyzed.
        /// </param>
        /// <returns>
        /// Tuple containing 
        /// Item1: mid line between peak and valley
        /// Item2: amplitude between peak and valley
        /// </returns>
        private Tuple<double, double> MidlineAmp(Tuple<int, int, int> focus,
                                                  List<double> wave)
        {
            double valleyAverage = ( wave[focus.Item1] + wave[focus.Item3] )/2;
            double midline       = (wave[focus.Item2] + valleyAverage) / 2;
            double amplitude     = Math.Abs(wave[focus.Item2] - midline);


            return new Tuple<double,double>(midline,amplitude);
        }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// Counts the amount of indexes that where passed to reach the two midpoints 
        /// </summary>
        /// <param name="stMidline">Midpoint of the first peak</param>
        /// <param name="enMidline">Midpoint of the second peak</param>
        /// <param name="pvPair1">
        /// First pair of indexes that contain VPV of stMidline
        /// </param>
        /// <param name="pvPair2">
        /// Second pair of indexes that contain VPV of enMidline
        /// </param>
        /// <param name="wave">
        /// Data points that contains the values being analyze
        /// </param>
        /// <returns>
        /// Returns the index count it took to get from stMidline to enMidline.
        /// Returns 0 if it did not reach stMidline to enMidline
        /// </returns>
        private int FrequencyBetween(double stMidline,
                                     double enMidline,
                                     Tuple<int, int, int> pvPair1,
                                     Tuple<int, int, int> pvPair2,
                                     List<double> wave)
        {
            int index         = pvPair1.Item1;  //Starting base valley index (left side)
            int indexEnd      = 0;              //Ending point 2nd peak valley
            int passedMidline = 0;              //Amount of times passed a midpoint
            int frequency     = 0;              //index counter, after passing midpoints

            //Check to see if we are going to calculate the end of the wave 
            // instead of two peak points.
            if (pvPair1.Item3 == -1 || pvPair2 == null)
            {
                indexEnd      = wave.Count - 1;
            }
            else
            {
                indexEnd      = pvPair2.Item2;
            }


            do
            {
                switch (passedMidline)
                {
                    case 0: //We are at the left side valley, going up to the peak
                        if (SMath.IntervalRange(wave[index], wave[index + 1], stMidline))
                        {
                            passedMidline++;
                        }
                        break;
                    case 1: //We are at the peak going down to the right side valley
                        if (SMath.IntervalRange(wave[index + 1], wave[index], stMidline))
                        {
                            passedMidline++;
                        }
                        break;
                    default://We are at the valley going to the next peak
                        if (pvPair2 != null && index >= pvPair2.Item1)
                        {
                            if (SMath.IntervalRange(wave[index],
                                                    wave[index + 1],
                                                    enMidline))
                            {
                                passedMidline++;
                            }
                        }
                        break;
                }

                if (passedMidline > 0)
                    frequency++;

            } while (index++ < indexEnd 
                    && passedMidline < 3 
                    && (index+1) < wave.Count);

            //if we passed or reached the mid lines, return frequency, otherwise 0
           return (passedMidline) == 3 ? frequency:0;
        }
    }
}