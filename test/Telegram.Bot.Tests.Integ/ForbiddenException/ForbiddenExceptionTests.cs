using System.Threading.Tasks;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Tests.Integ.Framework;
using Xunit;

namespace Telegram.Bot.Tests.Integ.Forbidden
{
    [Collection(Constants.TestCollections.ForbiddenException)]
    [TestCaseOrderer(Constants.TestCaseOrderer, Constants.AssemblyName)]

    public class ForbiddenExceptionTests
    {
        private TestsFixture Fixture { get; }
        private ITelegramBotClient BotClient => Fixture.BotClient;

        public ForbiddenExceptionTests(TestsFixture testsFixture)
        {
            Fixture = testsFixture;
        }

        [OrderedFact("Should throw ForbiddenException while trying to send message to the channel")]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.SendMessage)]
        public async Task Should_Throw_ForbiddenException_For_Non_Existent_Chat()
        {
            ForbiddenException exception = await Assert.ThrowsAsync<ForbiddenException>(
                async () => await BotClient.SendTextMessageAsync("@telegram", "test")
            );

            Assert.Equal(403, exception.ErrorCode);
            Assert.Contains("bot is not a member of the channel chat", exception.Message);
        }
    }
}
