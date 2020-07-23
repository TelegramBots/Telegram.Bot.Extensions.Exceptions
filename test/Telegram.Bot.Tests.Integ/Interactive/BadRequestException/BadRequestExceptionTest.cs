using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Tests.Integ.Framework;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.Payments;
using Xunit;

namespace Telegram.Bot.Tests.Integ.Interactive.BadRequest
{
    [Collection(Constants.TestCollections.BadRequestException)]
    [Trait(Constants.CategoryTraitName, Constants.InteractiveCategoryValue)]
    [TestCaseOrderer(Constants.TestCaseOrderer, Constants.AssemblyName)]
    public class Interactive_BadRequestExceptionTests :
        IClassFixture<Interactive_BadRequestExceptionTests.PrivateChatFixture>,
        IClassFixture<Interactive_BadRequestExceptionTests.PaymentFixture>
    {
        private PrivateChatFixture Fixture { get; }

        private TestsFixture TestsFixture { get; }

        private PaymentFixture _paymentFixture { get; }

        private ITelegramBotClient BotClient => TestsFixture.BotClient;

        public Interactive_BadRequestExceptionTests(
            TestsFixture testsFixture,
            PrivateChatFixture classFixture,
            PaymentFixture paymentFixture)
        {
            TestsFixture = testsFixture;
            Fixture = classFixture;
            _paymentFixture = paymentFixture;
        }

        [OrderedFact("Should throw BadRequestException while trying to leave a private chat")]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.LeaveChat)]
        public async Task Should_Throw_Exception_While_Leaving_Private_Chat()
        {
            BadRequestException exception = await Assert.ThrowsAsync<BadRequestException>(
                async () => await BotClient.LeaveChatAsync(chatId: Fixture.PrivateChat)
            );

            Assert.Equal(400, exception.ErrorCode);
            Assert.Contains("chat member status can't be changed in private chats", exception.Message);
        }

        [OrderedFact("Should throw InvalidQueryIdException when answering an inline query after 10 seconds")]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.AnswerInlineQuery)]
        public async Task Should_Throw_Exception_When_Answering_Late()
        {
            await TestsFixture.SendTestInstructionsAsync(
                "Write an inline query that I'll never answer!",
                startInlineQuery: true
            );

            Update queryUpdate = await TestsFixture.UpdateReceiver.GetInlineQueryUpdateAsync();

            InlineQueryResultBase[] results =
            {
                new InlineQueryResultArticle(
                    id: "article:bot-api",
                    title: "Telegram Bot API",
                    inputMessageContent: new InputTextMessageContent("https://core.telegram.org/bots/api"))
                {
                    Description = "The Bot API is an HTTP-based interface created for developers",
                },
            };

            await Task.Delay(10_000);

            InvalidQueryIdException exception = await Assert.ThrowsAsync<InvalidQueryIdException>(
                async () => await BotClient.AnswerInlineQueryAsync(
                    inlineQueryId: queryUpdate.InlineQuery!.Id,
                    results: results,
                    cacheTime: 0
                )
            );

            Assert.Equal(400, exception.ErrorCode);
            Assert.Contains("query is too old and response timeout expired or query ID is invalid", exception.Message);
        }

        #region Payment exceptions tests

        [OrderedFact("Should throw BadRequestException when sending invoice with invalid provider data")]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.SendInvoice)]
        public async Task Should_Throw_When_Send_Invoice_Invalid_Provider_Data()
        {
            const string payload = "my-payload";

            LabeledPrice[] prices =
            {
                new LabeledPrice("PART_OF_PRODUCT_PRICE_1", 150),
                new LabeledPrice("PART_OF_PRODUCT_PRICE_2", 2029),
            };
            Invoice invoice = new Invoice
            {
                Title = "PRODUCT_TITLE",
                Currency = "CAD",
                StartParameter = "start_param",
                TotalAmount = prices.Sum(p => p.Amount),
                Description = "PRODUCT_DESCRIPTION",
            };

            BadRequestException exception = await Assert.ThrowsAsync<BadRequestException>(
                async () => await BotClient.SendInvoiceAsync(
                    chatId: (int)_paymentFixture.PrivateChat.Id,
                    title: invoice.Title,
                    description: invoice.Description,
                    payload: payload,
                    providerToken: _paymentFixture.PaymentProviderToken,
                    startParameter: invoice.StartParameter,
                    currency: invoice.Currency,
                    prices: prices,
                    providerData: "INVALID-JSON"
                ));

            // ToDo: Add exception
            Assert.Equal(400, exception.ErrorCode);
            Assert.Contains("DATA_JSON_INVALID", exception.Message);
        }

        [OrderedFact("Should throw BadRequestException when answering shipping query with duplicate shipping Id")]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.SendInvoice)]
        public async Task Should_Throw_When_Answer_Shipping_Query_With_Duplicate_Shipping_Id()
        {
            const string payload = "my-payload";

            LabeledPrice[] productPrices =
            {
                new LabeledPrice("PART_OF_PRODUCT_PRICE_1", 150),
                new LabeledPrice("PART_OF_PRODUCT_PRICE_2", 2029),
            };
            Invoice invoice = new Invoice
            {
                Title = "PRODUCT_TITLE",
                Currency = "USD",
                StartParameter = "start_param",
                TotalAmount = productPrices.Sum(p => p.Amount),
                Description = "PRODUCT_DESCRIPTION",
            };

            await BotClient.SendInvoiceAsync(
                chatId: (int)_paymentFixture.PrivateChat.Id,
                title: invoice.Title,
                description: invoice.Description,
                payload: payload,
                providerToken: _paymentFixture.PaymentProviderToken,
                startParameter: invoice.StartParameter,
                currency: invoice.Currency,
                prices: productPrices,
                isFlexible: true
            );

            LabeledPrice[] shippingPrices =
            {
                new LabeledPrice("PART_OF_SHIPPING_TOTAL_PRICE_1", 500),
                new LabeledPrice("PART_OF_SHIPPING_TOTAL_PRICE_2", 299),
            };

            ShippingOption shippingOption = new ShippingOption
            {
                Id = "option1",
                Title = "OPTION-1",
                Prices = shippingPrices,
            };

            Update shippingUpdate = await GetShippingQueryUpdate();

            BadRequestException exception = await Assert.ThrowsAsync<BadRequestException>(
                async () => await BotClient.AnswerShippingQueryAsync(
                    shippingQueryId: shippingUpdate.ShippingQuery!.Id,
                    shippingOptions: new[] { shippingOption, shippingOption }
                )
            );

            // ToDo: Add exception
            Assert.Equal(400, exception.ErrorCode);
            Assert.Contains("SHIPPING_ID_DUPLICATE", exception.Message);

            await BotClient.AnswerShippingQueryAsync(
                shippingQueryId: shippingUpdate.ShippingQuery!.Id,
                errorMessage: "✅ Test Passed"
            );
        }

        private async Task<Update> GetShippingQueryUpdate(CancellationToken cancellationToken = default)
        {
            Update[] updates = await TestsFixture.UpdateReceiver.GetUpdatesAsync(
                cancellationToken: cancellationToken,
                updateTypes: UpdateType.ShippingQuery);

            Update update = updates.Single();

            await TestsFixture.UpdateReceiver.DiscardNewUpdatesAsync(cancellationToken);

            return update;
        }

        public class PaymentFixture : Framework.Fixtures.PrivateChatFixture
        {
            public string PaymentProviderToken { get; }

            public PaymentFixture(TestsFixture testsFixture)
                : base(testsFixture, Constants.TestCollections.BadRequestException)
            {
                PaymentProviderToken = ConfigurationProvider.TestConfigurations.PaymentProviderToken;
                if (PaymentProviderToken is null)
                {
                    throw new ArgumentNullException(nameof(PaymentProviderToken));
                }

                if (PaymentProviderToken.Length < 5)
                {
                    throw new ArgumentException("Payment provider token is invalid", nameof(PaymentProviderToken));
                }
            }
        }

        #endregion

        public class PrivateChatFixture : Framework.Fixtures.PrivateChatFixture
        {
            public PrivateChatFixture(TestsFixture testsFixture)
                : base(testsFixture, Constants.TestCollections.BadRequestException)
            {
            }
        }
    }
}
