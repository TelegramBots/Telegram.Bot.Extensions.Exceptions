using Telegram.Bot.Types;

// ReSharper disable CheckNamespace
namespace Telegram.Bot.Exceptions
{
    /// <summary>
    /// Represents an error from Bot API with 403 Forbidden HTTP status
    /// </summary>
    public class UnauthorizedException : ApiRequestException
    {
        /// <inheritdoc />
        public override int ErrorCode => UnauthorizedErrorCode;

        /// <summary>
        /// Represent error code number
        /// </summary>
        public const int UnauthorizedErrorCode = 401;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnauthorizedException"/> class
        /// </summary>
        /// <param name="message">The message</param>
        public UnauthorizedException(string message)
            : base(message, UnauthorizedErrorCode)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnauthorizedException"/> class
        /// </summary>
        /// <param name="message">The message</param>
        /// <param name="parameters">Response parameters</param>
        public UnauthorizedException(string message, ResponseParameters? parameters = default)
            : base(message, UnauthorizedErrorCode, parameters)
        { }
    }
}
