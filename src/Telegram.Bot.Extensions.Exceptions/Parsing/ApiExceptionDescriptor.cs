using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;

namespace Telegram.Bot.Extensions.Exceptions.Parsing
{
    internal class ApiExceptionDescriptor<TException> : IApiExceptionDescriptor
        where TException : ApiRequestException
    {
        private readonly Regex _regex;

        public int ErrorCode { get; }
        public string ErrorMessageRegex { get; }
        public Type Type => typeof(TException);
        public ExceptionFactory<TException>? CustomExceptionFactory { get; set; }

        public ApiExceptionDescriptor(int errorCode,
                                      string errorMessageRegex)
        {
            _regex = new Regex(errorMessageRegex, RegexOptions.Compiled | RegexOptions.IgnoreCase);

            ErrorCode = errorCode;
            ErrorMessageRegex = errorMessageRegex;
        }

        public bool TryParseException(int errorCode,
                                      string description,
                                      ResponseParameters? responseParameters,
                                      [NotNullWhen(true)] out ApiRequestException? exception)
        {
            exception = null;

            if (ErrorCode != errorCode || !_regex.IsMatch(description))
            {
                return false;
            }

            exception = CustomExceptionFactory?.Invoke(ErrorMessageRegex,
                                                       errorCode,
                                                       description,
                                                       responseParameters);

            exception ??= (ApiRequestException) Activator.CreateInstance(Type,
                                                                         description,
                                                                         responseParameters);

            return true;
        }
    }
}
