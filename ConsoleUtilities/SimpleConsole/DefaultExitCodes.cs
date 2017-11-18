// Copyright (c) Anton Vasiliev. All rights reserved.
// Licensed under the MIT license.
// See the License.md file in the project root for full license information.

namespace Silvers.ConsoleUtilities.SimpleConsole
{
    /// <summary>
    /// Holds default exit codes for <see cref="SimpleConsoleUtility"/>.
    /// </summary>
    public static class DefaultExitCodes
    {
        /// <summary>
        /// This value indicates that execution is completed succesfully.
        /// </summary>
        public const int Success = 0;

        /// <summary>
        /// This value indicates that execution is finished prematurely.\n
        /// It is used in cases when user code was not called before application termination.
        /// </summary>
        public const int UnexpectedExit = 1;

        /// <summary>
        /// This value indicates that command line can't be unambiguously parsed to switches
        /// and parameters.
        /// </summary>
        public const int CommandLineParseError = 2;

        /// <summary>
        /// This value indicates that error occured while opening output file.
        /// </summary>
        public const int CreateOutputFileError = 3;

        /// <summary>
        /// This value indicates that an unhandled exception was thrown during execution.
        /// </summary>
        public const int UnhandledException = 4;
    }
}
