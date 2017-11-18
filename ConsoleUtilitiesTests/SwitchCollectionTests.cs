// Copyright (c) Anton Vasiliev. All rights reserved.
// Licensed under the MIT license.
// See the License.md file in the project root for full license information.

namespace Silvers.ConsoleUtilitiesTests
{
    using System;
    using System.Collections.Generic;
    using Silvers.ConsoleUtilities;
    using Xunit;

    public class SwitchCollectionTests
    {
        public static IEnumerable<object[]> GetIncompatibleSwitchesPair()
        {
            var baseSwitch = new CommandLineSwitch("switch", 's', 0, "Test switch.");
            var incompatibleSwitch1 = new CommandLineSwitch("other-switch", 's', 0, "Test switch.");
            var incompatibleSwitch2 = new CommandLineSwitch("switch", 'p', 0, "Test switch.");
            var incompatibleSwitch3 = new CommandLineSwitch(null, 's', 0, "Test switch.");
            var incompatibleSwitch4 = new CommandLineSwitch("switch", null, 0, "Test switch.");

            yield return new object[] { baseSwitch, incompatibleSwitch1 };
            yield return new object[] { baseSwitch, incompatibleSwitch2 };
            yield return new object[] { baseSwitch, incompatibleSwitch3 };
            yield return new object[] { baseSwitch, incompatibleSwitch4 };
            yield return new object[] { incompatibleSwitch3, baseSwitch };
            yield return new object[] { incompatibleSwitch4, baseSwitch };
        }

        public static IEnumerable<object[]> GetCompatibleSwitchesPair()
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
            yield return new object[] { switch5, switch3 };
            yield return new object[] { switch5, switch6 };
        }

        public static IEnumerable<object[]> GetCompatibleSwitchesList()
        {
            var switch1 = new CommandLineSwitch("switch1", 's', 0, "Test switch.");
            var switch2 = new CommandLineSwitch("switch2", 'p', 0, "Test switch.");
            var switch3 = new CommandLineSwitch("switch3", null, 0, "Test switch.");
            var switch4 = new CommandLineSwitch("switch4", null, 0, "Test switch.");
            var switch5 = new CommandLineSwitch(null, 'a', 0, "Test switch.");
            var switch6 = new CommandLineSwitch(null, 'b', 0, "Test switch.");

            yield return new object[]
            {
                new CommandLineSwitch[] { switch1, switch2, switch3, switch4 }
            };
            yield return new object[]
            {
                new CommandLineSwitch[] { switch1, switch3, switch4, switch5 }
            };
            yield return new object[]
            {
                new CommandLineSwitch[] { switch1, switch5, switch2, switch6 }
            };
            yield return new object[]
            {
                new CommandLineSwitch[] { switch3, switch4, switch6, switch1 }
            };
            yield return new object[]
            {
                new CommandLineSwitch[] { switch3, switch5, switch2, switch4 }
            };
            yield return new object[]
            {
                new CommandLineSwitch[] { switch5, switch3, switch1, switch2 }
            };
            yield return new object[]
            {
                new CommandLineSwitch[] { switch5, switch6, switch4, switch3 }
            };
        }

        [Fact]
        public void CreateEmptySwicthCollection()
        {
            var switches = new CommandLineSwitchSet();
        }

        [Fact]
        public void CreateAndCopyEmptySwicthCollection()
        {
            var sourceSwitches = new CommandLineSwitchSet();
            var copySwitches = new CommandLineSwitchSet(sourceSwitches);
        }

        [Theory]
        [MemberData(nameof(GetCompatibleSwitchesPair))]
        public void CreateSwicthCollectionWithSwitches(
            CommandLineSwitch firstSwitch, CommandLineSwitch secondSwitch)
        {
            var switchArray = new CommandLineSwitch[] { firstSwitch, secondSwitch };

            var switches = new CommandLineSwitchSet(switchArray);
        }

        [Theory]
        [MemberData(nameof(GetCompatibleSwitchesPair))]
        public void CreateAndCopySwicthCollectionWithSwitches(
            CommandLineSwitch firstSwitch, CommandLineSwitch secondSwitch)
        {
            var switchArray = new CommandLineSwitch[] { firstSwitch, secondSwitch };

            var sourceSwitches = new CommandLineSwitchSet(switchArray);
            var copySwitches = new CommandLineSwitchSet(sourceSwitches);
        }

        [Theory]
        [MemberData(nameof(GetCompatibleSwitchesPair))]
        public void AddCorrectSwitchesToEmptyCollection(
            CommandLineSwitch firstSwitch, CommandLineSwitch secondSwitch)
        {
            var switches = new CommandLineSwitchSet();
            switches.AddSwitch(firstSwitch);
            switches.AddSwitch(secondSwitch);
        }

        [Theory]
        [MemberData(nameof(GetCompatibleSwitchesList))]
        public void AddCorrectSwitchesToCollection(CommandLineSwitch[] switches)
        {
            var switchArray = new CommandLineSwitch[] { switches[0], switches[1] };

            var switchesCollection = new CommandLineSwitchSet(switchArray);

            switchesCollection.AddSwitch(switches[2]);
        }

        [Theory]
        [MemberData(nameof(GetCompatibleSwitchesList))]
        public void AddCorrectSwitchesToCopiedCollection(CommandLineSwitch[] switches)
        {
            var switchArray = new CommandLineSwitch[] { switches[0], switches[1] };

            var sourceSwitches = new CommandLineSwitchSet(switchArray);
            var copySwitches = new CommandLineSwitchSet(sourceSwitches);

            copySwitches.AddSwitch(switches[2]);
        }

        [Theory]
        [MemberData(nameof(GetIncompatibleSwitchesPair))]
        public void CreateSwitchCollectionWithIncompatibleSwitches(
            CommandLineSwitch firstSwitch, CommandLineSwitch secondSwitch)
        {
            var switchArray = new CommandLineSwitch[] { firstSwitch, secondSwitch };

            Assert.Throws<InvalidOperationException>(() =>
            {
                var switches = new CommandLineSwitchSet(switchArray);
            });
        }

        [Theory]
        [MemberData(nameof(GetIncompatibleSwitchesPair))]
        public void AddConflictingSwitchesToEmptyCollection(
            CommandLineSwitch firstSwitch, CommandLineSwitch secondSwitch)
        {
            var switches = new CommandLineSwitchSet();
            switches.AddSwitch(firstSwitch);

            Assert.Throws<InvalidOperationException>(() =>
            {
                switches.AddSwitch(secondSwitch);
            });
        }
    }
}
