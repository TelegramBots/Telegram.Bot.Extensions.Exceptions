using System.Threading.Tasks;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Tests.Integ.Framework;
using Xunit;

namespace Telegram.Bot.Tests.Integ.Admin_Bot
{
    [Collection(Constants.TestCollections.BadRequestException)]
    [TestCaseOrderer(Constants.TestCaseOrderer, Constants.AssemblyName)]
    public class SupergroupAdminBotTests : IClassFixture<SupergroupAdminBotTestsFixture>
    {
        private readonly SupergroupAdminBotTestsFixture _classFixture;

        private ITelegramBotClient BotClient => _classFixture.TestsFixture.BotClient;

        public SupergroupAdminBotTests(SupergroupAdminBotTestsFixture classFixture)
        {
            _classFixture = classFixture;
        }

        [OrderedFact("Should throw exception in deleting chat photo with no photo currently set")]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.DeleteChatPhoto)]
        public async Task Should_Throw_On_Deleting_Chat_Deleted_Photo()
        {
            BadRequestException e = await Assert.ThrowsAsync<BadRequestException>(() =>
                BotClient.DeleteChatPhotoAsync(_classFixture.Chat.Id));

            // ToDo: Create exception type
            Assert.IsType<BadRequestException>(e);
            Assert.Equal("Bad Request: CHAT_NOT_MODIFIED", e.Message);
        }

        [OrderedFact("Should throw exception when trying to set sticker set for a chat with less than 100 members")]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.SetChatStickerSet)]
        public async Task Should_Throw_On_Setting_Chat_Sticker_Set()
        {
            const string setName = "EvilMinds";

            BadRequestException exception = await Assert.ThrowsAsync<BadRequestException>(() =>
                BotClient.SetChatStickerSetAsync(_classFixture.Chat.Id, setName)
            );

            // ToDo: Create exception type
            Assert.Equal(400, exception.ErrorCode);
            Assert.Equal("Bad Request: can't set supergroup sticker set", exception.Message);
        }
    }
}
