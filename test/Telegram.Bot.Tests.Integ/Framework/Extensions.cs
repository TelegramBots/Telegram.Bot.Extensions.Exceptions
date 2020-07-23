using System;
using System.Linq;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Tests.Integ.Framework
{
    internal static class Extensions
    {
        public static User GetUser(this Update update) =>
            update.Type switch
            {
                UpdateType.Message => update.Message!.From,
                UpdateType.InlineQuery => update.InlineQuery!.From,
                UpdateType.CallbackQuery => update.CallbackQuery!.From,
                UpdateType.PreCheckoutQuery => update.PreCheckoutQuery!.From,
                UpdateType.ShippingQuery => update.ShippingQuery!.From,
                UpdateType.ChosenInlineResult => update.ChosenInlineResult!.From,
                UpdateType.PollAnswer => update.PollAnswer!.User,
                _ => throw new ArgumentException("Unsupported update type {0}.", update.Type.ToString())
            };

        public static string GetTesters(this UpdateReceiver updateReceiver)
        {
            return string.Join(", ",
                updateReceiver.AllowedUsernames.Select(username => username.Replace("_", "\\_"))
            );
        }
    }
}
