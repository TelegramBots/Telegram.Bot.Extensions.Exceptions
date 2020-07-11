using Telegram.Bot.Types;

// ReSharper disable once CheckNamespace
namespace Telegram.Bot.Exceptions
{
    /// <summary>
    /// The exception that is thrown when the bot sends message to
    /// a user who has not initiated a chat with bot yet
    /// </summary>
    public class ChatNotInitiatedException : ForbiddenException
    {
        /// <summary>
        /// Initializes a new object of the <see cref="ChatNotInitiatedException"/> class
        /// </summary>
        /// <param name="message">The message of this exception</param>
        public ChatNotInitiatedException(string message)
            : base(message)
        { }

        /// <summary>
        /// Initializes a new object of the <see cref="ChatNotInitiatedException"/> class
        /// </summary>
        /// <param name="message">The message of this exception</param>
        /// <param name="parameters">Response parameters</param>
        public ChatNotInitiatedException(
            string message,
            ResponseParameters? parameters = default)
            : base(message, parameters)
        { }
    }
}
