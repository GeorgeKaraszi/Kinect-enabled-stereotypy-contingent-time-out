using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTime
{
    class Program
    {
        static void Main(string[] args)
        {
            GestureInterpreter classifier = new GestureInterpreter();
            
            try
            {
                using (StreamReader sr = new StreamReader("MichaelFB3217.txt"))
                {
                    String str;

                    while ((str = sr.ReadLine()) != null)
                    {
                        double val;
                        try
                        {
                            // Convert string into double and add it to wave.
                            val = Convert.ToDouble(str);

                            // Process point.
                            bool returnVal = classifier.ProcessPoint(val);

                            if (returnVal)
                            {
                                System.Console.WriteLine("HAND FLAPPING DETECTED");
                            }
                        }
                        catch (FormatException)
                        {
                            Console.WriteLine("Unable to convert \"{0}\" to a double.", str);
                        }
                        catch (OverflowException)
                        {
                            Console.WriteLine("\"{0}\" is outside the range of a double.", str);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read: ");
                Console.WriteLine(e.Message);
            }

            Console.ReadKey();
        }

        
    }

    public class GestureInterpreter
    {
        // Circular array implementation of wave buffer.
        private double[] wave;

        // List of peaks and valleys.
        LinkedList<PeakValley> PVList;
        // List of PVPairs (VPV or PVP) for calculating frequency.
        LinkedList<PVPair> PVPairs;
        // List of periods. It's a list so it can be decremented as PVPairs are removed.
        LinkedList<int> periods;

        // Size of wave buffer to analyze.
        private const int window_size = 60;

        // Minimum and maximum frequency.
        private const int min_frequency = 5;
        private const int max_frequency = 26;

        // Minimum distance between peaks and valleys.
        private const double min_peak_valley_distance = 0.15;
        
        // Number of frames for counting.
        int frame;

        // Peak and valley data initialization.
        double peak;
        double valley;
        // Frames at which the peak and valley is set.
        int peakset;
        int valset;
        
        public GestureInterpreter()
        {
            wave = new double[window_size];
            PVList = new LinkedList<PeakValley>();
            PVPairs = new LinkedList<PVPair>();
            periods = new LinkedList<int>();

            frame = -1;

            peak = 0;
            valley = 1;

            peakset = -1;
            valset = -1;
        }

        // Process input and return if hand flap has been detected.
        public bool ProcessPoint(double val)
        {
            bool returnVal = false;

            frame++;

            // First, check if there is to be a PV removed from the list.
            if (PVList.Count > 0 && PVList.First().frame <= frame - window_size)
            {
                System.Console.WriteLine("Removing PV {0}, {1}...", PVList.First().frame, PVList.First().value);
                PVList.RemoveFirst();
            }

            // And if there is a PVPair to be removed from the list.
            if (PVPairs.Count > 0 && PVPairs.First().frames.Item1 <= frame - window_size)
            {
                PVPair pvp = PVPairs.First();
                System.Console.WriteLine("Removing PVPair {0}, {1}, {2}...", pvp.frames.Item1, pvp.frames.Item2, pvp.frames.Item3);
                // If the removal of this PVPair entitles the removal of a period.
                if (periods.Count > 0 && pvp.frames.Item1 == periods.First())
                {
                    periods.RemoveFirst();
                    System.Console.WriteLine("Period is now {0}.", periods.Count);
                }
                PVPairs.RemoveFirst();
            }

            int index = frame % window_size;
            wave[index] = val;
            //System.Console.WriteLine("wave[{0}] = {1}.", index, val);

            // If the point is between peak and valley
            if (wave[index] <= peak && wave[index] >= valley)
            {
                if (peak - valley < min_peak_valley_distance)
                {
                    return false;
                }
                // If valset is oldest, save valley to list, reset valley, and continue.
                if (valset < peakset)
                {
                    //Be sure the valley is less then the last captured peak.
                    if (PVList.Count > 0 && valley >= PVList.Last().value)
                    {
                        valset = frame;
                        valley = wave[index];
                        return false;
                    }

                    // If Adding a PeakValley triggers hand-flapping detection, return true.
                    returnVal = this.AddPV(new PeakValley(valset, valley));

                    valley = 1;
                    return returnVal;
                }

                // If peakset is oldest, save peak to list, reset peak, and continue.
                if (peakset < valset)
                {
                    //Be sure the peak is higher then the last captured valley
                    if (PVList.Count > 0 && peak <= PVList.Last().value)
                    {
                        peakset = frame;
                        peak = wave[index];
                        return false;
                    }

                    // If Adding a PeakValley triggers hand-flapping detection, return true.
                    returnVal = this.AddPV(new PeakValley(peakset, peak));

                    peak = 0;
                    return returnVal;
                }
            }
            if (wave[index] > peak)
            {
                peakset = frame;
                peak = wave[index];
            }
            if (wave[index] < valley)
            {
                valset = frame;
                valley = wave[index];
            }

            return returnVal;
        }

        // Add a PeakValley to the list.
        private bool AddPV(PeakValley pv)
        {
            PVList.AddLast(pv);
            Console.WriteLine("Added new PV: {0}, {1}.", pv.frame, pv.value);

            // If there are at least three points and the last point is not the third point in the last PVPair
            // (if there is one), then create a PVPair from these three points.
            if (PVList.Count > 2)
            {
                if (PVPairs.Count > 0 && PVPairs.Last().frames.Item3 == PVList.ElementAt<PeakValley>(PVList.Count - 2).frame)
                {
                    return false;
                }

                int n = PVList.Count;
                int p1 = PVList.ElementAt<PeakValley>(n - 3).frame;
                int p2 = PVList.ElementAt<PeakValley>(n - 2).frame;
                int p3 = PVList.ElementAt<PeakValley>(n - 1).frame;
                double outsideAverage = (wave[p1 % window_size] + wave[p3 % window_size]) / 2;
                double midline = (wave[p2 % window_size] + outsideAverage) / 2;
                double amplitude = Math.Abs(wave[p2 % window_size] - midline);

                PVPairs.AddLast(new PVPair(new Tuple<int, int, int>(p1, p2, p3), midline, amplitude));
                Console.WriteLine("Created a new PVPair, {0}, {1}, {2} with midline {3} and amplitude {4}.", p1, p2, p3, midline, amplitude);

                // Now, if there is a previous PVPair, get the frequency between the two PVPairs.
                // If the frequency is greater than the threshold minimum and less than the
                // threshold maximum, increment the period.
                if (PVPairs.Count > 1)
                {
                    PVPair prevpvp = PVPairs.ElementAt<PVPair>(PVPairs.Count - 2);
                    int frequency = this.FrequencyBetween(prevpvp, PVPairs.Last());
                    if (frequency > min_frequency && frequency < max_frequency)
                    {
                        periods.AddLast(PVPairs.Last().frames.Item1);
                        System.Console.WriteLine("Frequency = {0} and Period = {1}.", frequency, periods.Count);
                        if (periods.Count >= 3)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        // Calculate the frequency between two PVPairs.
        private int FrequencyBetween(PVPair pvp1, PVPair pvp2)
        {
            int index = pvp1.frames.Item1;
            int start = -1;
            int end = -1;
            // 1 for VPV and 0 for PVP
            bool pvpattern = wave[pvp1.frames.Item1 % window_size] < wave[pvp1.frames.Item2 % window_size];
            bool passedMidLine = false;

            while (passedMidLine == false)
            {
                int i = index % window_size;
                int j = (index + 1) % window_size;
                passedMidLine =
                    pvpattern ? (pvp1.midline > wave[i] && pvp1.midline < wave[j] ? true : false)
                              : (pvp1.midline < wave[i] && pvp1.midline > wave[j] ? true : false);

                index++;
            }

            start = index;
            passedMidLine = false;
            index = pvp2.frames.Item1;
            while (passedMidLine == false)
            {
                int i = index % window_size;
                int j = (index + 1) % window_size;
                passedMidLine =
                    pvpattern ? (pvp1.midline > wave[i] && pvp1.midline < wave[j] ? true : false)
                              : (pvp1.midline < wave[i] && pvp1.midline > wave[j] ? true : false);

                index++;
            }
            end = index;

            return end - start;
        }
    }

    public class PeakValley
    {
        public int frame { get; }
        public double value { get; }

        public PeakValley(int frame, double value)
        {
            this.frame = frame;
            this.value = value;
        }
    }

    public class PVPair
    {
        // Item1, 2, and 3 correspond to the frames of the PVPair.
        public Tuple<int, int, int> frames { get; }
        public double midline { get; }
        public double amplitude { get; }

        public PVPair(Tuple<int, int, int> frames, double midline, double amplitude)
        {
            this.frames = new Tuple<int, int, int>(frames.Item1, frames.Item2, frames.Item3);
            this.midline = midline;
            this.amplitude = amplitude;
        }
    }
}
