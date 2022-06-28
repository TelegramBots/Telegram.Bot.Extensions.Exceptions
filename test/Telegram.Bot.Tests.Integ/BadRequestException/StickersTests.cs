using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Tests.Integ.Framework;
using Telegram.Bot.Types;
using Xunit;

namespace Telegram.Bot.Tests.Integ.Stickers
{
    [Collection(Constants.TestCollections.BadRequestException)]
    [Trait(Constants.CategoryTraitName, Constants.InteractiveCategoryValue)]
    [TestCaseOrderer(Constants.TestCaseOrderer, Constants.AssemblyName)]
    public class BadRequestException_StickersTests : IClassFixture<StickersTestsFixture>
    {
        private ITelegramBotClient BotClient => _fixture.BotClient;

        private readonly StickersTestsFixture _classFixture;

        private readonly TestsFixture _fixture;

        public BadRequestException_StickersTests(TestsFixture fixture, StickersTestsFixture classFixture)
        {
            _classFixture = classFixture;
            _fixture = fixture;
        }

        [OrderedFact("Should throw " + nameof(InvalidStickerSetNameException) +
                     " while trying to create sticker set with name not ending in _by_<bot username>")]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.CreateNewStickerSet)]
        public async Task Should_Throw_InvalidStickerSetNameException()
        {
            InvalidStickerSetNameException exception = await Assert.ThrowsAsync<InvalidStickerSetNameException>(
                async () => await BotClient.CreateNewStaticStickerSetAsync(
                    userId: _classFixture.OwnerUserId,
                    name: "Invalid_Sticker_Set_Name",
                    title: "Sticker Set Title",
                    pngSticker: _classFixture.UploadedStickers.First().FileId,
                    emojis: "😀"
                )
            );

            Assert.Equal(400, exception.ErrorCode);
            Assert.Contains("invalid sticker set name is specified", exception.Message);
        }

        [OrderedFact("Should throw " + nameof(InvalidStickerEmojisException) +
                     " while trying to create sticker with invalid emoji")]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.CreateNewStickerSet)]
        public async Task Should_Throw_InvalidStickerEmojisException()
        {
            InvalidStickerEmojisException exception = await Assert.ThrowsAsync<InvalidStickerEmojisException>(
                async () => await BotClient.CreateNewStaticStickerSetAsync(
                    userId: _classFixture.OwnerUserId,
                    name: "valid_name3" + _classFixture.TestStickerSetName,
                    title: "Sticker Set Title",
                    pngSticker: _classFixture.UploadedStickers.First().FileId,
                    emojis: "INVALID"
                )
            );

            Assert.Equal(400, exception.ErrorCode);
            Assert.Contains("invalid sticker emojis", exception.Message);
        }

        [OrderedFact("Should throw " + nameof(InvalidStickerDimensionsException) +
                     " while trying to create sticker with invalid dimensions")]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.CreateNewStickerSet)]
        public async Task Should_Throw_InvalidStickerDimensionsException()
        {
            // ToDo exception when sending jpeg file. Bad Request: STICKER_PNG_NOPNG
            await using System.IO.Stream stream = System.IO.File.OpenRead(Constants.PathToFile.Photos.Logo);
            InvalidStickerDimensionsException exception = await Assert.ThrowsAsync<InvalidStickerDimensionsException>(
                async () => await BotClient.CreateNewStaticStickerSetAsync(
                    userId: _classFixture.OwnerUserId,
                    name: "valid_name2" + _classFixture.TestStickerSetName,
                    title: "Sticker Set Title",
                    pngSticker: stream,
                    emojis: "😁"
                )
            );

            Assert.Equal(400, exception.ErrorCode);
            Assert.Contains("STICKER_PNG_DIMENSIONS", exception.Message);
        }

        [OrderedFact("Should throw " + nameof(StickerSetNameExistsException) +
                     " while trying to create sticker set with the same name")]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.CreateNewStickerSet)]
        public async Task Should_Throw_StickerSetNameExistsException()
        {
            await using System.IO.Stream stream = System.IO.File.OpenRead(Constants.PathToFile.Photos.Ruby);
            StickerSetNameExistsException exception = await Assert.ThrowsAsync<StickerSetNameExistsException>(
                async () => await BotClient.CreateNewStaticStickerSetAsync(
                    userId: _classFixture.OwnerUserId,
                    name: _classFixture.TestStickerSetName,
                    title: "Another Test Sticker Set",
                    pngSticker: stream,
                    emojis: "😎"
                )
            );

            Assert.Equal(400, exception.ErrorCode);
            Assert.Contains("sticker set name is already occupied", exception.Message);
        }

        [OrderedFact("Should throw StickerSetNotModifiedException while trying to remove the last sticker in the set")]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.DeleteStickerFromSet)]
        public async Task Should_Throw_StickerSetNotModifiedException()
        {
            StickerSet testStickerSet = await BotClient.GetStickerSetAsync(_classFixture.TestStickerSetName);
            Sticker stickerToRemove = testStickerSet.Stickers[0];

            StickerSetNotModifiedException exception = await Assert.ThrowsAsync<StickerSetNotModifiedException>(
                async () =>
                {
                    //foreach (Sticker sticker in testStickerSet.Stickers)
                    //{
                    //    await BotClient.DeleteStickerFromSetAsync(sticker.FileId);
                    //}

                    //foreach (Sticker sticker in testStickerSet.Stickers)
                    //{
                    //    await BotClient.DeleteStickerFromSetAsync(sticker.FileId);
                    //}

                    await BotClient.DeleteStickerFromSetAsync(stickerToRemove.FileId);
                    await BotClient.DeleteStickerFromSetAsync(stickerToRemove.FileId);
                }
            );

            Assert.Equal(400, exception.ErrorCode);
            Assert.Contains("STICKERSET_NOT_MODIFIED", exception.Message);
        }
    }
}
