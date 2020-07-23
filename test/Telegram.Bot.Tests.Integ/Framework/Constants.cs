using Telegram.Bot.Tests.Integ.Framework.XunitExtensions;

namespace Telegram.Bot.Tests.Integ.Framework
{
    public static class Constants
    {
        public const string CategoryTraitName = "Category";

        public const string InteractiveCategoryValue = "Interactive";

        public const string MethodTraitName = "Method";

        public const string AssemblyName = "Telegram.Bot.Tests.Integ";

        public const string TestCaseOrderer =
            AssemblyName + "." + nameof(Framework) + "." + nameof(Framework.TestCaseOrderer);

        public const string TestCaseDiscoverer =
            AssemblyName + "." + nameof(Framework) + "." + nameof(XunitExtensions) + "." + nameof(RetryFactDiscoverer);

        public static class TestCollections
        {
            public const string BadRequestException = "BadRequestException";
            public const string ForbiddenException = "ForbiddenException";
            public const string NotFoundException = "NotFoundException";
            public const string TooManyRequestsException = "TooManyRequestsException";
            public const string UnauthorizedException = "UnauthorizedException";
        }

        public static class PathToFile
        {
            private const string FilesDir = "Files/";

            public static class Photos
            {
                private const string PhotoDir = FilesDir + "Photo/";

                public const string Logo = PhotoDir + "logo.png";

                public const string Vlc = PhotoDir + "vlc.png";

                public const string Ruby = PhotoDir + "ruby.png";
            }
        }

        public static class TelegramBotApiMethods
        {
            public const string GetMe = "getMe";

            public const string SendMessage = "sendMessage";

            public const string ForwardMessage = "forwardMessage";

            public const string AnswerCallbackQuery = "answerCallbackQuery";

            public const string AnswerInlineQuery = "answerInlineQuery";

            public const string SendInvoice = "sendInvoice";

            public const string AnswerShippingQuery = "answerShippingQuery";

            public const string AnswerPreCheckoutQuery = "answerPreCheckoutQuery";

            public const string SetChatTitle = "setChatTitle";

            public const string SetChatDescription = "setChatDescription";

            public const string SetChatPermissions = "setChatPermissions";

            public const string SetChatAdministratorCustomTitle = "setChatAdministratorCustomTitle";

            public const string ExportChatInviteLink = "exportChatInviteLink";

            public const string PinChatMessage = "pinChatMessage";

            public const string GetChat = "getChat";

            public const string LeaveChat = "leaveChat";

            public const string GetUserProfilePhotos = "getUserProfilePhotos";

            public const string GetChatMember = "getChatMember";

            public const string GetChatAdministrators = "getChatAdministrators";

            public const string GetChatMembersCount = "getChatMembersCount";

            public const string SendChatAction = "sendChatAction";

            public const string GetFile = "getFile";

            public const string UnpinChatMessage = "unpinChatMessage";

            public const string SetChatPhoto = "setChatPhoto";

            public const string DeleteChatPhoto = "deleteChatPhoto";

            public const string KickChatMember = "kickChatMember";

            public const string UnbanChatMember = "unbanChatMember";

            public const string RestrictChatMember = "restrictChatMember";

            public const string PromoteChatMember = "promoteChatMember";

            public const string GetStickerSet = "getStickerSet";

            public const string SendPhoto = "sendPhoto";

            public const string SendVideo = "sendVideo";

            public const string SendAnimation = "sendAnimation";

            public const string SendAudio = "sendAudio";

            public const string SendVenue = "sendVenue";

            public const string SendVoice = "sendVoice";

            public const string SendVideoNote = "sendVideoNote";

            public const string SendDocument = "sendDocument";

            public const string SendContact = "sendContact";

            public const string EditMessageText = "editMessageText";

            public const string EditMessageMedia = "editMessageMedia";

            public const string EditMessageReplyMarkup = "editMessageReplyMarkup";

            public const string EditMessageCaption = "editMessageCaption";

            public const string DeleteMessage = "deleteMessage";

            public const string SendLocation = "sendLocation";

            public const string EditMessageLiveLocation = "editMessageLiveLocation";

            public const string StopMessageLiveLocation = "stopMessageLiveLocation";

            public const string SetChatStickerSet = "setChatStickerSet";

            public const string SendSticker = "sendSticker";

            public const string SendMediaGroup = "sendMediaGroup";

            public const string UploadStickerFile = "uploadStickerFile";

            public const string CreateNewStickerSet = "createNewStickerSet";

            public const string AddStickerToSet = "addStickerToSet";

            public const string SetStickerPositionInSet = "setStickerPositionInSet";

            public const string DeleteStickerFromSet = "deleteStickerFromSet";

            public const string SendGame = "sendGame";

            public const string SetGameScore = "setGameScore";

            public const string GetGameHighScores = "getGameHighScores";

            public const string SetWebhook = "setWebhook";

            public const string DeleteWebhook = "deleteWebhook";

            public const string GetWebhookInfo = "getWebhookInfo";

            public const string SendPoll = "sendPoll";

            public const string StopPoll = "stopPoll";

            public const string SetMyCommands = "setMyCommands";

            public const string GetMyCommands = "getMyCommands";

            public const string SendDice = "sendDice";
        }
    }
}
