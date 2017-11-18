// Copyright (c) Anton Vasiliev. All rights reserved.
// Licensed under the MIT license.
// See the License.md file in the project root for full license information.

namespace Silvers.ConsoleUtilities
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Represents an error that occurs when opening file for output stream.
    /// </summary>
    [Serializable]
    public class StreamOutputException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StreamOutputException"/> class with
        /// specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception,
        /// or a null reference if no inner exception is specified.</param>
        public StreamOutputException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StreamOutputException"/> class
        /// with serialized data.
        /// </summary>
        /// <param name="info">The System.Runtime.Serialization.SerializationInfo that holds the
        /// serialized object data about the exception being thrown.</param>
        /// <param name="context">The System.Runtime.Serialization.StreamingContext that contains
        /// contextual information about the source or destination.</param>
        protected StreamOutputException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
