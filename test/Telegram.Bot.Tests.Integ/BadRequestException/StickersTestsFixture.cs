using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Tests.Integ.Framework;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Xunit;

namespace Telegram.Bot.Tests.Integ.Stickers
{
    public class StickersTestsFixture : IAsyncLifetime
    {
        private ITelegramBotClient BotClient => _fixture.BotClient;

        private readonly TestsFixture _fixture;

        public IEnumerable<File> UploadedStickers { get; private set; }

        public string TestStickerSetName { get; }

        public long OwnerUserId { get; private set; }

        public StickersTestsFixture(TestsFixture testsFixture)
        {
            _fixture = testsFixture;

            TestStickerSetName = $"test_set_by_{testsFixture.BotUser.Username}";
        }

        public async Task InitializeAsync()
        {
            OwnerUserId = await GetStickerOwnerIdAsync(_fixture, Constants.TestCollections.BadRequestException);
            UploadedStickers = await UploadStickersAsync();

            await CreateNewStickerSet();
            await AddStickersAsynd();
        }

        private static async Task<long> GetStickerOwnerIdAsync(TestsFixture testsFixture, string collectionName)
        {
            long ownerId;

            if (ConfigurationProvider.TestConfigurations.StickerOwnerUserId == default)
            {
                await testsFixture.UpdateReceiver.DiscardNewUpdatesAsync();

                Message notifMessage = await testsFixture.SendTestCollectionNotificationAsync(collectionName,
                    $"\nNo value is set for `{nameof(ConfigurationProvider.TestConfigurations.StickerOwnerUserId)}` " +
                    "in test settings.\n\n" +
                    ""
                );

                const string cqData = "sticker_tests:owner";
                Message cqMessage = await testsFixture.BotClient.SendTextMessageAsync(
                    testsFixture.SupergroupChat,
                    testsFixture.UpdateReceiver.GetTesters() +
                    "\nUse the following button to become Sticker Set Owner",
                    replyToMessageId: notifMessage.MessageId,
                    replyMarkup: new InlineKeyboardMarkup(
                        InlineKeyboardButton.WithCallbackData("I am the Owner!", cqData)
                    )
                );

                Update cqUpdate = await testsFixture.UpdateReceiver
                    .GetCallbackQueryUpdateAsync(cqMessage.MessageId, cqData);

                ownerId = cqUpdate.CallbackQuery!.From.Id;
            }
            else
            {
                ownerId = ConfigurationProvider.TestConfigurations.StickerOwnerUserId ?? throw new ArgumentNullException(nameof(ownerId));
            }

            return ownerId;
        }

        private async Task<List<File>> UploadStickersAsync()
        {
            List<File> stickerFiles = new List<File>(2);
            foreach (string pngFile in new[] { Constants.PathToFile.Photos.Logo, Constants.PathToFile.Photos.Ruby })
            {
                await using System.IO.Stream stream = System.IO.File.OpenRead(pngFile);
                File file = await BotClient.UploadStickerFileAsync(
                    userId: OwnerUserId,
                    pngSticker: stream
                );

                stickerFiles.Add(file);
            }

            return stickerFiles;
        }

        private async Task CreateNewStickerSet()
        {
            try
            {
                await BotClient.CreateNewStickerSetAsync(
                    userId: OwnerUserId,
                    name: TestStickerSetName,
                    title: "Test Sticker Set",
                    pngSticker: UploadedStickers.First().FileId,
                    emojis: "😁"
                );
            }
            catch (StickerSetNameExistsException)
            {
                await DeleteStickersAsync();
            }


        }

        private async Task AddStickersAsynd()
        {
            await BotClient.AddStickerToSetAsync(
                userId: OwnerUserId,
                name: TestStickerSetName,
                pngSticker: UploadedStickers.Last().FileId,
                emojis: "😏😃"
            );

            System.IO.Stream stream = System.IO.File.OpenRead(Constants.PathToFile.Photos.Vlc);
            await BotClient.AddStickerToSetAsync(
                userId: OwnerUserId,
                name: TestStickerSetName,
                pngSticker: stream,
                emojis: "😇",
                maskPosition: new MaskPosition
                {
                    Point = MaskPositionPoint.Forehead,
                    Scale = .8f
                }
            );
        }

        public async Task DisposeAsync()
        {
            await DeleteStickersAsync();
        }

        private async Task DeleteStickersAsync()
        {
            StickerSet testStickerSet = await BotClient.GetStickerSetAsync(TestStickerSetName);

            foreach (Sticker sticker in testStickerSet.Stickers)
            {
                await BotClient.DeleteStickerFromSetAsync(sticker.FileId);
            }
        }
    }
}
