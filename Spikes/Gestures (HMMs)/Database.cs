// Accord.NET Sample Applications
// http://accord-framework.net
//
// Copyright © 2009-2014, César Souza
// All rights reserved. 3-BSD License:
//
//   Redistribution and use in source and binary forms, with or without
//   modification, are permitted provided that the following conditions are met:
//
//      * Redistributions of source code must retain the above copyright
//        notice, this list of conditions and the following disclaimer.
//
//      * Redistributions in binary form must reproduce the above copyright
//        notice, this list of conditions and the following disclaimer in the
//        documentation and/or other materials provided with the distribution.
//
//      * Neither the name of the Accord.NET Framework authors nor the
//        names of its contributors may be used to endorse or promote products
//        derived from this software without specific prior written permission.
// 
//  THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
//  ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
//  WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
//  DISCLAIMED. IN NO EVENT SHALL <COPYRIGHT HOLDER> BE LIABLE FOR ANY
//  DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
//  (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
//  LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
//  ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
//  (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
//  SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
// 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.IO;
using System.Xml.Serialization;
using System.Drawing;
using System.IO.Compression;
using System.Runtime.Serialization.Formatters.Binary;
using Accord.Statistics.Distributions.Multivariate;
using Accord.Statistics.Models.Fields;
using Accord.Statistics.Models.Markov;

namespace Gestures.HMMs
{
    public class Database
    {
        public BindingList<string> Classes { get; private set; }
        public BindingList<Sequence> Samples { get; private set; }

        public int Count
        {
            get { return Samples.Count; }
        }

        public Database()
        {
            Classes = new BindingList<string>();
            Samples = new BindingList<Sequence>();
        }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// Save the database from memory
        /// </summary>
        /// <param name="path">File path to database</param>
        public void Save(Stream steam, 
            HiddenConditionalRandomField<double[]> hcrf)
        {
            using (Stream tempStream = new MemoryStream())
            {
                var serializer = new XmlSerializer(typeof (BindingList<Sequence>));
                hcrf.Save(tempStream);
                serializer.Serialize(tempStream, Samples);
                CompressSave(steam, tempStream);
            }
        }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// Load database into memory
        /// </summary>
        /// <param name="path">File path with database</param>
        public object Load(Stream stream)
        {

            using (Stream streamTmp = UncompressFile(stream))
            {

                var serializer = new XmlSerializer(typeof (BindingList<Sequence>));
                object hcrf = null;//HiddenConditionalRandomField<double[]>.Load(streamTmp);
                var samples = (BindingList<Sequence>) serializer.Deserialize(streamTmp);

                Classes.Clear();
                foreach (string label in samples.First().Classes)
                    Classes.Add(label);

                Samples.Clear();
                foreach (Sequence sample in samples)
                {
                    sample.Classes = Classes;
                    Samples.Add(sample);
                }

                return hcrf;
            }
        }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// Compress the database file
        /// </summary>
        /// <param name="stream">File found</param>
        /// <param name="tmpStream">Uncompressed data to be compressed</param>
        private void CompressSave(Stream stream, Stream tmpStream)
        {
            tmpStream.Seek(0, 0);

            byte[] buffer = new byte[tmpStream.Length];
            tmpStream.Read(buffer, 0, buffer.Length);

            using (GZipStream output = new GZipStream(stream,
                                              CompressionMode.Compress))
            {
                output.Write(buffer, 0, buffer.Length);
            }
        }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// Uncompress the database file
        /// </summary>
        /// <param name="path">File found</param>
        private Stream UncompressFile(Stream stream)
        {
            MemoryStream destinationFile = new MemoryStream();

            using (Stream output = new GZipStream(stream, CompressionMode.Decompress))
            {
                output.CopyTo(destinationFile);
                destinationFile.Seek(0, 0);
            }

            // Close the files.
            return destinationFile;
        }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// Add new pattern to the database
        /// </summary>
        /// <param name="sequence"></param>
        /// <param name="classLabel"></param>
        /// <returns></returns>
        public Sequence Add(Point[] sequence, string classLabel)
        {
            if (sequence == null || sequence.Length == 0)
                return null;

            if (!Classes.Contains(classLabel))
                Classes.Add(classLabel);

            int classIndex = Classes.IndexOf(classLabel);

            Sequence sample = new Sequence()
            {
                Classes = Classes,
                SourcePath = sequence,
                Output = classIndex
            };

            Samples.Add(sample);

            return sample;
        }

        //--------------------------------------------------------------------------------
        public void Clear()
        {
            Classes.Clear();
            Samples.Clear();
        }

        //--------------------------------------------------------------------------------
        public int SamplesPerClass()
        {
            int min = 0;
            foreach (string label in Classes)
            {
                int c = Samples.Count(p => p.OutputName == label);

                if (min == 0) 
                    min = c;

                else if (c < min)
                    min = c;
            }

            return min;
        }
    }
}
