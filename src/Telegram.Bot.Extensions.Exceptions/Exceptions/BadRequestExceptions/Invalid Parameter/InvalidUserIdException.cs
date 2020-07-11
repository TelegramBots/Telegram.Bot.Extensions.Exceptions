using Telegram.Bot.Types;

// ReSharper disable once CheckNamespace
namespace Telegram.Bot.Exceptions
{
    /// <summary>
    /// The exception that is thrown when the user does not exist
    /// </summary>
    public class InvalidUserIdException : InvalidParameterException
    {
        /// <summary>
        /// Initializes a new object of the <see cref="InvalidUserIdException"/> class
        /// </summary>
        /// <param name="message">The error message of this exception.</param>
        public InvalidUserIdException(string message)
            : base("user_id", message)
        { }

        /// <summary>
        /// Initializes a new object of the <see cref="InvalidUserIdException"/> class
        /// </summary>
        /// <param name="message">The error message of this exception.</param>
        /// <param name="parameters">Response parameters</param>
        public InvalidUserIdException(string message, ResponseParameters? parameters = default)
            : base("user_id", message, parameters)
        { }
    }
}
