using System;
using System.Collections.Generic;
using System.Linq;

namespace WesternMichgian.SeniorDesign.KinectProject.Algorithms
{
    public class GestureInterpreter
    {
        // Circular array implementation of wave buffer.
        private readonly double[] _wave;

        // List of peaks and valleys.
        readonly List<PeakValley> _pvList;
        // List of PVPairs (VPV or PVP) for calculating frequency.
        readonly List<PvPair> _pvPairs;
        // List of periods. It's a list so it can be decremented as PVPairs are removed.
        readonly List<int> _periods;

        // Size of wave buffer to analyze.
        private const int WindowSize      = 60;

        // Minimum and maximum frequency.
        private const int MinFrequency    = 5;
        private const int MaxFrequency    = 26;

        //Minimum value in which triggers the event
        private const int PeriodThreshold = 3;

        // Minimum distance between peaks and valleys.
        private const double MinPeakValleyDistance = 0.15;

        // Number of frames for counting.
        private int Frame { get; set; }

        // Peak and valley data initialization.
        private double Peak { get; set; }
        private double Valley { get; set; }
        // Frames at which the peak and valley is set.
        private int Peakset { get; set; }
        private int Valset { get; set; }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// Initializes all necessary data members of the GestureInterpreter class.
        /// </summary>
        public GestureInterpreter()
        {
            _wave    = new double[WindowSize];

            _pvList  = new List<PeakValley>();
            _pvPairs = new List<PvPair>();
            _periods = new List<int>();

            Frame    = -1;

            Peak     = 0;
            Valley   = 1;

            Peakset  = -1;
            Valset   = -1;
        }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// Process input and return if hand flap has been detected.
        /// </summary>
        /// <param name="val"> Double value from gesture detector. </param>
        /// <returns> 
        /// A boolean of whether sufficient hand-flapping has been detected or not. 
        /// </returns>
        public bool ProcessPoint(double val)
        {
            bool returnVal = false;
            int index      = ++Frame % WindowSize;
            _wave[index]   = val;

            // First, check if there is to be a PV removed from the list.
            if (_pvList.Count > 0 && _pvList.First().Frame <= Frame - WindowSize)
            {
                _pvList.RemoveAt(0);
            }

            // And if there is a PVPair to be removed from the list.
            if (_pvPairs.Count > 0 && _pvPairs.First().Frames.Item1 <= Frame - WindowSize)
            {
                PvPair pvp = _pvPairs.First();

                // If the removal of this PVPair entitles the removal of a period.
                if (_periods.Count > 0 && pvp.Frames.Item1 == _periods.First())
                {
                    _periods.RemoveAt(0);
                }
                _pvPairs.RemoveAt(0);
            }

            // If the point is between peak and valley
            if (_wave[index] <= Peak && _wave[index] >= Valley)
            {
                if (Peak - Valley < MinPeakValleyDistance)
                {
                    return false;
                }
                // If valset is oldest, save valley to list, reset valley, and continue.
                if (Valset < Peakset)
                {
                    //Be sure the valley is less then the last captured peak.
                    if (_pvList.Count > 0 && Valley >= _pvList.Last().Value)
                    {
                        Valset = Frame;
                        Valley = _wave[index];
                        return false;
                    }

                    //If Adding a PeakValley triggers hand-flapping detection, return true.
                    returnVal = AddPv(new PeakValley(Valset, Valley));

                    Valley = 1;
                    return returnVal;
                }

                // If peakset is oldest, save peak to list, reset peak, and continue.
                if (Peakset < Valset)
                {
                    //Be sure the peak is higher then the last captured valley
                    if (_pvList.Count > 0 && Peak <= _pvList.Last().Value)
                    {
                        Peakset = Frame;
                        Peak = _wave[index];
                        return false;
                    }

                    //If Adding a PeakValley triggers hand-flapping detection, return true.
                    returnVal = AddPv(new PeakValley(Peakset, Peak));

                    Peak = 0;
                    return returnVal;
                }
            }
            if (_wave[index] > Peak)
            {
                Peakset = Frame;
                Peak = _wave[index];
            }
            if (_wave[index] < Valley)
            {
                Valset = Frame;
                Valley = _wave[index];
            }

            return false;
        }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// Add a peak or a valley (a PV) to the list of PVs.
        /// </summary>
        /// <param name="pv"> Peak or valley to add to list. </param>
        /// <returns>
        /// If sufficient hand-flapping is detected in adding this PV,
        /// return true, otherwise return false.
        /// </returns>
        private bool AddPv(PeakValley pv)
        {
            Tuple<double, double> midAmp = null;
            int[] pvGroup                = null;

            _pvList.Add(pv);

            // If there are at least three points and the last point is not the third 
            // point in the last PVPair
            // (if there is one), then create a PVPair from these three points.
            if (_pvList.Count <= 2)
                return false;

            if (_pvPairs.Count > 0 && 
                _pvPairs.Last().Frames.Item3 == _pvList[_pvList.Count - 2].Frame)
            {
                return false;
            }

            //Gather a group of indexes that form the wave
            pvGroup = _pvList.Skip(_pvList.Count - 4).Select(s => s.Frame).ToArray();
            midAmp  = MidlineAmp(pvGroup);

            _pvPairs.Add(new PvPair(pvGroup, midAmp));

            //Increment the period if the frequency threshold is met
            if (_pvPairs.Count > 1)
            {
                PvPair prevpvp = _pvPairs[_pvPairs.Count - 2];
                int frequency = FrequencyBetween(prevpvp, _pvPairs.Last());

                if (frequency <= MinFrequency || frequency >= MaxFrequency)
                    return false;

                _periods.Add(_pvPairs.Last().Frames.Item1);
                if (_periods.Count >= PeriodThreshold)
                {
                    return true;
                }
            }

            return false;
        }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// Calculates the mid line and amplitude of a given set of valley and peaks
        /// </summary>
        /// <param name="pvGroup">
        /// Group of indexes that form the peak and valley of the given wave
        /// </param>
        /// <returns>
        /// Tuple containing 
        /// Item1: mid line between peak and valley
        /// Item2: amplitude between peak and valley
        /// </returns>
        private Tuple<double, double> MidlineAmp(IReadOnlyList<int> pvGroup )
        {
            int p1                = pvGroup[0] % WindowSize;
            int p2                = pvGroup[1] % WindowSize;
            int p3                = pvGroup[2] % WindowSize;

            double outsideAverage = (_wave[p1] + _wave[p3]) / 2;
            double midline        = (_wave[p2] + outsideAverage) / 2;
            double amplitude      = Math.Abs(_wave[p2] - midline);

            return new Tuple<double, double>(midline, amplitude);
        }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// Calculate the frequency between two PVPairs.
        /// </summary>
        /// <param name="pvp1"> First PVPair whose midline is being used. </param>
        /// <param name="pvp2"> Second PVPair. </param>
        /// <returns> The number of frames comprising a period of the wave. </returns>
        private int FrequencyBetween(PvPair pvp1, PvPair pvp2)
        {
            int index          = pvp1.Frames.Item1; // Get start Frame for calculation.
            bool passedMidLine = false;
            int start          = 0;
            int end            = 0;

            // 1 for VPV and 0 for PVP.
            bool pvpattern     = _wave[pvp1.Frames.Item1 % WindowSize] < 
                                 _wave[pvp1.Frames.Item2 % WindowSize];

            while (passedMidLine == false)
            {
                int i = index++ % WindowSize;
                int j = index % WindowSize;
                passedMidLine =
                    pvpattern ? (pvp1.Midline > _wave[i] && pvp1.Midline < _wave[j])
                              : (pvp1.Midline < _wave[i] && pvp1.Midline > _wave[j]);
            }
            start = index;

            // Get end Frame for frequency calculation.
            index = pvp2.Frames.Item1;
            passedMidLine = false;
            while (passedMidLine == false)
            {
                int i = index++ % WindowSize;
                int j = index % WindowSize;
                passedMidLine =
                    pvpattern ? (pvp1.Midline > _wave[i] && pvp1.Midline < _wave[j])
                              : (pvp1.Midline < _wave[i] && pvp1.Midline > _wave[j]);
            }
            end = index;

            return end - start;
        }

