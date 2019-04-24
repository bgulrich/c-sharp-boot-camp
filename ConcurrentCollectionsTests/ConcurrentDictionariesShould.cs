using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ConcurrentCollectionsTests
{
    public class ConcurrentDictionariesShould
    {
        #region AddOrUpdate

        [Fact]
        public void AddOrUpdateTheItemAndReturnItsUpdatedValueWhenAddOrUpdateIsCalled()
        {
            var items = new ConcurrentDictionary<string, int>();

            // Add
            var value = items.AddOrUpdate("some item", 1, (k, v) => v + 1);

            Assert.Equal(1, items["some item"]);
            Assert.Equal(1, value);

            // Update
            value = items.AddOrUpdate("some item", 1, (k, v) => v + 1);

            Assert.Equal(2, items["some item"]);
            Assert.Equal(2, value);
        }


        [Fact]
        public async Task InvokeDelegateMultipleTimesForSingleAddOrUpdateCallsInMultithreadedScenarios()
        {
            var items = new ConcurrentDictionary<string, int>();

            var addFactoryInvocations = 0;
            var updateFactoryInvocations = 0;

            void Worker()
            {
                for (var i = 0; i < 500; ++i)
                {
                    // dictionary will internally call TryAdd in a while loop until successful
                    // which should add additional iterations to our updater if threads are contending
                    items.AddOrUpdate("some item",
                        (k) =>
                        {
                            // increment shared counter atomically (closure)
                            Interlocked.Increment(ref addFactoryInvocations);
                            return 1;
                        },
                        (k, v) =>
                        {
                            // increment shared counter atomically (closure)
                            Interlocked.Increment(ref updateFactoryInvocations);
                            return v + 1;
                        });
                }
            }

            var t1 = Task.Run(Worker);
            var t2 = Task.Run(Worker);
            var t3 = Task.Run(Worker);
            var t4 = Task.Run(Worker);

            await Task.WhenAll(t1, t2, t3, t4);

            Assert.Equal(2000, items["some item"]);

            // at least one but no more than 4 add invocations
            Assert.True(addFactoryInvocations > 0);
            Assert.True(addFactoryInvocations <= 4);

            // more than 2000 update invocations
            Assert.True(updateFactoryInvocations > 2000);

            // => this shows that only the required delegate is executed (not both) on each AddOrUpdate attempt internally
            // so utilizing a closure to capture extra data from AddOrUpdate is acceptable
        }

        #endregion

        #region GetOrAdd

        [Fact]
        public void GetOrAddTheItemAndReturnItsUpdatedValueWhenCallingGetOrAdd()
        {
            var items = new ConcurrentDictionary<string, int>();

            // Add
            var value = items.GetOrAdd("some item", 0);

            Assert.Equal(0, items["some item"]);
            Assert.Equal(0, value);

            // Update
            items["some item"] = 5;

            // Get
            value = items.GetOrAdd("some item", 1);

            Assert.Equal(5, value);
        }


        #endregion

        #region Example

        #region Helpers
        private enum ItemType
        {
            SkinnyJeans = 0,
            Latte = 1,
            MacbookPro = 2,
            Scarf = 3,
            Beanie = 4
        }

        private class Shipment
        {
            public ItemType Item { get; set; }
            public int Quantity { get; set; }
        }

        private class AttemptedPurchase
        {
            public ItemType Item { get; set; }
            public int DesiredQuantity { get; set; }
            public int ReceivedQuantity { get; set; }

            public override string ToString() => DesiredQuantity == ReceivedQuantity ? "OK" : $"Item: {Item}, Desired: {DesiredQuantity}, Received: {ReceivedQuantity}";
        }
        #endregion

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(5)]
        public async Task HandleConcurrentUpdates(int taskMultiplier)
        {
            // 1/6 ratio of shipment to purchaser tasks gives =>
            // 1 shipment task x 10 seconds / ~50 ms x 25 items = ~5000 items added
            // 6 purchaser tasks x 10 seconds / ~25 ms x 2.5 items = ~ 6000 items attempted to be purchased
            // expecting some lost and diminished sales with worst case scenario around 20%
            const int shipmentToPurchaseMultiplier = 6;

            var hipsterInventory = new ConcurrentDictionary<ItemType, int>();

            var shipments = new ConcurrentQueue<Shipment>();
            var attemptedPurchases = new ConcurrentQueue<AttemptedPurchase>();
            var tasks = new List<Task>();

            var random = new Random();

            // cancel all tasks after 10 seconds
            var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));

            // receiving hipster merch shipments
            async Task ShipmentLoop()
            {
                // loop until canceled
                while (true)
                {
                    // get a random item and quantity (10-40) in each shipment
                    var item = (ItemType)random.Next(0, 5);
                    var shipmentQuantity = random.Next(10, 41);

                    // update inventory
                    hipsterInventory.AddOrUpdate(item, shipmentQuantity, (key, existing) => existing + shipmentQuantity);

                    // create shipment record
                    shipments.Enqueue(new Shipment { Item = item, Quantity = shipmentQuantity });

                    // wait a bit before the next shipment
                    await Task.Delay(random.Next(0, 100), cts.Token);
                }
            }

            // customers buy our hipster merch
            async Task CustomerLoop()
            {
                // delay purchases for 100 milliseconds so we can build up some inventory
                await Task.Delay(TimeSpan.FromMilliseconds(100));

                // loop until canceled
                while (true)
                {
                    // random item and desired quantity (1-4) to purchase
                    var item = (ItemType)random.Next(0, 5);
                    var desiredQuantity = random.Next(1, 5);

                    var receivedQuantity = -1;

                    // update inventory after purchase
                    hipsterInventory.AddOrUpdate(item,
                        // key did not exist, create it with a zero value and set received quantity to zero
                        (key) =>
                        {
                            // set closure variable for later consumption
                            receivedQuantity = 0;
                            return 0;
                        },
                        // key exists, update using desired quantity
                        (key, existing) =>
                        {
                            // set closure variable for later consumption
                            // how many can we receive? => minimum of desired and existing
                            receivedQuantity = Math.Min(existing, desiredQuantity);
                            return existing - receivedQuantity;
                        });

                    // create purchase record
                    attemptedPurchases.Enqueue(new AttemptedPurchase {Item = item, DesiredQuantity = desiredQuantity, ReceivedQuantity = receivedQuantity});

                    // wait a bit before the next purchase
                    await Task.Delay(random.Next(0, 50), cts.Token);
                }
            }

            // spin up shipments
            for (var i = 0; i < taskMultiplier; ++i)
            {
                tasks.Add(Task.Run(ShipmentLoop, cts.Token));
            }

            // spin up purchasers
            for (var i = 0; i < taskMultiplier * shipmentToPurchaseMultiplier; ++i)
            {
                tasks.Add(Task.Run(CustomerLoop, cts.Token));
            }
      
            // wait for everything to complete
            await Assert.ThrowsAnyAsync<OperationCanceledException>(() => Task.WhenAll(tasks));

            // total people who at least tried to buy something
            var totalAttemptedPurchases = 0;
            // total people who actually did buy something
            var totalPurchases = 0;
            // we had no inventory of the item they wanted
            var lostSales = 0;
            // we sold them something but couldn't supply all they wanted to buy
            var diminishedSales = 0;
            // total sold items
            var totalSoldItems = 0;
            // total receivedItems
            var totalReceivedItems = 0;
            // total inventory items
            var totalInventory = hipsterInventory.Values.Sum();

            // calculate statistics
            while (attemptedPurchases.TryDequeue(out AttemptedPurchase attemptedPurchase))
            {
                ++totalAttemptedPurchases;

                if (attemptedPurchase.ReceivedQuantity > 0)
                {
                    ++totalPurchases;

                    if (attemptedPurchase.DesiredQuantity != attemptedPurchase.ReceivedQuantity)
                        ++diminishedSales;

                    totalSoldItems += attemptedPurchase.ReceivedQuantity;
                }
                else
                    ++lostSales;
            }

            while (shipments.TryDequeue(out var shipment))
            {
                totalReceivedItems += shipment.Quantity;
            }

            var lostOrDiminishedSalesRatio = (lostSales + diminishedSales) / (totalAttemptedPurchases * 1.0d);
            Assert.InRange(lostOrDiminishedSalesRatio, 0.01, 20);
            // make sure we accounted for every item
            Assert.Equal(totalReceivedItems, totalSoldItems + totalInventory);
        }
        #endregion
    }
}
