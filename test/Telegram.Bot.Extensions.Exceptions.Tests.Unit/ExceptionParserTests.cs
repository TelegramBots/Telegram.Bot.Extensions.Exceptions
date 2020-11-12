#nullable enable

using System;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Exceptions.Parsing;
using Telegram.Bot.Types;
using Xunit;
using Xunit.Abstractions;

namespace Telegram.Bot.Extensions.Exceptions.Tests.Unit
{
    public class ExceptionParserTests
    {
        private readonly ITestOutputHelper _outputHelper;

        public ExceptionParserTests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        [Fact]
        public void Should_Throw_InvalidOperationException_On_Null_Exception()
        {
            var exceptionParser = new ExceptionParser(new[] { new MockApiExceptionDescriptor() });

            ApiRequestException? apiRequestException = default;

            var exception = Assert.Throws<InvalidOperationException>(
                () =>
                {
                    apiRequestException = exceptionParser.Parse(default!);
                }
            );

            _outputHelper.WriteLine(exception.Message);

            Assert.Null(apiRequestException);
        }
    }

    internal class MockApiExceptionDescriptor : IApiExceptionDescriptor
    {
        public int ErrorCode => 0;
        public string ErrorMessageRegex => string.Empty;
        public Type Type => typeof(ApiRequestException);

        public bool TryParseException(
            int errorCode,
            string description,
            ResponseParameters? responseParameters,
            out ApiRequestException? exception)
        {
            exception = null;
            return true;
        }
    }
}