        //================================================================================
        /// <summary>
        /// Class for objects representing either peaks or valleys.
        /// Data members are the Frame identifying number and the progress value.
        /// </summary>
        private class PeakValley
        {
            public int Frame { get; }
            public double Value { get; }

            //----------------------------------------------------------------------------
            public PeakValley(int frame, double value)
            {
                Frame = frame;
                Value = value;
            }
        }

        //================================================================================
        /// <summary>
        /// Class for objects representing data points of the form VPV or PVP.
        /// Data members are the three Frame numbers, the midline, and the amplitude
        /// </summary>
        private class PvPair
        {
            // Item1, 2, and 3 correspond to the frames of the PVPair.
            public Tuple<int, int, int> Frames { get; }
            public double Midline { get; }
            public double Amplitude { get; }

            //----------------------------------------------------------------------------
            public PvPair(Tuple<int, int, int> frames, double midline, double amplitude)
            {
                Frames    = new Tuple<int, int, int>(frames.Item1, frames.Item2, 
                                                     frames.Item3);
                Midline   = midline;
                Amplitude = amplitude;
            }

            //----------------------------------------------------------------------------
            public PvPair(IReadOnlyList<int> frames, Tuple<double, double> midAmp)
            {
                Frames    = new Tuple<int, int, int>(frames[0],frames[1],frames[2]);
                Midline   = midAmp.Item1;
                Amplitude = midAmp.Item2;
            }
        }
    }
}
