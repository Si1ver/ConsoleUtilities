// Copyright (c) Anton Vasiliev. All rights reserved.
// Licensed under the MIT license.
// See the License.md file in the project root for full license information.

namespace Silvers.ConsoleUtilitiesTests
{
    using System;
    using System.Collections.Generic;
    using Silvers.ConsoleUtilities;
    using Xunit;

    public class ArgumentsTests
    {
        public static IEnumerable<object[]> GetIncorrectArgumentsConstructorParameters()
        {
            var switch1 = new CommandLineSwitch("switch1", 's', 0, "Test switch.");
            var switch2 = new CommandLineSwitch("switch2", 'p', 0, "Test switch.");

            var switches = new CommandLineSwitchSet(new CommandLineSwitch[] { switch1, switch2 });
            var arguments = new string[] { "-switch1", "-p" };

            yield return new object[] { null, null };
            yield return new object[] { switches, null };
            yield return new object[] { null, arguments };
        }

        public static IEnumerable<object[]> GetIncorrectCommandLineArguments()
        {
            var emptySwitches = new CommandLineSwitchSet();

            var switch1 = new CommandLineSwitch("switch1", 's', 0, "Test switch.");
            var switch2 = new CommandLineSwitch("switch2", 'p', 1, "Test switch.");

            var switches = new CommandLineSwitchSet(new CommandLineSwitch[] { switch1, switch2 });

            var arguments1 = new string[] { "switch1" };
            var arguments2 = new string[] { "-switch1" };
            var arguments3 = new string[] { "-switch1", "-switch1" };
            var arguments4 = new string[] { "-switch1", "-s" };
            var arguments5 = new string[] { "-switch1", "-p" };
            var arguments6 = new string[] { "-switch1", "-p", "d", "-d" };
            var arguments7 = new string[] { "-switch1", "-p", "d", "d" };
            var arguments8 = new string[] { "-switch1", "a" };
            var arguments9 = new string[] { "-switch1", "-" };

            yield return new object[] { emptySwitches, arguments1 };
            yield return new object[] { emptySwitches, arguments2 };
            yield return new object[] { switches, arguments1 };
            yield return new object[] { switches, arguments3 };
            yield return new object[] { switches, arguments4 };
            yield return new object[] { switches, arguments5 };
            yield return new object[] { switches, arguments6 };
            yield return new object[] { switches, arguments7 };
            yield return new object[] { switches, arguments8 };
            yield return new object[] { switches, arguments9 };
        }

        public static IEnumerable<object[]> GetCorrectCommandLineArguments()
        {
            var emptySwitches = new CommandLineSwitchSet();

            var switch1 = new CommandLineSwitch("switch1", 's', 0, "Test switch.");
            var switch2 = new CommandLineSwitch("switch2", 'p', 1, "Test switch.");

            var switches = new CommandLineSwitchSet(new CommandLineSwitch[] { switch1, switch2 });

            var emptyArguments = new string[] { };
            var arguments1 = new string[] { "-switch1" };
            var arguments2 = new string[] { "-switch2", "param" };
            var arguments3 = new string[] { "-s" };
            var arguments4 = new string[] { "-p", "param" };
            var arguments5 = new string[] { "-switch1", "-p", "param" };
            var arguments6 = new string[] { "-switch2", "param", "-s" };

            yield return new object[] { emptySwitches, emptyArguments };
            yield return new object[] { switches, emptyArguments };
            yield return new object[] { switches, arguments1 };
            yield return new object[] { switches, arguments2 };
            yield return new object[] { switches, arguments3 };
            yield return new object[] { switches, arguments4 };
            yield return new object[] { switches, arguments5 };
            yield return new object[] { switches, arguments6 };
        }

        [Theory]
        [MemberData(nameof(GetIncorrectArgumentsConstructorParameters))]
        public void CreateArgumentsWithIncorrectParameters(
            CommandLineSwitchSet switches, string[] rawArguments)
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var arguments = new CommandLineArguments(switches, rawArguments);
            });
        }

        [Theory]
        [MemberData(nameof(GetIncorrectCommandLineArguments))]
        public void CreateArgumentsWithIncorrectCommandLine(
            CommandLineSwitchSet switches, string[] rawArguments)
        {
            Assert.Throws<ArgumentsParsingException>(() =>
            {
                var arguments = new CommandLineArguments(switches, rawArguments);
            });
        }

        [Theory]
        [MemberData(nameof(GetCorrectCommandLineArguments))]
        public void CreateArgumentsWithСorrectCommandLine(
            CommandLineSwitchSet switches, string[] rawArguments)
        {
            var arguments = new CommandLineArguments(switches, rawArguments);
        }

        [Fact]
        public void CreateArgumentsAndGetArguments()
        {
            var switch1 = new CommandLineSwitch("switch1", 'a', 0, "Test argument.");
            var switch2 = new CommandLineSwitch("switch2", 'b', 0, "Test argument.");
            var switch3 = new CommandLineSwitch("switch3", null, 0, "Another test argument.");
            var switch4 = new CommandLineSwitch("switch4", null, 1, "Another test argument.");
            var switch5 = new CommandLineSwitch(null, 'c', 0, "Another test argument.");
            var switch6 = new CommandLineSwitch(null, 'd', 1, "Another test argument.");
            var switch7 = new CommandLineSwitch(null, 'e', 1, "Another test argument.");

            var switches = new CommandLineSwitchSet(
                new CommandLineSwitch[]
                {
                    switch1, switch2, switch3, switch4, switch5, switch6, switch7
                });

            var rawArguments = new string[] { "-a", "-switch3", "-d", "param_d", "-e", "param_e" };

            var arguments = new CommandLineArguments(switches, rawArguments);

            var switch1Result = arguments.GetArgument(switch1);
            var switch2Result = arguments.GetArgument(switch2);
            var switch3Result = arguments.GetArgument(switch3);
            var switch4Result = arguments.GetArgument(switch4);
            var switch5Result = arguments.GetArgument(switch5);
            var switch6Result = arguments.GetArgument(switch6);
            var switch7Result = arguments.GetArgument(switch7);

            Assert.True(switch1Result.isSpecified);
            Assert.Empty(switch1Result.parameters);

            Assert.False(switch2Result.isSpecified);
            Assert.Null(switch2Result.parameters);

            Assert.True(switch3Result.isSpecified);
            Assert.Empty(switch3Result.parameters);

            Assert.False(switch4Result.isSpecified);
            Assert.Null(switch4Result.parameters);

            Assert.False(switch5Result.isSpecified);
            Assert.Null(switch5Result.parameters);

            Assert.True(switch6Result.isSpecified);
            Assert.Single(switch6Result.parameters, "param_d");

            Assert.True(switch7Result.isSpecified);
            Assert.Single(switch7Result.parameters, "param_e");
        }
    }
}
