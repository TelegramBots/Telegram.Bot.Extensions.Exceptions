using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;

namespace Telegram.Bot.Extensions.Exceptions.Parsing
{
    internal delegate TException ExceptionFactory<out TException>(string errorMessageRegex,
                                                                  int errorCode,
                                                                  string description,
                                                                  ResponseParameters? responseParameters)
        where TException : ApiRequestException;
}
