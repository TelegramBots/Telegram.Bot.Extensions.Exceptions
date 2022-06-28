using System.Threading.Tasks;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Tests.Integ.Framework;
using Telegram.Bot.Types;
using Xunit;

namespace Telegram.Bot.Tests.Integ.TooManyRequests
{
    [Collection(Constants.TestCollections.TooManyRequestsException)]
    [TestCaseOrderer(Constants.TestCaseOrderer, Constants.AssemblyName)]
    public class TooManyRequestsExceptionTests
    {
        private TestsFixture Fixture { get; }

        private ITelegramBotClient BotClient => Fixture.BotClient;

        public TooManyRequestsExceptionTests(TestsFixture testsFixture)
        {
            Fixture = testsFixture;
        }

        [OrderedFact("Should throw TooManyRequestsException")]
        public async Task Should_Throw_TooManyRequestsException()
        {
            const int parallelTaskCount = 250;
            Task[] requestTasks = new Task[parallelTaskCount];
            for (int i = 0; i < parallelTaskCount; i++)
            {
                requestTasks[i] = Task.Run(async () =>
                {
                    Message message = await BotClient.SendTextMessageAsync(Fixture.SupergroupChat, "testmessage");
                    await BotClient.DeleteMessageAsync(Fixture.SupergroupChat, message.MessageId);
                });
            }

            TooManyRequestsException exception = await Assert.ThrowsAsync<TooManyRequestsException>(
                async () => await Task.WhenAll(requestTasks)
            );

            Assert.Equal(429, exception.ErrorCode);
            Assert.Contains("Too Many Requests: retry after", exception.Message);
            Assert.NotEqual(0, exception.Parameters?.RetryAfter);
        }
    }
}
