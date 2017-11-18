// Copyright (c) Anton Vasiliev. All rights reserved.
// Licensed under the MIT license.
// See the License.md file in the project root for full license information.

namespace Silvers.ConsoleUtilities.SimpleConsole
{
    using Silvers.ConsoleUtilities;

    /// <summary>
    /// Holds default <see cref="CommandLineSwitch">command line switches</see> for
    /// <see cref="SimpleConsoleUtility"/>.
    /// </summary>
    public static class DefaultSwitches
    {
        /// <summary>
        /// Default declaration of silent output opttion <see cref="CommandLineSwitch"/>.
        /// </summary>
        public static readonly CommandLineSwitch Silent = new CommandLineSwitch(
            "silent", null, 0, "Output only errors and execution results.");

        /// <summary>
        /// Default declaration of verbose output option <see cref="CommandLineSwitch"/>.
        /// </summary>
        public static readonly CommandLineSwitch Verbose = new CommandLineSwitch(
            "verbose", null, 0, "Output additional information.");

        /// <summary>
        /// Default declaration of output file option <see cref="CommandLineSwitch"/>.
        /// </summary>
        public static readonly CommandLineSwitch Output = new CommandLineSwitch(
            "output", null, 1, "Redirect output into file. File contents will be erased.");

        /// <summary>
        /// Default declaration of show usage information <see cref="CommandLineSwitch"/>.
        /// </summary>
        public static readonly CommandLineSwitch Help = new CommandLineSwitch(
            "help", null, 0, "Show usage instructions.");

        /// <summary>
        /// <see cref="CommandLineSwitchSet"/> containing all default
        /// <see cref="CommandLineSwitch">command line switches</see>.
        /// </summary>
        public static readonly CommandLineSwitchSet Collection =
            new CommandLineSwitchSet(
                new CommandLineSwitch[] { Silent, Verbose, Output, Help });
    }
}
