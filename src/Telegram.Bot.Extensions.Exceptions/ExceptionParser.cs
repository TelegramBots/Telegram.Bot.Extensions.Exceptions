using System;
using System.Net.Http;
using System.Threading.Tasks;
using Telegram.Bot.Exceptions;

namespace Telegram.Bot.Extensions.Exceptions
{
    /// <inheritdoc cref="IExceptionParser"/>
    public class ExceptionParser : IExceptionParser
    {
        /// <inheritdoc cref="IExceptionParser"/>
        public Task<Exception> ParseResponseAsync(HttpResponseMessage httpResponse)
        {
            throw new NotImplementedException();
        }
    }
}