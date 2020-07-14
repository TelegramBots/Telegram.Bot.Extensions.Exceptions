using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Telegram.Bot.Tests.Integ.Framework
{
    // ReSharper disable once UnusedMember.Global
    public class TestCollectionOrderer : ITestCollectionOrderer
    {
        private readonly string[] _orderedCollections =
        {
            // Tests that require user interaction:

            // Tests without the need for user interaction:
            Constants.TestCollections.NotFoundException,
            Constants.TestCollections.BadRequest,

            // ToDO
            Constants.TestCollections.Payment,
            Constants.TestCollections.Stickers,
            Constants.TestCollections.ChannelAdminBots,
            Constants.TestCollections.ChatMemberAdministration,
        };

        public IEnumerable<ITestCollection> OrderTestCollections(IEnumerable<ITestCollection> testCollections)
        {
            testCollections = testCollections.OrderBy(FindExecutionOrder);

            foreach (var collection in testCollections)
            {
                yield return collection;
            }
        }

        private int FindExecutionOrder(ITestCollection collection)
        {
            int? order = null;
            for (int i = 0; i < _orderedCollections.Length; i++)
            {
                if (_orderedCollections[i] == collection.DisplayName)
                {
                    order = i;
                    break;
                }
            }

            if (order is null)
            {
                throw new ArgumentException(
                    $"Collection \"{collection.DisplayName}\" not found in execution list.",
                    nameof(collection)
                );
            }

            return (int)order;
        }
    }
}
