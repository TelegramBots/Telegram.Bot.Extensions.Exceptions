using System.Threading.Tasks;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Tests.Integ.Framework;
using Xunit;

namespace Telegram.Bot.Tests.Integ.BadRequest
{
    [Collection(Constants.TestCollections.BadRequestException)]
    [TestCaseOrderer(Constants.TestCaseOrderer, Constants.AssemblyName)]
    public class ChannelAdminBotTests : IClassFixture<ChannelAdminBotTestFixture>
    {
        private readonly ChannelAdminBotTestFixture _classFixture;

        private readonly TestsFixture _fixture;

        private ITelegramBotClient BotClient => _fixture.BotClient;

        public ChannelAdminBotTests(TestsFixture testsFixture, ChannelAdminBotTestFixture classFixture)
        {
            _fixture = testsFixture;
            _classFixture = classFixture;
        }

        [OrderedFact("Should throw exception in deleting chat photo with no photo currently set")]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.DeleteChatPhoto)]
        public async Task Should_Throw_On_Deleting_Chat_Deleted_Photo()
        {
            BadRequestException exception = await Assert.ThrowsAsync<BadRequestException>(
                async () => await BotClient.DeleteChatPhotoAsync(_classFixture.Chat.Id)
            );

            // ToDo: Create exception type
            Assert.Equal(400, exception.ErrorCode);
            Assert.Equal("Bad Request: CHAT_NOT_MODIFIED", exception.Message);
        }

        [OrderedFact("Should throw exception when trying to set sticker set for a channel")]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.SetChatStickerSet)]
        public async Task Should_Throw_On_Setting_Chat_Sticker_Set()
        {
            const string setName = "EvilMinds";

            BadRequestException exception = await Assert.ThrowsAsync<BadRequestException>(
                async () => await _fixture.BotClient.SetChatStickerSetAsync(_classFixture.Chat.Id, setName)
            );

            // ToDo: Create exception type
            Assert.Equal(400, exception.ErrorCode);
            Assert.Equal("Bad Request: method is available only for supergroups", exception.Message);
        }
    }
}
