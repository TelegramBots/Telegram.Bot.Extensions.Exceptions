using Telegram.Bot.Exceptions;

namespace Telegram.Bot.Extensions.Exceptions.Parsing
{
    internal class ForbiddenExceptionDescriptor<TException> : ApiExceptionDescriptor<TException>
        where TException : ForbiddenException
    {
        public ForbiddenExceptionDescriptor(string errorMessageRegex)
            : base(ForbiddenException.ForbiddenErrorCode, errorMessageRegex)
        { }
    }
}
