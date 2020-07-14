using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;

namespace Telegram.Bot.Extensions.Exceptions.Exceptions
{
    /// <summary>
    /// Represents an error from Bot API with 404 Not Found HTTP status
    /// </summary>
    public class NotFoundException : ApiRequestException
    {
        /// <inheritdoc />
        public override int ErrorCode => NotFoundErrorCode;

        /// <summary>
        /// Represent error code number
        /// </summary>
        public const int NotFoundErrorCode = 404;

        /// <summary>
        /// Initializes a new instance of the <see cref="NotFoundException"/> class
        /// </summary>
        /// <param name="message">The message</param>
        public NotFoundException(string message)
            : base(message, NotFoundErrorCode)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="NotFoundException"/> class
        /// </summary>
        /// <param name="message">The message</param>
        /// <param name="parameters">Response parameters</param>
        public NotFoundException(string message, ResponseParameters? parameters = default)
            : base(message, NotFoundErrorCode, parameters)
        { }
    }
}
