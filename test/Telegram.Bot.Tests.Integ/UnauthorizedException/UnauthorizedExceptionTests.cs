using System.Threading.Tasks;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Exceptions;
using Telegram.Bot.Tests.Integ.Framework;
using Xunit;

namespace Telegram.Bot.Tests.Integ.Unauthorized
{
    [Collection(Constants.TestCollections.BadRequestException)]
    [TestCaseOrderer(Constants.TestCaseOrderer, Constants.AssemblyName)]

    public class UnauthorizedExceptionTests
    {
        [OrderedFact("Should throw UnauthorizedException")]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.GetMe)]
        public async Task Should_Throw_UnauthorizedException()
        {
            const string botToken = "1234567:4TT8bAc8GHUspu3ERYn-KGcvsvGB9u_n4ddy";
            ITelegramBotClient botClient = new TelegramBotClient(botToken)
            {
                ExceptionParser = ExceptionParser.CreateDefault()
            };

            UnauthorizedException exception = await Assert.ThrowsAsync<UnauthorizedException>(
                async () => await botClient.GetMeAsync()
            );

            Assert.Equal(401, exception.ErrorCode);
            Assert.Equal("Unauthorized", exception.Message);
        }
    }
}
