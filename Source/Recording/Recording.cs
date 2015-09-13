using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WesternMichgian.SeniorDesign.KinectProject.Recording
{
    class Recording<T>
    {
        private StreamWriter _file;                 //File stream
        private readonly string _fileName;          //File name to written data

        //--------------------------------------------------------------------------------
        /// <summary>
        /// To initialize a new file that records data.
        /// </summary>
        /// <param name="filepath">Path to the file that will be saved to</param>
        public Recording(string filepath)
        {
            _fileName = filepath;
        }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// To initialize a new file that records data.
        /// </summary>
        /// <param name="filepath">Path to the file that will be saved to</param>
        /// <param name="append">False will delete current file</param>
        public Recording(string filepath, bool append)
        {
            if (append == false)
            {
                if(File.Exists(filepath))
                    File.Delete(filepath);
            }
            _fileName = filepath;
        }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// To initialize a new file that does not exist
        /// </summary>
        /// <param name="filepath">Path to the file that will be saved to</param>
        /// <param name="newFile">If file exists, create a new one with a postfix</param>
        /// <param name="postfixRange">Range of postfix (-1 default to infinite)</param>
        public Recording(string filepath, string filetype, bool newFile, 
            int postfixRange = -1)
        {
            int postfix = 0;                        //Added postfix to newly created file
            string appendedFileName = filepath + filetype;//Filename with needed postfix

            if (newFile)                            //Do we create a file if exists?
            {
                if (File.Exists(appendedFileName))
                {
                    //Loop until a file is not found or if range has ran out
                    while(File.Exists(appendedFileName) && postfixRange != 0)
                    {
                        //Create new filename with added postfix number
                        appendedFileName = filepath + postfix++ + filetype;
                        postfixRange--;
                    }

                    //Error check if any files exist after loop
                    if (postfixRange == 0 && File.Exists(appendedFileName))
                    {
                        throw new Exception("Could not create new file " +
                                            "based on postfix range.");
                    }
                }
            }
            else if (File.Exists(appendedFileName)) //Check if file exists
            {
                File.Delete(appendedFileName);
            }

            _fileName = appendedFileName;           //Initialize new file
        }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// Adds a data value to a single line in a file
        /// </summary>
        /// <param name="value">Value object to be printed</param>
        public void Add(T value)
        {
            _file = new StreamWriter(_fileName,true);

            if (_file != null)
            {
                _file.WriteLine($"{value}");
                _file.Close();
            }
            else
            {
                throw new FileLoadException("File is null");
            }
        }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// Add a range of values to the file stream
        /// </summary>
        /// <param name="value">List of values</param>
        public void AddRange(List<T> value)
        {
            AddRange(value.ToArray());
        }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// Add a range of values to the file stream
        /// </summary>
        /// <param name="value">Array of values</param>
        public void AddRange(T[] value)
        {
            _file = new StreamWriter(_fileName, true);
            if (_file != null)
            {
                foreach (T obj in value)
                {
                    _file.WriteLine($"{obj}");
                }
                _file.Close();
            }
            else
            {
                throw new FileLoadException("File is null");
            }

        }
    }
}
