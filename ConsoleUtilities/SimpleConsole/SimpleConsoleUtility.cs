// Copyright (c) Anton Vasiliev. All rights reserved.
// Licensed under the MIT license.
// See the License.md file in the project root for full license information.

namespace Silvers.ConsoleUtilities.SimpleConsole
{
    using System;
    using Silvers.ConsoleUtilities;

    /// <summary>
    /// Represents a console utility without logic.
    /// </summary>
    public class SimpleConsoleUtility
    {
        private ConsoleUtilityContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleConsoleUtility"/> class.
        /// </summary>
        public SimpleConsoleUtility()
        {
            this.context = new ConsoleUtilityContext();

            this.ExitCode = DefaultExitCodes.UnexpectedExit;
        }

        /// <summary>
        /// Gets application exit code.
        /// </summary>
        public int ExitCode { get; private set; }

        /// <summary>
        /// Creates a new instance of the <see cref="SimpleConsoleUtility"/> class, initializes
        /// it with provided <see cref="CommandLineSwitchSet"/> and command line arguments
        /// and executes payload function if needed.\n When payload execution is finished,
        /// Environment.Exit() is called.
        /// </summary>
        /// <param name="switches">Collection of known command line switches.</param>
        /// <param name="arguments">Command line arguments. The name of the program should be
        /// omitted from the array of arguments.</param>
        /// <param name="payload">Function that accepts <see cref="ConsoleUtilityContext"/>
        /// as an argument and returns exit code.</param>
        public static void RunAndQuit(
            CommandLineSwitchSet switches,
            string[] arguments,
            Func<ConsoleUtilityContext, int> payload)
        {
            var utility = new SimpleConsoleUtility();

            bool allowedToExecute = utility.Initialize(switches, arguments);

            if (allowedToExecute)
            {
                utility.Execute(payload);
            }

            Environment.Exit(utility.ExitCode);
        }

        /// <summary>
        /// Prepares ConsoleUtilityContext to execute payload.
        /// </summary>
        /// <param name="switches">Collection of known command line switches.</param>
        /// <param name="arguments">Command line arguments. The name of the program should be
        /// omitted from the array of arguments.</param>
        /// <returns>true if payload can be executed; otherwise, false.</returns>
        public bool Initialize(CommandLineSwitchSet switches, string[] arguments)
        {
            if (switches == null)
            {
                throw new ArgumentNullException(nameof(switches));
            }

            if (arguments == null)
            {
                throw new ArgumentNullException(nameof(arguments));
            }

            try
            {
                this.context.ParseCommandLine(switches, arguments);
            }
            catch (ArgumentsParsingException ex)
            {
                this.ExitCode = DefaultExitCodes.CommandLineParseError;
                this.OutputWelcomeMessage();
                this.context.Output.Stream.WriteLine(
                    $"Error parsing command line arguments: {ex.Message}");
                this.context.Output.Stream.WriteLine();
                this.OutputHelpMessage();
                return false;
            }

            this.context.SetupVerboseLevel(DefaultSwitches.Silent, DefaultSwitches.Verbose);

            try
            {
                this.context.SetupOutput(DefaultSwitches.Output);
            }
            catch (StreamOutputException ex)
            {
                this.ExitCode = DefaultExitCodes.CreateOutputFileError;
                this.OutputWelcomeMessage();
                this.context.Output.Stream.WriteLine($"Error opening output file: {ex.Message}");
                this.context.Output.Stream.WriteLine();
                this.OutputHelpMessage();
                return false;
            }

            this.OutputWelcomeMessage();

            if (this.context.Arguments.GetArgument(DefaultSwitches.Help).isSpecified)
            {
                this.OutputHelpMessage();
                this.ExitCode = DefaultExitCodes.Success;
                return false;
            }

            if (this.context.Verbose)
            {
                this.context.Output.Stream.WriteLine(this.context.Arguments.ToString());
            }

            return true;
        }

        /// <summary>
        /// Executes payload function and try to call onTerminate handler in case of parent process
        /// termination.
        /// </summary>
        /// <param name="payload">Function that accepts <see cref="ConsoleUtilityContext"/>
        /// as an argument and returns exit code.</param>
        /// <param name="onTerminate">EventHandler that should be called in case of parent process
        /// termination.</param>
        public void Execute(Func<ConsoleUtilityContext, int> payload, EventHandler onTerminate)
        {
            if (onTerminate == null)
            {
                throw new ArgumentNullException(nameof(onTerminate));
            }

            AppDomain.CurrentDomain.ProcessExit += onTerminate;

            try
            {
                this.Execute(payload);
            }
            finally
            {
                AppDomain.CurrentDomain.ProcessExit -= onTerminate;
            }
        }

        /// <summary>
        /// Executes payload function.
        /// </summary>
        /// <param name="payload">Function that accepts <see cref="ConsoleUtilityContext"/>
        /// as an argument and returns exit code.</param>
        public void Execute(Func<ConsoleUtilityContext, int> payload)
        {
            if (payload == null)
            {
                throw new ArgumentNullException(nameof(payload));
            }

            try
            {
                this.ExitCode = payload.Invoke(this.context);
            }
            catch (Exception exception)
            {
                this.ExitCode = DefaultExitCodes.UnhandledException;

                this.context.Output.Stream.WriteLine($"Unhandled exception: {exception.Message}");
            }
        }

        /// <summary>
        /// Outputs application name and version to output stream.
        /// </summary>
        private void OutputWelcomeMessage()
        {
            if (!this.context.Silent)
            {
                this.context.Output.Stream.WriteLine(
                    $"{this.context.Name} v. {this.context.Version}");
                this.context.Output.Stream.WriteLine();
            }
        }

        /// <summary>
        /// Outputs usage information to output stream.
        /// </summary>
        private void OutputHelpMessage()
        {
            if (!this.context.Silent)
            {
                this.context.Output.Stream.Write(this.context.Switches.ToString());
            }
        }
    }
}
