using System;
using Telegram.Bot.Types;

// ReSharper disable CheckNamespace
namespace Telegram.Bot.Exceptions
{
    /// <summary>
    /// Represents an error from Bot API with 403 Forbidden HTTP status
    /// </summary>
    public class ForbiddenException : ApiRequestException
    {
        /// <inheritdoc />
        public override int ErrorCode => ForbiddenErrorCode;

        /// <summary>
        /// Represent error code number
        /// </summary>
        public const int ForbiddenErrorCode = 403;

        /// <summary>
        /// Initializes a new instance of the <see cref="ForbiddenException"/> class
        /// </summary>
        /// <param name="message">The message</param>
        public ForbiddenException(string message)
            : base(message, ForbiddenErrorCode)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ForbiddenException"/> class
        /// </summary>
        /// <param name="message">The message</param>
        /// <param name="parameters">Response parameters</param>
        public ForbiddenException(string message, ResponseParameters? parameters = default)
            : base(message, ForbiddenErrorCode, parameters)
        { }
    }
}
