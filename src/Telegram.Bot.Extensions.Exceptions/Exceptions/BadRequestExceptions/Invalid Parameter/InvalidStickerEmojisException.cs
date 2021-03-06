﻿using Telegram.Bot.Types;

// ReSharper disable once CheckNamespace
namespace Telegram.Bot.Exceptions
{
    /// <summary>
    /// The exception that is thrown when the sticker set is invalid
    /// </summary>
    public class InvalidStickerEmojisException : InvalidParameterException
    {
        /// <summary>
        /// Initializes a new object of the <see cref="InvalidStickerEmojisException"/> class
        /// </summary>
        /// <param name="message">The error message of this exception.</param>
        /// <param name="parameters">Response parameters</param>
        public InvalidStickerEmojisException(
            string message,
            ResponseParameters? parameters = default)
            : base("emojis", message, parameters)
        { }
    }
}
