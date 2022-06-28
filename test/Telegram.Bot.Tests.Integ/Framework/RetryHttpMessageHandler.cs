using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Telegram.Bot.Types;
using Xunit.Abstractions;
using Xunit.Sdk;

#nullable enable

namespace Telegram.Bot.Tests.Integ.Framework
{
    internal class RetryHttpMessageHandler : HttpClientHandler
    {
        readonly int _retryCount;
        readonly IMessageSink _diagnosticMessageSink;

        internal RetryHttpMessageHandler(int retryCount, IMessageSink diagnosticMessageSink)
        {
            _retryCount = retryCount;
            _diagnosticMessageSink = diagnosticMessageSink;
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            HttpResponseMessage? httpResponseMessage = default;

            for (var i = 0; i < _retryCount; i++)
            {
                httpResponseMessage = await base.SendAsync(request, cancellationToken);

                if (httpResponseMessage.StatusCode != (HttpStatusCode)429)
                {
                    break;
                }

                _diagnosticMessageSink.OnMessage(new DiagnosticMessage("Request was rate limited"));

                var body = await httpResponseMessage.Content.ReadAsStringAsync(cancellationToken);

                // Deserializing with an arbitrary type parameter since Result property should
                // be empty at this stage
                var apiResponse = JsonConvert.DeserializeObject<ApiResponse<int>>(body);

                if (apiResponse.Parameters is not null)
                {
                    const int mandatoryDelay = 30;
                    var seconds = apiResponse.Parameters.RetryAfter ?? mandatoryDelay;

                    _diagnosticMessageSink.OnMessage(
                        new DiagnosticMessage($"Retry attempt {i + 1}. Waiting for {seconds} seconds before retrying.")
                    );

                    var timeToWait = TimeSpan.FromSeconds(seconds);
                    await Task.Delay(timeToWait, cancellationToken);
                }
            }

            return httpResponseMessage!;
        }
    }
}
