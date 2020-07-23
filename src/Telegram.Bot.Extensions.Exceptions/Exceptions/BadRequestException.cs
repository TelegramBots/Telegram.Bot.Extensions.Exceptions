using Telegram.Bot.Types;

// ReSharper disable CheckNamespace
namespace Telegram.Bot.Exceptions
{
    /// <summary>
    /// Represents an error from Bot API with 400 Bad Request HTTP status
    /// </summary>
    public class BadRequestException : ApiRequestException
    {
        /// <inheritdoc />
        public override int ErrorCode => BadRequestErrorCode;

        /// <summary>
        /// Represent error code number
        /// </summary>
        public const int BadRequestErrorCode = 400;

        /// <summary>
        /// Initializes a new instance of the <see cref="BadRequestException"/> class
        /// </summary>
        /// <param name="message">The message</param>
        /// <param name="parameters">Response parameters</param>
        public BadRequestException(string message, ResponseParameters? parameters = default)
            : base(message, BadRequestErrorCode, parameters)
        { }
    }
}
