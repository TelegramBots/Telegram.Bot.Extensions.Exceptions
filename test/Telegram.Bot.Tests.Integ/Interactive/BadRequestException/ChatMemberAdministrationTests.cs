using System.Threading.Tasks;
using Telegram.Bot.Tests.Integ.Framework;
using Xunit;

namespace Telegram.Bot.Tests.Integ.Interactive.BadRequest
{
    [Collection(Constants.TestCollections.BadRequestException)]
    [Trait(Constants.CategoryTraitName, Constants.InteractiveCategoryValue)]
    [TestCaseOrderer(Constants.TestCaseOrderer, Constants.AssemblyName)]
    public class InteractiveChatMemberAdministrationTests : IClassFixture<ChatMemberAdministrationTestFixture>
    {
        //private ITelegramBotClient BotClient => _fixture.BotClient;

        private readonly TestsFixture _fixture;

        private readonly ChatMemberAdministrationTestFixture _classFixture;

        public InteractiveChatMemberAdministrationTests(TestsFixture fixture, ChatMemberAdministrationTestFixture classFixture)
        {
            _fixture = fixture;
            _classFixture = classFixture;
        }

        //[OrderedFact("Should promote chat member to change chat information")]
        //[Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.PromoteChatMember)]
        //public async Task Should_Promote_User_To_Change_Chat_Info()
        //{

            //ToDo exception when user isn't in group. Bad Request: bots can't add new chat members

        //    await BotClient.PromoteChatMemberAsync(
        //        chatId: _fixture.SupergroupChat.Id,
        //        userId: _classFixture.RegularMemberUserId,
        //        canChangeInfo: true
        //    );
        //}

        //[OrderedFact("Should demote chat member by taking his/her only admin right: change_info")]
        //[Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.PromoteChatMember)]
        //public async Task Should_Demote_User()
        //{

            //ToDo exception when user isn't in group. Bad Request: USER_NOT_MUTUAL_CONTACT

        //    await BotClient.PromoteChatMemberAsync(
        //        chatId: _fixture.SupergroupChat.Id,
        //        userId: _classFixture.RegularMemberUserId,
        //        canChangeInfo: false
        //    );
        //}
    }
}
