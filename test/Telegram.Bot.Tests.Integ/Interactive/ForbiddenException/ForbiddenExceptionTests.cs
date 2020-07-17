using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Tests.Integ.Framework;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Xunit;

namespace Telegram.Bot.Tests.Integ.Interactive.Forbidden
{
    [Collection(Constants.TestCollections.ForbiddenException)]
    [Trait(Constants.CategoryTraitName, Constants.InteractiveCategoryValue)]
    [TestCaseOrderer(Constants.TestCaseOrderer, Constants.AssemblyName)]
    public class InteractiveForbiddenExceptionTests
    {
        private ITelegramBotClient BotClient => Fixture.BotClient;

        private TestsFixture Fixture { get; }

        public InteractiveForbiddenExceptionTests(TestsFixture fixture)
        {
            Fixture = fixture;
        }

        [OrderedFact("Should throw ChatNotInitiatedException while trying to send message to a user who hasn't " +
                     "started a chat with bot but bot knows about him/her.")]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.SendMessage)]
        public async Task Should_Throw_ChatNotInitiatedException_When_Chat_Not_Initiated()
        {
            await Fixture.SendTestInstructionsAsync(
                "Forward a message to this chat from a user that never started a chat with this bot"
            );

            Update forwardedMessageUpdate = (await Fixture.UpdateReceiver.GetUpdatesAsync(u =>
                    u.Message?.ForwardFrom != null, updateTypes: UpdateType.Message
            )).Single();
            await Fixture.UpdateReceiver.DiscardNewUpdatesAsync();

            ChatNotInitiatedException exception = await Assert.ThrowsAsync<ChatNotInitiatedException>(
                async () => await BotClient.SendTextMessageAsync(
                    forwardedMessageUpdate!.Message!.ForwardFrom!.Id,
                    $"Error! If you see this message, talk to @{forwardedMessageUpdate.Message.From!.Username}"
                )
            );

            Assert.Equal(403, exception.ErrorCode);
            Assert.Contains("bot can't initiate conversation with a user", exception.Message);
        }

        [OrderedFact("Should throw ForbiddenException while trying to send message to another bot.")]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.SendMessage)]
        public async Task Should_Throw_ForbiddenException_Sending_Message_To_Bot()
        {
            //ToDo add exception. forward message from another bot. Forbidden: bot can't send messages to bots
            await Fixture.SendTestInstructionsAsync(
                "Forward a message to this chat from another bot"
            );

            Update forwardedMessageUpdate = (await Fixture.UpdateReceiver.GetUpdatesAsync(u =>
                    u.Message?.ForwardFrom != null, updateTypes: UpdateType.Message
            )).Single();
            await Fixture.UpdateReceiver.DiscardNewUpdatesAsync();

            ForbiddenException exception = await Assert.ThrowsAsync<ForbiddenException>(
                async () => await BotClient.SendTextMessageAsync(
                    forwardedMessageUpdate!.Message!.ForwardFrom!.Id,
                    $"Error! If you see this message, talk to @{forwardedMessageUpdate.Message.From!.Username}"
                )
            );

            Assert.Equal(403, exception.ErrorCode);
            Assert.Contains("bot can't send messages to bots", exception.Message);
        }
    }
}
