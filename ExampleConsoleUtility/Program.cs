// Copyright (c) Anton Vasiliev. All rights reserved.
// Licensed under the MIT license.
// See the License.md file in the project root for full license information.

namespace S1.ExampleConsoleUtility
{
    using Silvers.ConsoleUtilities.SimpleConsole;

    internal static class Program
    {
        private static void Main(string[] arguments)
        {
            // SimpleConsoleUtility is a wrapper that implements default behaviour.
            SimpleConsoleUtility.RunAndQuit(
                HelloWorldUtility.Switches.Set,  // Set of known command line switches.
                arguments,  // Command line arguments (as they appear in Main function).
                HelloWorldUtility.Execute);  // Payload function that returns exit code.
        }
    }
}
