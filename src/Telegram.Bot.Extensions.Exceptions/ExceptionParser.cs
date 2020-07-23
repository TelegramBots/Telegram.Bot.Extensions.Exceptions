using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Exceptions.Parsing;
using Telegram.Bot.Types;

namespace Telegram.Bot.Extensions.Exceptions
{
    /// <inheritdoc />
    public class ExceptionParser : IExceptionParser
    {
        private static readonly IApiExceptionDescriptor[] ExceptionDescriptors =
        {
            new BadRequestExceptionDescriptor<ChatNotFoundException>("chat not found"),
            new BadRequestExceptionDescriptor<UserNotFoundException>("user not found"),
            new BadRequestExceptionDescriptor<InvalidUserIdException>("USER_ID_INVALID"),
            new BadRequestExceptionDescriptor<InvalidQueryIdException>(
                "query is too old and response timeout expired or query ID is invalid"
            ),

            #region Stickers

            new BadRequestExceptionDescriptor<InvalidStickerSetNameException>(
                "invalid sticker set name is specified"
            ),
            new BadRequestExceptionDescriptor<InvalidStickerEmojisException>(
                "invalid sticker emojis"
            ),
            new BadRequestExceptionDescriptor<InvalidStickerDimensionsException>(
                "STICKER_PNG_DIMENSIONS"
            ),
            new BadRequestExceptionDescriptor<StickerSetNameExistsException>(
                "sticker set name is already occupied"
            ),
            new BadRequestExceptionDescriptor<StickerSetNotModifiedException>(
                "STICKERSET_NOT_MODIFIED"
            ),

            #endregion

            #region Games

            new BadRequestExceptionDescriptor<InvalidGameShortNameException>(
                "GAME_SHORTNAME_INVALID"
            ),
            new BadRequestExceptionDescriptor<InvalidGameShortNameException>(
                "game_short_name is empty"
            ),
            new BadRequestExceptionDescriptor<InvalidGameShortNameException>(
                "wrong game short name specified"
            ),

            #endregion

            new BadRequestExceptionDescriptor<ContactRequestException>(
                "phone number can be requested in private chats only"
            ),

            new ForbiddenExceptionDescriptor<ChatNotInitiatedException>(
                "bot can't initiate conversation with a user"
            ),

            new BadRequestExceptionDescriptor<InvalidParameterException>(
                $@"Bad Request: invalid (?<{InvalidParameterException.ParamGroupName}>[\w|\s]+)$"
            )
            {
                CustomExceptionFactory = InvalidParameterExceptionFactory
            },
            new BadRequestExceptionDescriptor<InvalidParameterException>(
                $@"Bad Request: (?<{InvalidParameterException.ParamGroupName}>[\w|\s]+) invalid$"
            )
            {
                CustomExceptionFactory = InvalidParameterExceptionFactory
            },

            new BadRequestExceptionDescriptor<MessageIsNotModifiedException>(
                "message is not modified"
            ),
        };

        private readonly IApiExceptionDescriptor[] _exceptionDescriptors;

        internal ExceptionParser(IEnumerable<IApiExceptionDescriptor> exceptionDescriptors)
        {
            if (exceptionDescriptors is null)
                throw new ArgumentNullException(nameof(exceptionDescriptors));

            _exceptionDescriptors = exceptionDescriptors.ToArray();
        }

        /// <inheritdoc />
        /// <exception cref="InvalidOperationException">
        /// Thrown when one of the parsers violate the contract and returns <c>null</c>
        /// </exception>
        public ApiRequestException Parse(
            int errorCode,
            string description,
            ResponseParameters? responseParameters)
        {
            ApiRequestException? exception;

            foreach (var descriptor in _exceptionDescriptors)
            {
                if (descriptor.TryParseException(
                    errorCode,
                    description,
                    responseParameters,
                    out exception))
                {
                    return exception ?? throw new InvalidOperationException(
                        $"Descriptor for '{descriptor.Type.Name}' exception returned null. Parsed " +
                        "exception must not be null if TryParseException returned true."
                    );
                }
            }

            exception = errorCode switch
            {
                BadRequestException.BadRequestErrorCode =>
                    new BadRequestException(description, responseParameters),

                UnauthorizedException.UnauthorizedErrorCode =>
                    new UnauthorizedException(description, responseParameters),

                ForbiddenException.ForbiddenErrorCode =>
                    new ForbiddenException(description, responseParameters),

                NotFoundException.NotFoundErrorCode =>
                    new NotFoundException(description, responseParameters),

                TooManyRequestsException.TooManyRequestsErrorCode =>
                    new TooManyRequestsException(description, responseParameters),

                _ => new ApiRequestException(description, errorCode, responseParameters)
            };

            return exception;
        }

        private static InvalidParameterException InvalidParameterExceptionFactory(
            string errorMessageRegex,
            int errorCode,
            string description,
            ResponseParameters? responseParameter)
        {
            var paramName = Regex.Match(description, errorMessageRegex, RegexOptions.IgnoreCase)
                .Groups[InvalidParameterException.ParamGroupName]
                .Value;

            return new InvalidParameterException(paramName, description);
        }

        /// <summary>
        /// Creates a default implementation of <see cref="IExceptionParser"/>
        /// </summary>
        /// <returns>An instance of <see cref="ExceptionParser"/></returns>
        public static IExceptionParser CreateDefault() => new ExceptionParser(ExceptionDescriptors);
    }
}
