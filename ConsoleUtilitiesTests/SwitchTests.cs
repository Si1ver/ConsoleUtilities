// Copyright (c) Anton Vasiliev. All rights reserved.
// Licensed under the MIT license.
// See the License.md file in the project root for full license information.

namespace Silvers.ConsoleUtilitiesTests
{
    using System;
    using System.Collections.Generic;
    using Silvers.ConsoleUtilities;
    using Xunit;

    public class SwitchTests
    {
        public static IEnumerable<object[]> GetCorrectSwitches()
        {
            yield return new object[] { "switch1", 's', 0, "Test switch." };
            yield return new object[] { "Switch", null, 0, "Another switch." };
            yield return new object[] { null, 'S', 1, "Yet another test switch." };
            yield return new object[] { string.Empty, '1', 10, "Test switch." };
        }

        public static IEnumerable<object[]> GetIncorrectSwitches()
        {
            yield return new object[] { "  ", null, 0, "Test switch." };
            yield return new object[] { "  ", '1', 0, "Test switch." };
            yield return new object[] { string.Empty, null, 0, "Another switch." };
            yield return new object[] { "l", null, 0, "Another switch." };
            yield return new object[] { null, null, 1, "Yet another test switch." };
            yield return new object[] { "switch0", 's', 10, string.Empty };
            yield return new object[] { string.Empty, '!', 10, "Test switch." };
            yield return new object[] { "***", null, 10, "Test switch." };
            yield return new object[] { "SWITCH", 's', -5, "Test switch." };
        }

        public static IEnumerable<object[]> GetSwitchesAndShortNames()
        {
            yield return new object[] { "switch", 'a', 0, "Test switch.", "-a" };
            yield return new object[] { string.Empty, 's', 0, "Another switch.", "-s" };
            yield return new object[] { "one", null, 1, "And other.", "-one" };
        }

        public static IEnumerable<object[]> GetSwitchesAndFullNames()
        {
            yield return new object[] { "one", 'a', 0, "Test switch.", "-one" };
            yield return new object[] { string.Empty, 's', 0, "Another switch.", "-s" };
            yield return new object[] { "one", null, 1, "And other.", "-one" };
        }

        public static IEnumerable<object[]> GetIncompatibleSwitches()
        {
            var baseSwitch = new CommandLineSwitch("switch1", 's', 0, "Test switch.");
            var switch11 = new CommandLineSwitch("switch11", 's', 0, "Test switch.");
            var switch12 = new CommandLineSwitch("switch1", 'p', 0, "Test switch.");
            var switch13 = new CommandLineSwitch(null, 's', 0, "Test switch.");
            var switch14 = new CommandLineSwitch("switch1", null, 0, "Test switch.");

            yield return new object[] { baseSwitch, switch11 };
            yield return new object[] { baseSwitch, switch12 };
            yield return new object[] { baseSwitch, switch13 };
            yield return new object[] { baseSwitch, switch14 };
            yield return new object[] { switch13, baseSwitch };
            yield return new object[] { switch14, baseSwitch };
        }

        public static IEnumerable<object[]> GetCompatibleSwitches()
        {
            var switch1 = new CommandLineSwitch("switch1", 's', 0, "Test switch.");
            var switch2 = new CommandLineSwitch("switch2", 'p', 0, "Test switch.");
            var switch3 = new CommandLineSwitch("switch3", null, 0, "Test switch.");
            var switch4 = new CommandLineSwitch("switch4", null, 0, "Test switch.");
            var switch5 = new CommandLineSwitch(null, 'a', 0, "Test switch.");
            var switch6 = new CommandLineSwitch(null, 'b', 0, "Test switch.");

            yield return new object[] { switch1, switch2 };
            yield return new object[] { switch1, switch3 };
            yield return new object[] { switch1, switch5 };
            yield return new object[] { switch3, switch4 };
            yield return new object[] { switch3, switch5 };
            yield return new object[] { switch5, switch6 };
        }

        public static IEnumerable<object[]> GetSwitchAndMatchingKey()
        {
            var switch1 = new CommandLineSwitch("switch", 's', 0, "Test switch.");
            var switch2 = new CommandLineSwitch("switch", null, 0, "Test switch.");
            var switch3 = new CommandLineSwitch(null, 's', 0, "Test switch.");

            yield return new object[] { switch1, "switch" };
            yield return new object[] { switch1, "s" };
            yield return new object[] { switch2, "switch" };
            yield return new object[] { switch3, "s" };
        }

