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
    /// Represents a collection of command line switches accepted by console utility.
    /// </summary>
    public class CommandLineSwitchSet : IEnumerable<CommandLineSwitch>
    {
        /// <summary>
        /// List of added switches.
        /// </summary>
        private List<CommandLineSwitch> switches;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandLineSwitchSet"/> class with
        /// an empty collection of switches.
        /// </summary>
        public CommandLineSwitchSet()
        {
            this.switches = new List<CommandLineSwitch>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandLineSwitchSet"/> class
        /// and copies a list of switches from another collection.
        /// </summary>
        /// <param name="other">Collection to copy switches from.</param>
        public CommandLineSwitchSet(CommandLineSwitchSet other)
        {
            this.switches = new List<CommandLineSwitch>(other.switches);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandLineSwitchSet"/> class
        /// and adds a list of switches to it.
        /// </summary>
        /// <param name="switchesArray">List of switches to add to collection.</param>
        public CommandLineSwitchSet(CommandLineSwitch[] switchesArray)
        {
            this.switches = new List<CommandLineSwitch>();

            foreach (CommandLineSwitch @switch in switchesArray)
            {
                this.AddSwitch(@switch);
            }
        }

        /// <summary>
        /// Add switch to collection.
        /// </summary>
        /// <param name="switch">Switch to add to collection.</param>
        public void AddSwitch(CommandLineSwitch @switch)
        {
            foreach (var existingSwitch in this.switches)
            {
                if (@switch.IsIncompatibleWith(existingSwitch))
                {
                    throw new InvalidOperationException(string.Format(
                        "Switch \"{0}\" is incompatible with switch \"{1}\".",
                        @switch.FullPrefixedName,
                        existingSwitch.FullPrefixedName));
                }
            }

            this.switches.Add(@switch);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the <see cref="CommandLineSwitchSet"/>.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<CommandLineSwitch> GetEnumerator()
        {
            return this.switches.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.switches.GetEnumerator();
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            var builder = new StringBuilder();

            builder.AppendLine("Command line switches:");

            foreach (var @switch in this.switches)
            {
                builder.AppendLine(@switch.ToString());
            }

            return builder.ToString();
        }
    }
}
