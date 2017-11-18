// Copyright (c) Anton Vasiliev. All rights reserved.
// Licensed under the MIT license.
// See the License.md file in the project root for full license information.

namespace Silvers.ConsoleUtilities
{
    using System;
    using System.Reflection;

    /// <summary>
    /// Holds information about application and environment. Information is organized in a way
    /// to simplify common tasks for console utility application.
    /// </summary>
    public class ConsoleUtilityContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleUtilityContext"/> class.\n
        /// Application name and version is loaded from assembly information. Output stream is set
        /// to console.
        /// </summary>
        public ConsoleUtilityContext()
        {
            AssemblyName assemblyName = Assembly.GetEntryAssembly().GetName();
            this.Name = assemblyName.Name;
            this.Version = assemblyName.Version;

            this.Output = new StreamOutput();
        }

        /// <summary>
        /// Gets application name. Loaded from Entry Assembly on class creation.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets application version. Loaded from Entry Assembly on class creation.
        /// </summary>
        public Version Version { get; private set; }

        /// <summary>
        /// Gets output stream. Set to console by defauls, can be set to file or another stream.
        /// </summary>
        public StreamOutput Output { get; private set; }

        /// <summary>
        /// Gets known command line switches.
        /// </summary>
        public CommandLineSwitchSet Switches { get; private set; }

        /// <summary>
        /// Gets parsed command line arguments.
        /// </summary>
        public CommandLineArguments Arguments { get; private set; }

        /// <summary>
        /// Gets a value indicating whether console utility should output only errors
        /// and execution results.
        /// </summary>
        public bool Silent { get; private set; }

        /// <summary>
        /// Gets a value indicating whether console utility should output additional information
        /// about execution process.
        /// </summary>
        public bool Verbose { get; private set; }

        /// <summary>
        /// Parse command line with provided set of command line switches.
        /// </summary>
        /// <param name="switches">Collection of known command line switches.</param>
        /// <param name="arguments">Command line arguments. The name of the program should be
        /// omitted from the array of arguments.</param>
        public void ParseCommandLine(CommandLineSwitchSet switches, string[] arguments)
        {
            if (this.Arguments != null)
            {
                throw new InvalidOperationException("Command line arguments are already parsed.");
            }

            if (switches == null)
            {
                throw new ArgumentNullException(nameof(switches));
            }

            if (arguments == null)
            {
                throw new ArgumentNullException(nameof(arguments));
            }

            this.Switches = switches;

            this.Arguments = new CommandLineArguments(switches, arguments);
        }

        /// <summary>
        /// Sets standard output options according to specified command line switches.
        /// </summary>
        /// <param name="silent"><see cref="CommandLineSwitch"/> that enables Silent option.\n
        /// Switch must have no parameters. If switch is found in parsed set of command line
        /// arguments, then Silent option is set to True. If switch is not found or object is null
        /// then Silent option will not change.</param>
        /// <param name="verbose"><see cref="CommandLineSwitch"/> that enables Verbose option.\n
        /// Switch must have no parameters. If switch is found in parsed set of command line
        /// arguments, then Verbose option is set to True. If switch is not found or object is null
        /// then Verbose option will not change.</param>
        public void SetupVerboseLevel(CommandLineSwitch silent, CommandLineSwitch verbose)
        {
            if (this.Arguments == null)
            {
                throw new InvalidOperationException("Command line arguments are not parsed yet.");
            }

            if (silent != null)
            {
                if (silent.ParametersCount != 0)
                {
                    throw new ArgumentException("Switch for option 'silent' must have no " +
                        "parameters.");
                }

                this.Silent = this.Arguments.GetArgument(silent).isSpecified;
            }

            if (verbose != null)
            {
                if (verbose.ParametersCount != 0)
                {
                    throw new ArgumentException("Switch for option 'verbose' must have no " +
                        "parameters.");
                }

                this.Verbose = !this.Silent && this.Arguments.GetArgument(verbose).isSpecified;
            }
        }

        /// <summary>
        /// Sets output target for <see cref="StreamOutput"/> to specified file.
        /// </summary>
        /// <param name="output"><see cref="CommandLineSwitch"/> that sets output to specified
        /// file.\n Switch must have one parameter. If switch is found in parsed set of command line
        /// arguments, then output stream is set to specified file.</param>
        public void SetupOutput(CommandLineSwitch output)
        {
            if (this.Arguments == null)
            {
                throw new InvalidOperationException("Command line arguments are not parsed yet.");
            }

            if (output == null)
            {
                throw new ArgumentNullException(nameof(output));
            }

            if (output.ParametersCount != 1)
            {
                throw new ArgumentException("Switch for option 'output' must have exactly " +
                    "one parameter.");
            }

            var outputArgument = this.Arguments.GetArgument(output);
            if (outputArgument.isSpecified)
            {
                this.Output.SetToFile(outputArgument.parameters[0]);
            }
        }
    }
}
