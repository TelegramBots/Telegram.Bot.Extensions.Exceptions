﻿using Telegram.Bot.Types;

// ReSharper disable once CheckNamespace
namespace Telegram.Bot.Exceptions
{
    /// <summary>
    /// The exception that is thrown when the sticker set is invalid
    /// </summary>
    public class StickerSetNotModifiedException : BadRequestException
    {
        /// <summary>
        /// Initializes a new object of the <see cref="StickerSetNotModifiedException"/> class
        /// </summary>
        /// <param name="message">The error message of this exception.</param>
        public StickerSetNotModifiedException(string message)
            : base(message)
        { }

        /// <summary>
        /// Initializes a new object of the <see cref="StickerSetNotModifiedException"/> class
        /// </summary>
        /// <param name="message">The error message of this exception.</param>
        /// <param name="parameters">Response parameters</param>
        public StickerSetNotModifiedException(
            string message, ResponseParameters? parameters = default)
            : base(message, parameters)
        { }
    }
}
