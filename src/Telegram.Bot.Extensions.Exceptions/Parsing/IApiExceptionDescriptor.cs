using System;
using System.Diagnostics.CodeAnalysis;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;

namespace Telegram.Bot.Extensions.Exceptions.Parsing
{
    internal interface IApiExceptionDescriptor
    {
        int ErrorCode { get; }
        string ErrorMessageRegex { get; }
        Type Type { get; }

        bool TryParseException(
            int errorCode,
            string description,
            ResponseParameters? responseParameters,
            [NotNullWhen(true)] out ApiRequestException? exception);
    }
}
