using Telegram.Bot.Types;

// ReSharper disable CheckNamespace
namespace Telegram.Bot.Exceptions
{
    /// <summary>
    /// Represents an error from Bot API with 429 Too Many Requests HTTP status
    /// </summary>
    public class TooManyRequestsException : ApiRequestException
    {
        /// <inheritdoc />
        public override int ErrorCode => TooManyRequestsErrorCode;

        /// <summary>
        /// Represent error code number
        /// </summary>
        public const int TooManyRequestsErrorCode = 429;

        /// <summary>
        /// Initializes a new instance of the <see cref="TooManyRequestsException"/> class
        /// </summary>
        /// <param name="message">The message</param>
        /// <param name="parameters">Response parameters</param>
        public TooManyRequestsException(string message, ResponseParameters? parameters = default)
            : base(message, TooManyRequestsErrorCode, parameters)
        { }
    }
}
