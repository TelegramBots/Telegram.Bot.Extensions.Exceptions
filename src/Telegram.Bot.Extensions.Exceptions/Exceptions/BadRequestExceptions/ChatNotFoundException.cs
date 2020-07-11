using Telegram.Bot.Types;

// ReSharper disable once CheckNamespace
namespace Telegram.Bot.Exceptions
{
    /// <summary>
    /// The exception that is thrown when the chat does not exist
    /// </summary>
    public class ChatNotFoundException : BadRequestException
    {
        /// <summary>
        /// Initializes a new object of the <see cref="ChatNotFoundException"/> class
        /// </summary>
        /// <param name="message">The error message of this exception.</param>
        public ChatNotFoundException(string message)
            : base(message)
        { }

        /// <summary>
        /// Initializes a new object of the <see cref="ChatNotFoundException"/> class
        /// </summary>
        /// <param name="message">The error message of this exception.</param>
        /// <param name="parameters">Response parameters</param>
        public ChatNotFoundException(string message, ResponseParameters? parameters = default)
            : base(message, parameters)
        { }
    }
}
