using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Accord.Statistics.Distributions.Fitting;
using Accord.Statistics.Distributions.Multivariate;
using Accord.Statistics.Models.Fields;
using Accord.Statistics.Models.Fields.Functions;
using Accord.Statistics.Models.Fields.Learning;
using Accord.Statistics.Models.Markov;
using Accord.Statistics.Models.Markov.Learning;
using Accord.Statistics.Models.Markov.Topology;

namespace Gestures.HMMs
{
    public class Hmm
    {
        /// <summary>
        /// Store's all the classifiers for pattern recognition
        /// </summary>
        public Database CLASSIFYDB { get; }

        public int HmmMatches { get; set; }
        public int HcrfMatches { get; set; }
        public int HmmGoodMatches { get; set; }
        public int HcrfGoodMatches { get; set; }
        public int HcrfBadMatches { get; set; }
        public int HmmBadMatches { get; set; }

        /// <summary>
        /// Markov Model using Normal distribution learning tool
        /// </summary>
        private HiddenMarkovClassifier<MultivariateNormalDistribution> _hmm;

        /// <summary>
        /// Hidden Conditional Random Field
        /// </summary>
        private HiddenConditionalRandomField<double[]> _hcrf;

        //--------------------------------------------------------------------------------
        public Hmm()
        {
            CLASSIFYDB      = new Database(); //Initialize database of samples.
            _hmm            = null;
            _hcrf           = null;
            HmmMatches      = 0;
            HcrfMatches     = 0;
            HmmGoodMatches  = 0;
            HcrfGoodMatches = 0;
            HcrfBadMatches  = 0;
            HmmBadMatches   = 0;
        }

        /// <summary>
        /// Clear the learning elements
        /// </summary>
        public void Clear()
        {
            _hmm  = null;
            _hcrf = null;
        }

        public void LoadHcrf(Object hcrf)
        {
            _hcrf = (HiddenConditionalRandomField < double[] >)hcrf;
        }

        public HiddenConditionalRandomField<double[]> GetHcrf() { return _hcrf; }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// Check to see if HMM can process the database
        /// </summary>
        /// <returns>True if it can now learn</returns>
        public bool CanLearn()
        {
            if (CLASSIFYDB.Classes.Count >= 2 && CLASSIFYDB.SamplesPerClass() >= 3)
                return true;

            return false;
        }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// Converts data set of X,Y to Point type
        /// </summary>
        /// <param name="dp">DataPoint to convert</param>
        /// <returns>Point of X Y from DataPoint</returns>
        private Point ConvertToPoint(DataPoint dp)
        {
            return new Point(Convert.ToInt32(dp.XValue),
                             Convert.ToInt32(dp.YValues[0]));
        }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// Converts array of DataPoints to Point type
        /// </summary>
        /// <param name="dp">DataPoint to convert</param>
        /// <returns>Array of points from DataPoint</returns>
        private Point[] ConvertToPoint(DataPoint[] dp)
        {
            Point[] points = new Point[dp.Length];

            for (int i = 0; i < dp.Length; i++)
            {
                points[i] = ConvertToPoint(dp[i]);
            }

            return points;
        }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// Converts array of DataPoints to Point type
        /// </summary>
        /// <param name="dp">DataPoint to convert</param>
        /// <returns>Array of points from DataPoint</returns>
        private Point[] ConvertToPoint(DataPointCollection dp)
        {
            Point[] points = new Point[dp.Count];

            for (int i = 0; i < dp.Count; i++)
            {
                points[i] = ConvertToPoint(dp[i]);
            }

            return points;
        }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// Converts input to Point structure.
        /// **INFO**This does a simple Convert to Int, with no care past the decimal.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private Point ConvertToPoint(int x, int y)
        {
            return new Point(x, y);
        }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// Converts input to Point structure.
        /// **INFO**This does a simple Convert to Int, with no care past the decimal.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private Point ConvertToPoint(double x, double y)
        {
            return new Point(Convert.ToInt32(x),
                             Convert.ToInt32(y));
        }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// Converts input to Point structure.
        /// **INFO**This does a simple Convert to Int, with no care past the decimal.
        /// </summary>
        /// <param name="y"></param>
        /// <returns></returns>
        private Point[] ConvertToPoint(double[] y)
        {
            Point[] points = new Point[y.Length];

            for (int i = 0; i < y.Length; i++)
            {
                points[i] = new Point(i, Convert.ToInt32(y[i]));
            }

            return points;
        }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// Add's a pattern to the database for better recognition
        /// </summary>
        /// <param name="dataPoints">Data points the recorded pattern made</param>
        /// <param name="label">Classifier for the pattern</param>
        public bool AddPattern(DataPoint[] dataPoints, String label)
        {
            if (dataPoints.Length <= 0)
                return false;

            //Convert to Point(X,Y) Int's to be handled by the HMM later
            //There is no float or double handling
            Point[] points = ConvertToPoint(dataPoints);

            return CLASSIFYDB.Add(points, label) != null;
        }


