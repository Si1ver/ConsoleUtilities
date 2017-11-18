// Copyright (c) Anton Vasiliev. All rights reserved.
// Licensed under the MIT license.
// See the License.md file in the project root for full license information.

namespace Silvers.ConsoleUtilities
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Represents an error that occurs when parsing command line arguments.
    /// </summary>
    [Serializable]
    public class ArgumentsParsingException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ArgumentsParsingException"/> class
        /// with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public ArgumentsParsingException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArgumentsParsingException"/> class
        /// with serialized data.
        /// </summary>
        /// <param name="info">The System.Runtime.Serialization.SerializationInfo that holds the
        /// serialized object data about the exception being thrown.</param>
        /// <param name="context">The System.Runtime.Serialization.StreamingContext that contains
        /// contextual information about the source or destination.</param>
        protected ArgumentsParsingException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
