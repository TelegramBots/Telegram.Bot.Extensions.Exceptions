using System.Threading.Tasks;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Tests.Integ.Framework;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using Xunit;

namespace Telegram.Bot.Tests.Integ.BadRequest
{
    [Collection(Constants.TestCollections.BadRequestException)]
    [TestCaseOrderer(Constants.TestCaseOrderer, Constants.AssemblyName)]
    public class BadRequestExceptionTests
    {
        private TestsFixture Fixture { get; }
        private ITelegramBotClient BotClient => Fixture.BotClient;

        public BadRequestExceptionTests(TestsFixture testsFixture)
        {
            Fixture = testsFixture;
        }

        [OrderedFact("Should throw UserNotFoundException")]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.GetChatMember)]
        public async Task Should_Throw_UserNotFoundException()
        {
            UserNotFoundException exception = await Assert.ThrowsAsync<UserNotFoundException>(
                async () => await BotClient.GetChatMemberAsync("@testchannel111112", 100120232)
            );

            Assert.Equal(400, exception.ErrorCode);
            Assert.Contains("user not found", exception.Message);
        }

        [OrderedFact("Should throw BadRequestException")]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.GetChatMember)]
        public async Task Should_Throw_InvalidParameterException()
        {
            BadRequestException exception = await Assert.ThrowsAsync<InvalidParameterException>(
                async () => await BotClient.GetChatMemberAsync(Fixture.SupergroupChat.Id, -100120232)
            );

            Assert.Equal(400, exception.ErrorCode);
            Assert.Contains("invalid user_id specified", exception.Message);
        }

        [OrderedFact("Should throw ChatNotFoundException while trying to send message to an invalid chat")]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.SendMessage)]
        public async Task Should_Throw_ChatNotFoundException_For_Non_Existent_Chat()
        {
            ChatNotFoundException exception = await Assert.ThrowsAsync<ChatNotFoundException>(
                async () => await BotClient.SendTextMessageAsync(0, "test")
            );

            Assert.Equal(400, exception.ErrorCode);
            Assert.Contains("chat not found", exception.Message);
        }

        [OrderedFact("Should throw UserNotFoundException while trying to promote an invalid user id")]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.SendMessage)]
        public async Task Should_Throw_UserNotFoundException_For_Invalid_User_Id()
        {
            UserNotFoundException exception = await Assert.ThrowsAsync<UserNotFoundException>(
                async () => await BotClient.PromoteChatMemberAsync(Fixture.SupergroupChat.Id, 123456)
            );

            Assert.Equal(400, exception.ErrorCode);
            Assert.Contains("user not found", exception.Message);
        }

        [OrderedFact("Should throw ContactRequestException while asking for user's phone number " +
                     "in non-private chat via reply keyboard markup")]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.SendMessage)]
        public async Task Should_Throw_ContactRequestException_When_Asked_To_Share_Contact_In_Non_Private_Chat()
        {
            ReplyKeyboardMarkup replyMarkup = new ReplyKeyboardMarkup(new[]
            {
                KeyboardButton.WithRequestContact("Share Contact"),
            });

            ContactRequestException exception = await Assert.ThrowsAsync<ContactRequestException>(
                async () => await BotClient.SendTextMessageAsync(
                    Fixture.SupergroupChat.Id,
                    "You should never see this message",
                    replyMarkup: replyMarkup
                )
            );

            Assert.Equal(400, exception.ErrorCode);
            Assert.Contains("phone number can be requested in private chats only", exception.Message);
        }

        [OrderedFact("Should throw MessageIsNotModifiedException while editing previously " +
                     "sent message with the same text")]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.SendMessage)]
        public async Task Should_Throw_MessageIsNotModifiedException_When_Message_Is_Not_Modified()
        {
            const string messageTextToModify = "Message text to modify";
            Message message = await BotClient.SendTextMessageAsync(
                Fixture.SupergroupChat.Id,
                messageTextToModify);

            MessageIsNotModifiedException exception = await Assert.ThrowsAsync<MessageIsNotModifiedException>(
                async () => await BotClient.EditMessageTextAsync(
                    Fixture.SupergroupChat.Id,
                    message.MessageId,
                    messageTextToModify
                )
            );

            Assert.Equal(400, exception.ErrorCode);
            Assert.Contains("message is not modified: specified new message content and reply markup are exactly the same as a current content and reply markup of the message", exception.Message);
        }

        [OrderedFact("Should throw BadRequestException due to not having enough poll options")]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.SendPoll)]
        public async Task Should_Throw_BadRequestException_Not_Enough_Options()
        {
            BadRequestException exception = await Assert.ThrowsAsync<BadRequestException>(
                async () => await BotClient.SendPollAsync(
                    chatId: Fixture.SupergroupChat,
                    question: "You should never see this poll",
                    options: new[] { "The only poll option" }
                )
            );

            Assert.Equal(400, exception.ErrorCode);
            Assert.Contains("poll must have at least 2 option", exception.Message);
        }

        [OrderedFact("Should throw InvalidParameterException while trying to get file using wrong file_id")]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.GetFile)]
        public async Task Should_Throw_InvalidParameterException_When_Invalid_FileId()
        {
            InvalidParameterException exception = await Assert.ThrowsAsync<InvalidParameterException>(
                async () => await BotClient.GetFileAsync("Invalid_File_id")
            );

            Assert.Equal(400, exception.ErrorCode);
            Assert.Contains("invalid file_id", exception.Message);
        }

        [OrderedFact("Should throw InvalidGameShortNameException")]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.SendGame)]
        public async Task Should_Throw_InvalidGameShortNameException_When_GameShortName_Invalid()
        {
            InvalidGameShortNameException exception = await Assert.ThrowsAsync<InvalidGameShortNameException>(
                async () => await BotClient.SendGameAsync(
                    chatId: Fixture.SupergroupChat.Id,
                    gameShortName: "my game"
                )
            );

            Assert.Equal(400, exception.ErrorCode);
            Assert.Contains("GAME_SHORTNAME_INVALID", exception.Message);
        }

        [OrderedFact("Should throw InvalidGameShortNameException for empty name")]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.SendGame)]
        public async Task Should_Throw_InvalidGameShortNameException_When_GameShortName_Invalid_2()
        {
            InvalidGameShortNameException exception = await Assert.ThrowsAsync<InvalidGameShortNameException>(
                async () => await BotClient.SendGameAsync(
                    chatId: Fixture.SupergroupChat.Id,
                    gameShortName: string.Empty
                )
            );

            Assert.Equal(400, exception.ErrorCode);
            Assert.Contains("game_short_name", exception.Message);
        }

        [OrderedFact("Should throw InvalidGameShortNameException for non-existent game")]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.SendGame)]
        public async Task Should_Throw_InvalidGameShortNameException_When_GameShortName_Invalid_3()
        {
            InvalidGameShortNameException exception = await Assert.ThrowsAsync<InvalidGameShortNameException>(
                async () => await BotClient.SendGameAsync(
                    chatId: Fixture.SupergroupChat.Id,
                    gameShortName: "non_existing_game"
                )
            );

            Assert.Equal(400, exception.ErrorCode);
            Assert.Contains("wrong game short name specified", exception.Message);
        }

        // ToDo: Send game with markup & game button NOT as 1st: BUTTON_POS_INVALID
        // ToDo: Send game with markup & w/o game button: REPLY_MARKUP_GAME_EMPTY
    }
}
