using System.IO;
using System.Threading.Tasks;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Exceptions;
using Telegram.Bot.Tests.Integ.Framework;
using Xunit;

namespace Telegram.Bot.Tests.Integ.NotFound
{
    [Collection(Constants.TestCollections.NotFoundException)]
    [TestCaseOrderer(Constants.TestCaseOrderer, Constants.AssemblyName)]
    public class NotFoundExceptionTests
    {
        private TestsFixture Fixture { get; }

        private ITelegramBotClient BotClient => Fixture.BotClient;

        public NotFoundExceptionTests(TestsFixture testsFixture)
        {
            Fixture = testsFixture;
        }

        [OrderedFact("Should throw NotFoundException with \"Not Found\" error when" +
                     " malformed API Token is provided")]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.GetMe)]
        public async Task Should_Fail_Test_Api_Token()
        {
            const string botToken = "0:1this_is_an-invalid-token_for_tests";
            ITelegramBotClient botClient = new TelegramBotClient(botToken)
            {
                ExceptionParser = ExceptionParser.CreateDefault()
            };

            NotFoundException exception = await Assert.ThrowsAsync<NotFoundException>(
                async () => await botClient.TestApiAsync()
            );

            Assert.Equal(404, exception.ErrorCode);
            Assert.Equal("Not Found", exception.Message);
        }

        [OrderedFact("Should throw NotFoundException while trying to download file using wrong file_path")]
        public async Task Should_Throw_FilePath_NotFoundException()
        {
            await using MemoryStream destinationStream = new MemoryStream();

            NotFoundException exception = await Assert.ThrowsAsync<NotFoundException>(
                async () => await BotClient.DownloadFileAsync(
                    filePath: "Invalid_File_Path",
                    destination: destinationStream
                )
            );

            Assert.Equal(0, destinationStream.Length);
            Assert.Equal(0, destinationStream.Position);

            Assert.Equal(404, exception.ErrorCode);
            Assert.Equal("Not Found", exception.Message);
        }
    }
}
