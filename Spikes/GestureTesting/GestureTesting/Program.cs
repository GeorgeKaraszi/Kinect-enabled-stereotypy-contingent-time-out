using GestureTraining;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestureTesting
{
    class Program
    {
        static void Main(string[] args)
        {
            GestureInterpreter classifier = new GestureInterpreter();
            // Total number of good (not faulty) tests.
            int total = 0;
            char[] delimiters = { '\n' };

            // Array of file names to test.
            String[] list_of_files;

            // List of the files that report false positives.
            List<String> false_positives;
            // List of the files that report false negatives.
            List<String> false_negatives;

            try
            {
                // Get list of test files and initialize the two error lists.
                list_of_files = Directory.GetFiles(@"..\..\corpus\");
                false_positives = new List<String>();
                false_negatives = new List<String>();
                
                foreach(String filename in list_of_files)
                {
                    try
                    {
                        using (StreamReader sr = new StreamReader(filename))
                        {
                            // Important to reset the classifier, a change not yet made to Source.
                            classifier.Reset();

                            // Read file into string and tokenize it.
                            String file = sr.ReadToEnd();
                            String[] points = file.Split(delimiters);

                            Boolean flapping = false;
                            Boolean flappingfound = false;

                            // Last point is whether flapping should be detected or not.
                            if (points == null || points.Length < 2)
                            {
                                continue;
                            }
                            int f = Convert.ToInt32(points[points.Length - 1]);
                            flapping = Convert.ToBoolean(f);

                            // Convert points to doubles and process them.
                            double[] wave = new double[points.Length - 2];
                            for (int i = 0; i < wave.Length; i++)
                            {
                                wave[i] = Convert.ToDouble(points[i]);
                                flappingfound = classifier.ProcessPoint(wave[i]);
                                if (flappingfound)
                                {
                                    break;
                                }
                            }

                            total++;

                            // If file did not register properly, record it.
                            if (flapping && !flappingfound)
                            {
                                false_negatives.Add(filename);
                            }
                            else if (!flapping && flappingfound)
                            {
                                false_positives.Add(filename);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Problem: {0}", e.Message);
                    }
                }

                Console.WriteLine("Total tests = {0}", list_of_files.Length);
                Console.WriteLine("Faulty tests = {0}", total - list_of_files.Length);
                Console.WriteLine("False positives = {0}", false_positives.Count);
                Console.WriteLine("False negatives = {0}", false_negatives.Count);

                Console.WriteLine("\nFalse positives:\n");
                foreach (String file in false_positives)
                {
                    Console.WriteLine(file);
                }

                Console.WriteLine("\nFalse negatives:\n");
                foreach (String file in false_negatives)
                {
                    Console.WriteLine(file);
                }

                double accuracy = 100 * (((double)total - false_negatives.Count - false_positives.Count) / total);
                Console.WriteLine("\nAccuracy = {0}%.", Math.Round((Decimal)accuracy, 1, MidpointRounding.AwayFromZero));
            }
            catch (Exception e)
            {
                Console.WriteLine("Problem: {0}", e.Message);
            }

            // Keep console window open.
            Console.ReadKey();
        }
    }
}
