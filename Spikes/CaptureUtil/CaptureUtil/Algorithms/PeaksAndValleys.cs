using System;
using System.Collections.Generic;
using System.Linq;

namespace CaptureUtil.Algorithms
{
    public class PeaksAndValleys
    {
         public PeaksAndValleys() { }

        public Tuple<int, double>[] FindPeaksAndValleys(List<double> wave)
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
                    if (peak - valley < 0.15)
                    {
                        continue;
                    }
                    // If valset is oldest, save valley to list, reset valley, and continue.
                    if (valset < peakset)
                    {
                        //Be sure the valley is less then the last captured peak
                        if (pvList.Count > 0 && valley >= pvList.Last().Item2)
                        {
                            continue;
                        }


                        pvList.Add(new Tuple<int, double>(valset, valley));

                        valley = 1;
                        i--;

                        continue;
                    }

                    // If peakset is oldest, save peak to list, reset peak, and continue.
                    if (peakset < valset)
                    {
                        //Be sure the peak is higher then the last captured valley
                        if (pvList.Count > 0 && peak <= pvList.Last().Item2)
                        {
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

            return pvList.ToArray();
        }
    }
}