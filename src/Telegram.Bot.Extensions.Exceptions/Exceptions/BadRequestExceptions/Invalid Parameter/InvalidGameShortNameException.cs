using Telegram.Bot.Types;

// ReSharper disable once CheckNamespace
namespace Telegram.Bot.Exceptions
{
    /// <summary>
    /// The exception that is thrown when the specified game short name is empty, invalid, or wrong(non-existent).
    /// </summary>
    public class InvalidGameShortNameException : InvalidParameterException
    {
        /// <summary>
        /// Initializes a new object of the <see cref="InvalidGameShortNameException"/> class
        /// </summary>
        /// <param name="message">The error message of this exception.</param>
        public InvalidGameShortNameException(string message)
            : base("game_short_name", message)
        { }

        /// <summary>
        /// Initializes a new object of the <see cref="InvalidGameShortNameException"/> class
        /// </summary>
        /// <param name="message">The error message of this exception.</param>
        /// <param name="parameters">Response parameters</param>
        public InvalidGameShortNameException(
            string message,
            ResponseParameters? parameters = default)
            : base("game_short_name", message, parameters)
        { }
    }
}
