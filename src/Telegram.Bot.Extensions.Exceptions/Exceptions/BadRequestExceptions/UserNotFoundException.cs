using Telegram.Bot.Types;

// ReSharper disable once CheckNamespace
namespace Telegram.Bot.Exceptions
{
    /// <summary>
    /// The exception that is thrown when the user does not exist
    /// </summary>
    public class UserNotFoundException : BadRequestException
    {
        /// <summary>
        /// Initializes a new object of the <see cref="UserNotFoundException"/> class
        /// </summary>
        /// <param name="message">The error message of this exception.</param>
        public UserNotFoundException(string message)
            : base(message)
        { }

        /// <summary>
        /// Initializes a new object of the <see cref="UserNotFoundException"/> class
        /// </summary>
        /// <param name="message">The error message of this exception.</param>
        /// <param name="parameters">Response parameters</param>
        public UserNotFoundException(string message, ResponseParameters? parameters = default)
            : base(message, parameters)
        { }
    }
}
