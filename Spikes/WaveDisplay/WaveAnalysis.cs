using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaveDisplay
{
    class WaveAnalysis
    {
        public WaveAnalysis() { }

        private Tuple<int, double> CheckAndCenter(List<double> wave, int index)
        {
            int i = 0;
            int indexset = 0;
            int range = 0;
            int midpoint = 0;
            while (++i < 4)
            {
                indexset = index + i;
                if (indexset + 1 < wave.Count)
                {
                    if (Math.Abs(wave[indexset] - wave[indexset + 1]) < Double.Epsilon)
                    {
                        break;
                    }
                }

                indexset = -1;
            }

            if (indexset != -1)
            {
                range = indexset;

                while (Math.Abs(wave[indexset] - wave[range]) < Double.Epsilon)
                    range++;

                range -= 1;
                midpoint = range - (int) (Math.Abs(range - indexset)/2);

                if (Math.Abs(range - indexset) >= 4)
                {
                    return new Tuple<int, double>(midpoint, wave[midpoint]);
                }
            }

            return null;
        }

        public Tuple<int, double> []FindPeaksAndValleys(List<double> wave)
        {
            List<Tuple<int, double>> pvList = new List<Tuple<int, double>>();
            double peak = 0;
            double valley = 1;
            int peakset = -1;
            int valset = -1;

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
                        if (pvList.Count > 0)
                        {
                            //Be sure the valley is less then the last captured peak
                            if (pvList.Last().Item2 < valley)
                            {
                                continue;
                            }
                        }

                        var tempset = CheckAndCenter(wave, valset);
                        if (tempset != null)
                        {
                            pvList.Add(tempset);
                        }
                        else
                            pvList.Add(new Tuple<int, double>(valset, valley));

                        valley = 1;
                        i--;

                        continue;
                    }
                    // If peakset is oldest, save peak to list, reset peak, and continue.
                    if (peakset < valset)
                    {
                        if (pvList.Count > 0)
                        {
                            //Be sure the peak is higher then the last captured valley
                            if (pvList.Last().Item2 > peak)
                            { 
                                continue;
                            }
                        }

                        var tempset = CheckAndCenter(wave, peakset);
                        if (tempset != null)
                        {
                            pvList.Add(tempset);
                        }
                        else
                        {
                            pvList.Add(new Tuple<int, double>(peakset, peak));
                        }

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


        public List<double> SmoothPeaksAndValleys(List<double> wave)
        {
            var pv = FindPeaksAndValleys(wave);
            var modifiedWave = new List<double>(wave);
            var betweenPeaks = new List<Tuple<int, int>>();

            //Collect all peaks from the Peaks and Valley list. 
            // The order should be P[i] V(i+1) P(i+2)
            for (int i = 0; i < pv.Count(); i += 2)
            {
                if (i + 2 < pv.Count())
                {
                    betweenPeaks.Add(new Tuple<int, int>(pv[i].Item1, pv[i+2].Item1));
                }
                else if (i + 1 <= pv.Count())
                {
                    betweenPeaks.Add(new Tuple<int, int>(pv[i].Item1, -1));
                }
            }

            
            foreach (var bpeaks in betweenPeaks)
            {
                int range = 0;

                if (bpeaks.Item2 == -1)
                    break;
                    //range = Math.Abs(bpeaks.Item1 - wave.Count);
                else
                {
                    range = Math.Abs(bpeaks.Item1 - bpeaks.Item2);
                }

                var focusgroup = new List<double>(range);

                for (int i = 0; i <= range; i++)
                {
                    focusgroup.Add(modifiedWave[bpeaks.Item1 + i]);
                }

                var tweakedWave = TweakWaveDeviation(focusgroup);

                for (int i = 0; i <= range; i++)
                {
                    modifiedWave[i + bpeaks.Item1] = tweakedWave[i];
                }

            }


            return modifiedWave;
        }

        private List<double> TweakWaveDeviation(List<double> wave)
        {
            WaveUtilities wu = new WaveUtilities();

            var sd = wu.CaculateStdDev(wave);
            //0.33
            //0.35
            //0.11 bad
            //0.14 bad
            //0.259 goodish
            //0.12 bad

            if (sd < 0.15)
            {
                var maxPeak = wave.Max();
                var lowValley = wave.Min();
                var avg = wave.Average();
                var midPoint = (int) Math.Round((double)wave.Count/2);
                var incrementBy = maxPeak/(midPoint+1);
                double incrmenter = incrementBy;

                if (wave[0] > wave[wave.Count-1])
                    incrmenter = wave[0];
                else
                {
                    incrmenter = wave[wave.Count-1];
                }

                for (int i = 0; i < wave.Count; i++)
                {
                    wave[i] = incrmenter;
                }

            }

            return wave;
            
        }

    }
}
