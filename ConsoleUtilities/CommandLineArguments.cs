// Copyright (c) Anton Vasiliev. All rights reserved.
// Licensed under the MIT license.
// See the License.md file in the project root for full license information.

namespace Silvers.ConsoleUtilities
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Represents a set of command line arguments parsed to switches and parameters.
    /// </summary>
    public class CommandLineArguments : IEnumerable<KeyValuePair<CommandLineSwitch, string[]>>
    {
        private Dictionary<CommandLineSwitch, string[]> arguments;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandLineArguments"/> class and parses
        /// command line arguments to set of switches and parameters.
        /// </summary>
        /// <param name="switches">Collection of known command line switches.</param>
        /// <param name="rawArguments">Command line arguments. The name of the program should be
        /// omitted from the array of arguments.</param>
        public CommandLineArguments(CommandLineSwitchSet switches, string[] rawArguments)
        {
            if (switches == null)
            {
                throw new ArgumentNullException(nameof(switches));
            }

            if (rawArguments == null)
            {
                throw new ArgumentNullException(nameof(rawArguments));
            }

            ParseResult result = Parse(switches, rawArguments);

            if (result.ParsingFailed)
            {
                throw new ArgumentsParsingException(result.ErrorText);
            }

            this.arguments = result.Arguments;
        }

        /// <summary>
        /// Returns information about the switch presence and parameters.
        /// </summary>
        /// <param name="switch">The <see cref="CommandLineSwitch"/> to look for in parsed
        /// arguments.</param>
        /// <returns>Returns a Tuple object containing two elements:\n
        /// bool isSpecified - set to True if switch is present in a set of parsed argumetns.\n
        /// string[] parameters - an array of parameters specified for the switch.</returns>
        public(bool isSpecified, string[] parameters) GetArgument(CommandLineSwitch @switch)
        {
            this.arguments.TryGetValue(@switch, out string[] parameters);
            return (parameters != null, parameters);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the <see cref="CommandLineArguments"/>.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<KeyValuePair<CommandLineSwitch, string[]>> GetEnumerator()
        {
            return this.arguments.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.arguments.GetEnumerator();
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            var builder = new StringBuilder();

            builder.AppendLine("Parsed command line arguments:");

            if (this.arguments.Count == 0)
            {
                builder.AppendLine("(none)");
            }
            else
            {
                foreach (var argument in this.arguments)
                {
                    builder.Append(argument.Key.FullPrefixedName);
                    foreach (var value in argument.Value)
                    {
                        builder.Append(" ");
                        builder.Append(value);
                    }

                    builder.AppendLine();
                }
            }

            return builder.ToString();
        }

        /// <summary>
        /// Parses command line arguments to set of switches and parameters.
        /// </summary>
        /// <param name="switches">Collection of known command line switches.</param>
        /// <param name="rawArguments">Command line arguments. The name of the program should be
        /// omitted from the array of arguments.</param>
        /// <returns>Returns <see cref="ParseResult"/> class containing parsing results.</returns>
        private static ParseResult Parse(CommandLineSwitchSet switches, string[] rawArguments)
        {
            var parsedArguments = new Dictionary<CommandLineSwitch, string[]>();

            CommandLineSwitch currentSwitch = null;
            string[] parameters = null;
            int parametersLeft = 0;

            foreach (string argument in rawArguments)
            {
                if (parameters == null)
                {
                    if (argument.Length < 1 || argument[0] != CommandLineSwitch.PrefixSymbol)
                    {
                        return new ParseResult($"Invalid switch: \"{argument}\". " +
                            "Prefix symbol not found.");
                    }

                    string @switch = argument.Substring(1);

                    if (string.IsNullOrWhiteSpace(@switch))
                    {
                        return new ParseResult($"Invalid switch: \"{argument}\". " +
                            "Name not specified.");
                    }

                    foreach (CommandLineSwitch existingSwitch in switches)
                    {
                        if (existingSwitch.IsMatchesWithString(@switch))
                        {
                            currentSwitch = existingSwitch;
                            break;
                        }
                    }

                    if (currentSwitch == null)
                    {
                        return new ParseResult($"Unknown switch \"{argument}\".");
                    }

                    if (parsedArguments.ContainsKey(currentSwitch))
                    {
                        return new ParseResult($"Switch \"{argument}\" provided " +
                            "more than one time.");
                    }

                    parameters = new string[currentSwitch.ParametersCount];
                    parametersLeft = currentSwitch.ParametersCount;

                    parsedArguments.Add(currentSwitch, parameters);
                }
                else
                {
                    parameters[currentSwitch.ParametersCount - parametersLeft] = argument;
                    --parametersLeft;
                }

                if (parametersLeft == 0)
                {
                    parameters = null;
                    currentSwitch = null;
                }
            }

            if (parametersLeft != 0)
            {
                return new ParseResult(string.Format(
                    "Incorrect parameters count for switch \"{0}\". {1} parameters required, " +
                    "but {2} found.",
                    currentSwitch.FullPrefixedName,
                    currentSwitch.ParametersCount,
                    currentSwitch.ParametersCount - parametersLeft));
            }

            return new ParseResult(parsedArguments);
        }

        /// <summary>
        /// Helper class that represents result of parsing operation.
        /// </summary>
        private class ParseResult
        {
            public ParseResult(Dictionary<CommandLineSwitch, string[]> arguments)
            {
                this.Arguments = arguments;
                this.ErrorText = null;
            }

            public ParseResult(string errorText)
            {
                this.Arguments = null;
                this.ErrorText = errorText;
            }

            public Dictionary<CommandLineSwitch, string[]> Arguments { get; private set; }

            public string ErrorText { get; private set; }

            public bool ParsingFailed
            {
                get
                {
                    return this.Arguments == null;
                }
            }
        }
    }
}
