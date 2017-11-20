// Copyright (c) Anton Vasiliev. All rights reserved.
// Licensed under the MIT license.
// See the License.md file in the project root for full license information.

namespace S1.ExampleConsoleUtility
{
    using Silvers.ConsoleUtilities;
    using Silvers.ConsoleUtilities.SimpleConsole;

    internal static class HelloWorldUtility
    {
        // Function that implements functionalty for console utility and returns exit code.
        // Function should not throw exceptions.
        internal static int Execute(ConsoleUtilityContext context)
        {
            // Get 'user' switch from parsed arguments.
            var userNameArgument = context.Arguments.GetArgument(Switches.UserName);

            if (!userNameArgument.isSpecified)
            {
                // If switch is not specified, print error message to default output.
                context.Output.Stream.WriteLine("Error: User name not specified.");

                if (!context.Silent)
                {
                    // If not in silent mode, we can additionally print help message.
                    context.Output.Stream.WriteLine();
                    context.Output.Stream.Write(context.Switches.ToString());
                }

                // Return error code.
                return ExitCodes.InvalidArgumentSet;
            }

            // Get user name from switch.
            string userName = userNameArgument.parameters[0];

            // Get 'greeting' switch from parsed arguments.
            var greetingArgument = context.Arguments.GetArgument(Switches.Greeting);

            // If switch is present, use greeting message from it.
            string greeting = greetingArgument.isSpecified
                ? greetingArgument.parameters[0] : "Hello";

            // Parameter check is done. Now to primary functionality.
            // Print greeting message to default output.
            context.Output.Stream.WriteLine($"{greeting}, {userName}!");

            // Return success exit code.
            return DefaultExitCodes.Success;
        }

        // All stuff about command line switches.
        internal static class Switches
        {
            // Switch that sets user name.
            internal static readonly CommandLineSwitch UserName =
                new CommandLineSwitch("user", 'u', 1, "User name. This switch is required.");

            // Switch that allows to change greetng phrase.
            internal static readonly CommandLineSwitch Greeting =
                new CommandLineSwitch("greeting", 'g', 1, "Phrase to greet user with.");

            // A set of known command line switches.
            internal static readonly CommandLineSwitchSet Set;

            // It's not required to setup switches in static constructor.
            // You can do it in another place.
            static Switches()
            {
                // Create a new set of switches and fill it with default switches.
                Set = new CommandLineSwitchSet(DefaultSwitches.Collection);

                // Add custom switches to set.
                Set.AddSwitch(UserName);
                Set.AddSwitch(Greeting);
            }
        }

        // Exit codes used in console utility.
        internal static class ExitCodes
        {
            // This exit code is returned when user name is not speified.
            public const int InvalidArgumentSet = 11;
        }
    }
}
