using Telegram.Bot.Exceptions;

namespace Telegram.Bot.Extensions.Exceptions.Parsing
{
    internal class BadRequestExceptionDescriptor<TException> : ApiExceptionDescriptor<TException>
        where TException : BadRequestException
    {
        public BadRequestExceptionDescriptor(string errorMessageRegex)
            : base(BadRequestException.BadRequestErrorCode, errorMessageRegex)
        { }
    }
}