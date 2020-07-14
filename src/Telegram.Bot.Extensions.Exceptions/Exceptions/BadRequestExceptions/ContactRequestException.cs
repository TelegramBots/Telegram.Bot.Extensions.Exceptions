using Telegram.Bot.Types;

// ReSharper disable once CheckNamespace
namespace Telegram.Bot.Exceptions
{
    /// <summary>
    /// The exception that is thrown when the phone number requested in group chat
    /// </summary>
    public class ContactRequestException : BadRequestException
    {
        /// <summary>
        /// Initializes a new object of the <see cref="ContactRequestException"/> class
        /// </summary>
        /// <param name="message">The error message of this exception.</param>
        public ContactRequestException(string message)
            : base(message)
        { }

        /// <summary>
        /// Initializes a new object of the <see cref="ContactRequestException"/> class
        /// </summary>
        /// <param name="message">The error message of this exception.</param>
        /// <param name="parameters">Response parameters</param>
        public ContactRequestException(string message, ResponseParameters? parameters = default)
            : base(message, parameters)
        { }
    }
}
