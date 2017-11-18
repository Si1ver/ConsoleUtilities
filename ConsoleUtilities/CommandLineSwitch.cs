// Copyright (c) Anton Vasiliev. All rights reserved.
// Licensed under the MIT license.
// See the License.md file in the project root for full license information.

namespace Silvers.ConsoleUtilities
{
    using System;
    using System.Text;

    /// <summary>
    /// Class for defining command line switch. Defined switches are later used with
    /// <see cref="CommandLineArguments"/>.
    /// </summary>
    public class CommandLineSwitch
    {
        /// <summary>
        /// Prefix symbol for all switches. Placed in front of names and shortcuts.
        /// </summary>
        public const char PrefixSymbol = '-';

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandLineSwitch"/> class to use
        /// with <see cref="CommandLineArguments"/>. All parameters are verified
        /// and <see cref="ArgumentException"/> is thrown if any invalid parameters are found.
        /// </summary>
        /// <param name="name">Full name of switch. Optional, can't be shorter than 2
        /// characters.</param>
        /// <param name="shortcut">Single character alias for switch name. Optional if name is
        /// not empty.</param>
        /// <param name="parametersCount">Amount of parameters that have to be passed to switch.
        /// All parameters are required if switch is present.</param>
        /// <param name="meaning">Description of switch to show in help.</param>
        public CommandLineSwitch(string name, char? shortcut, int parametersCount, string meaning)
        {
            this.Name = name ?? string.Empty;

            if (this.Name.Length == 1)
            {
                throw new ArgumentException(
                    "Name can't be shorter than 2 characters.", nameof(name));
            }

            foreach (char c in this.Name)
            {
                if (!CharIsLetterOrDigit(c) && c != '-' && c != '_')
                {
                    throw new ArgumentException(
                        $"Invalid character \"{c}\". Valid characters are letters, digits," +
                        " underscores and hyphens.", nameof(name));
                }
            }

            this.Shortcut = shortcut;

            if (this.Shortcut.HasValue && !CharIsLetterOrDigit(this.Shortcut.Value))
            {
                throw new ArgumentException(
                    "Switch shortcut must be letter or digit.", nameof(shortcut));
            }

            if (!shortcut.HasValue && string.IsNullOrEmpty(name))
            {
                throw new ArgumentException(
                    "Both switch name and shortcut can't be empty at the same time.", nameof(name));
            }

            this.ParametersCount = parametersCount;

            if (parametersCount < 0)
            {
                throw new ArgumentException(
                    "Parameter count can't be less than zero.", nameof(parametersCount));
            }

            this.Meaning = meaning;

            if (string.IsNullOrWhiteSpace(meaning))
            {
                throw new ArgumentException(
                    "Switch meaning can't be empty.", nameof(meaning));
            }
        }

        /// <summary>
        /// Gets full name of the switch. Contains two or more symbols. Can be empty if Shortcut
        /// is not empty.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets shortcut cheracter for the switch. Can be null if Name is not empty.
        /// </summary>
        public char? Shortcut { get; private set; }

        /// <summary>
        /// Gets exact number of parameters that need to be provided after switch.
        /// </summary>
        public int ParametersCount { get; private set; }

        /// <summary>
        /// Gets text description of the switch. Presented to user as part of usage information.
        /// </summary>
        public string Meaning { get; private set; }

        /// <summary>
        /// Gets prefixed shortcut or prefixed namein case if shortcut is not set.
        /// </summary>
        public string ShortPrefixedName
        {
            get
            {
                if (this.Shortcut.HasValue)
                {
                    return $"{PrefixSymbol}{this.Shortcut.Value}";
                }
                else
                {
                    return $"{PrefixSymbol}{this.Name}";
                }
            }
        }

        /// <summary>
        /// Gets prefixed name or prefixed shortcut in case if name is not set.
        /// </summary>
        public string FullPrefixedName
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(this.Name))
                {
                    return $"{PrefixSymbol}{this.Name}";
                }
                else
                {
                    return $"{PrefixSymbol}{this.Shortcut.Value}";
                }
            }
        }

        /// <summary>
        /// Compares switch's name and shortcut with specified string.
        /// </summary>
        /// <param name="key">String to compare with. Can't be null.</param>
        /// <returns>true, if name or shortcut matches with specified string.</returns>
        public bool IsMatchesWithString(string key)
        {
            if (key.Length > 1)
            {
                return !string.IsNullOrEmpty(this.Name) && key == this.Name;
            }
            else
            {
                return this.Shortcut.HasValue && key[0] == this.Shortcut.Value;
            }
        }

        /// <summary>
        /// Check that switch is incompatible with other switch.
        /// Two switches are considered incompatible if they share names or shortcuts.
        /// </summary>
        /// <param name="other">Switch to compare with current switch. Can't be null.</param>
        /// <returns>true, if switches are incompatible.</returns>
        public bool IsIncompatibleWith(CommandLineSwitch other)
        {
            return (!string.IsNullOrEmpty(this.Name) && this.Name == other.Name) ||
                (this.Shortcut.HasValue && other.Shortcut.HasValue &&
                this.Shortcut.Value == other.Shortcut.Value);
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            var builder = new StringBuilder();

            if (this.Shortcut.HasValue)
            {
                builder.Append(PrefixSymbol);
                builder.Append(this.Shortcut.Value);

                if (!string.IsNullOrEmpty(this.Name))
                {
                    builder.Append(" or ");
                }
            }

            if (!string.IsNullOrEmpty(this.Name))
            {
                builder.Append(PrefixSymbol);
                builder.Append(this.Name);
            }

            if (this.ParametersCount == 1)
            {
                builder.Append(" <value>");
            }
            else if (this.ParametersCount > 1)
            {
                builder.AppendFormat(" <{0} values>", this.ParametersCount);
            }

            builder.Append("\n\t");
            builder.Append(this.Meaning);

            return builder.ToString();
        }

        /// <summary>
        /// Helper function to check characters in name and shortcut.
        /// </summary>
        /// <param name="c">Character to check.</param>
        /// <returns>true, if character is latin letter or digit.</returns>
        private static bool CharIsLetterOrDigit(char c)
        {
            return (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || (c >= '0' && c <= '9');
        }
    }
}