        public static IEnumerable<object[]> GetSwitchAndStringRepresentation()
        {
            var switch1 = new CommandLineSwitch("switch", 's', 0, "Test switch.");
            var switch2 = new CommandLineSwitch("switch", null, 0, "Test switch.");
            var switch3 = new CommandLineSwitch(null, 's', 0, "Test switch.");
            var switch4 = new CommandLineSwitch("switch", 's', 1, "Test switch.");
            var switch5 = new CommandLineSwitch("switch", null, 1, "Test switch.");
            var switch6 = new CommandLineSwitch(null, 's', 1, "Test switch.");
            var switch7 = new CommandLineSwitch("switch", 's', 5, "Test switch.");
            var switch8 = new CommandLineSwitch("switch", null, 5, "Test switch.");
            var switch9 = new CommandLineSwitch(null, 's', 5, "Test switch.");

            yield return new object[] { switch1, "-s or -switch\n\tTest switch." };
            yield return new object[] { switch2, "-switch\n\tTest switch." };
            yield return new object[] { switch3, "-s\n\tTest switch." };
            yield return new object[] { switch4, "-s or -switch <value>\n\tTest switch." };
            yield return new object[] { switch5, "-switch <value>\n\tTest switch." };
            yield return new object[] { switch6, "-s <value>\n\tTest switch." };
            yield return new object[] { switch7, "-s or -switch <5 values>\n\tTest switch." };
            yield return new object[] { switch8, "-switch <5 values>\n\tTest switch." };
            yield return new object[] { switch9, "-s <5 values>\n\tTest switch." };
        }

        [Theory]
        [MemberData(nameof(GetCorrectSwitches))]
        public void CreateCorrectSwitch(
            string name, char? shortcut, int parametersCount, string meaning)
        {
            var @switch = new CommandLineSwitch(name, shortcut, parametersCount, meaning);
        }

        [Theory]
        [MemberData(nameof(GetIncorrectSwitches))]
        public void CreateIncorrectSwitch(
            string name, char? shortcut, int parametersCount, string meaning)
        {
            Assert.Throws<ArgumentException>(() =>
            {
                var @switch = new CommandLineSwitch(name, shortcut, parametersCount, meaning);
            });
        }

        [Theory]
        [MemberData(nameof(GetSwitchesAndShortNames))]
        public void ShortPrefixedNameCheck(
            string name, char? shortcut, int parametersCount, string meaning, string result)
        {
            var @switch = new CommandLineSwitch(name, shortcut, parametersCount, meaning);

            Assert.Equal(@switch.ShortPrefixedName, result);
        }

        [Theory]
        [MemberData(nameof(GetSwitchesAndFullNames))]
        public void FullPrefixedNameCheck(
            string name, char? shortcut, int parametersCount, string meaning, string result)
        {
            var @switch = new CommandLineSwitch(name, shortcut, parametersCount, meaning);

            Assert.Equal(@switch.FullPrefixedName, result);
        }

        [Theory]
        [MemberData(nameof(GetIncompatibleSwitches))]
        public void SwitchesAreIncompatible(CommandLineSwitch first, CommandLineSwitch second)
        {
            Assert.True(first.IsIncompatibleWith(second));
        }

        [Theory]
        [MemberData(nameof(GetCompatibleSwitches))]
        public void SwitchesAreCompatible(CommandLineSwitch first, CommandLineSwitch second)
        {
            Assert.False(first.IsIncompatibleWith(second));
        }

        [Theory]
        [MemberData(nameof(GetSwitchAndMatchingKey))]
        public void SwitchesAreMatchingString(CommandLineSwitch @switch, string key)
        {
            Assert.True(@switch.IsMatchesWithString(key));
        }

        [Theory]
        [MemberData(nameof(GetSwitchAndStringRepresentation))]
        public void SwitchesToString(CommandLineSwitch @switch, string stringRepresentation)
        {
            Assert.Equal(@switch.ToString(), stringRepresentation);
        }
    }
}