        //--------------------------------------------------------------------------------
        /// <summary>
        /// Add's a pattern to the database for better recognition
        /// </summary>
        /// <param name="dataPoints">Data points the recorded pattern made</param>
        /// <param name="label">Classifier for the pattern</param>
        public bool AddPattern(DataPointCollection dataPoints, String label)
        {
            if (dataPoints.Count <= 0)
                return false;

            //Convert to Point(X,Y) Int's to be handled by the HMM later
            //There is no float or double handling
            Point[] points = ConvertToPoint(dataPoints);

            return CLASSIFYDB.Add(points, label) != null;
        }

        //--------------------------------------------------------------------------------

        public bool LearnHmm()
        {

            BindingList<Sequence> samples = CLASSIFYDB.Samples;
            BindingList<String> classes   = CLASSIFYDB.Classes;
            double[][][] inputs           = new double[samples.Count][][];
            int[] outputs                 = new int[samples.Count];
            int states                    = 10;
            int iterations                = 200;
            double tolerance              = 1E-5;
            bool rejection                = false;
            _hmm                          = new HiddenMarkovClassifier
                                                <MultivariateNormalDistribution>(
                                                classes.Count,
                                                new Forward(states),
                                                new MultivariateNormalDistribution(2),
                                                classes.ToArray());

            for (int i = 0; i < inputs.Length; i++)
            {
                inputs[i]  = samples[i].Input;
                outputs[i] = samples[i].Output;
            }

            // Create the learning algorithm for the ensemble classifier
            var teacher =
                new HiddenMarkovClassifierLearning<MultivariateNormalDistribution>(
                    _hmm,i => // Train each model using the selected convergence criteria
                        new BaumWelchLearning<MultivariateNormalDistribution>(
                        _hmm.Models[i])
                        {
                            Tolerance = tolerance,
                            Iterations = iterations,

                            FittingOptions = new NormalOptions()
                                             {
                                Regularization = 1e-5
                                             }
                        }
                    );

            teacher.Empirical = true;
            teacher.Rejection = rejection;


            // Run the learning algorithm
            double error = teacher.Run(inputs, outputs);


            HmmMatches = 0;
            HmmBadMatches = 0;
            HmmGoodMatches = 0;

            // Classify all training instances
            foreach (var sample in CLASSIFYDB.Samples)
            {
                sample.RecognizedAs = _hmm.Compute(sample.Input);
                if (sample.RecognizedAs == sample.Output)
                {
                    HmmMatches++;
                    if (sample.RecognizedAs == 0)
                        HmmGoodMatches++;
                    else
                    {
                        HmmBadMatches++;
                    }
                }
            }

            return true;
        }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// After the initial learning from the hidden Markov model. The machine learning
        ///  aspect can be furthered improved with more samples to learn from.
        /// </summary>
        /// <returns>True if everything went correctly</returns>
        public bool LearnHcrf()
        {
            //Verify that the Markov model is good
            if (_hmm == null)
                return false;

            var samples         = CLASSIFYDB.Samples;
            var classes         = CLASSIFYDB.Classes;
            double[][][] inputs = new double[samples.Count][][];
            int[] outputs       = new int[samples.Count];
            int iterations      = 1500;
            double tolerance    = 1E-7;
            _hcrf               = new HiddenConditionalRandomField<double[]>(
                                     new MarkovMultivariateFunction(_hmm));

            for (int i = 0; i < inputs.Length; i++)
            {
                inputs[i]       = samples[i].Input;
                outputs[i]      = samples[i].Output;
            }

            // Create the learning algorithm for the ensemble classifier
            var teacher = new HiddenResilientGradientLearning<double[]>(_hcrf)
                          {
                              Iterations = iterations,
                              Tolerance = tolerance
                          };


            // Run the learning algorithm
            teacher.Run(inputs, outputs);

            HcrfMatches     = 0;
            HcrfBadMatches  = 0;
            HcrfGoodMatches = 0;
            foreach (var sample in CLASSIFYDB.Samples)
            {
                sample.RecognizedAs = _hcrf.Compute(sample.Input);

                if (sample.RecognizedAs == sample.Output)
                {
                    HcrfMatches++;
                    if (sample.RecognizedAs == 0)
                    {
                        HcrfGoodMatches++;
                    }
                    else
                    {
                        HcrfBadMatches++;
                    }
                }
            }

            return true;
        }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// Makes the final prediction on the data it has receive 
        /// </summary>
        /// <param name="dataPoints">Data points to check</param>
        /// <returns></returns>
        public string ComputeResults(DataPoint[] dataPoints)
        {
            return ComputeResults(ConvertToPoint(dataPoints));
        }

        public string ComputeResults(DataPointCollection dataPoints)
        {
            return ComputeResults(ConvertToPoint(dataPoints));
        }

        public string ComputeResults(double[] dataPoints)
        {
            return ComputeResults(ConvertToPoint(dataPoints));
        }

        public string ComputeResults(Point[] dataPoints)
        {
            double[][] input = Sequence.Preprocess(dataPoints);
            int index        = -1;

            if (input.Length < 5)
            {
                return null;
            }

            if (_hmm == null && _hcrf == null)
            {
                return String.Empty;
            }

            if (_hcrf != null)
            {
                index = _hcrf.Compute(input);
            }
            else if (_hmm != null)
            {
                index = _hmm.Compute(input);
            }
            else
            {
                return String.Empty;
            }

            return CLASSIFYDB.Classes[index];
        }
    }
}