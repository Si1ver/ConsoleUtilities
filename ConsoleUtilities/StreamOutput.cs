// Copyright (c) Anton Vasiliev. All rights reserved.
// Licensed under the MIT license.
// See the License.md file in the project root for full license information.

namespace Silvers.ConsoleUtilities
{
    using System;
    using System.IO;
    using System.Text;

    /// <summary>
    /// Simple wrapper for StreamWriter. Allows to set output stream to console, file or
    /// specified StreamWriter.
    /// </summary>
    public class StreamOutput
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StreamOutput"/> class and sets outpus
        /// stream to console.
        /// </summary>
        public StreamOutput() => this.SetToConsole();

        /// <summary>
        /// Gets output stream. Always not null. Set to console by default.
        /// </summary>
        public StreamWriter Stream { get; private set; }

        /// <summary>
        /// Set output stream to console (standard output).
        /// </summary>
        public void SetToConsole()
        {
            this.SetToStream(new StreamWriter(Console.OpenStandardOutput())
            {
                AutoFlush = true
            });
        }

        /// <summary>
        /// Set output stream to specified file.\n
        /// If file not exists, it willbe created. If file exists, it will be overwriten.\n
        /// If parent directory not exists, it will be created.\n
        /// If error occurs while opeing the file or creating parent directory,
        /// StreamOutputException will bw thrown and Stream object will be left unchanged.
        /// </summary>
        /// <param name="filePath">Path to file to set output stream to.</param>
        public void SetToFile(string filePath)
        {
            if (filePath == null)
            {
                throw new ArgumentNullException(nameof(filePath));
            }

            try
            {
                EnsureParentDirectoryForFile(filePath);
            }
            catch (Exception ex)
            {
                throw new StreamOutputException(
                    $"Error creating parent directory for file \"{filePath}\"", ex);
            }

            try
            {
                this.SetToStream(new StreamWriter(filePath, false, Encoding.UTF8));
            }
            catch (Exception ex)
            {
                throw new StreamOutputException($"Error opening output file \"{filePath}\"", ex);
            }
        }

        /// <summary>
        /// Set output to specified stream object.
        /// </summary>
        /// <param name="output">Stream to use as output. Can't be null.</param>
        public void SetToStream(StreamWriter output)
        {
            if (output == null)
            {
                throw new ArgumentNullException(nameof(output));
            }

            this.Close();

            this.Stream = output;
        }

        /// <summary>
        /// Creates parent directory for specified file name. If directory exists or only file name
        /// is provided, nothing is done.
        /// </summary>
        /// <param name="filePath">File path to check for parent directory.</param>
        private static void EnsureParentDirectoryForFile(string filePath)
        {
            string parentDirectory = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(parentDirectory) && !Directory.Exists(parentDirectory))
            {
                Directory.CreateDirectory(parentDirectory);
            }
        }

        /// <summary>
        /// Close current stream.
        /// </summary>
        private void Close()
        {
            if (this.Stream != null)
            {
                this.Stream.Close();
                this.Stream = null;
            }
        }
    }
}
