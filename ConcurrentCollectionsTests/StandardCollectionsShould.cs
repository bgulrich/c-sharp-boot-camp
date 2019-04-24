using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ConcurrentCollectionsTests
{
    public class StandardCollectionsShould
    {
        [Fact]
        public async Task ThrowExceptionsWhenSimultaneouslyEnqueueingDataFromMultipleThreads()
        {
            var orders = new Queue<string>();

            // kick off a few threads that are placing orders
            var t1 = Task.Run(() => PlaceOrders(orders, "Hamburger", 1000000));
            var t2 = Task.Run(() => PlaceOrders(orders, "Cheeseburger", 1000000));
            var t3 = Task.Run(() => PlaceOrders(orders, "Hot dog", 1000000));
            var t4 = Task.Run(() => PlaceOrders(orders, "Salad", 1000000));
            var t5 = Task.Run(() => PlaceOrders(orders, "Chicken", 1000000));

            var ex = await Assert.ThrowsAnyAsync<Exception>(() => Task.WhenAll(new [] {t1, t2, t3, t4, t5}));
        }

        [Fact]
        public async Task TakeAPerformanceHitWhenSynchronizedExternally()
        {
            var ordersLocked = new Queue<string>();
            var ordersSequential = new Queue<string>();
            var ordersConcurrent = new ConcurrentQueue<string>();

            var lockedStart = DateTime.Now;

            // kick off a few threads that are placing orders (locked)
            var lt1  = Task.Run(() => PlaceOrdersLocked(ordersLocked, "Hamburger", 100000));
            var lt2  = Task.Run(() => PlaceOrdersLocked(ordersLocked, "Cheeseburger", 100000));
            var lt3  = Task.Run(() => PlaceOrdersLocked(ordersLocked, "Hot dog", 100000));
            var lt4  = Task.Run(() => PlaceOrdersLocked(ordersLocked, "Salad", 100000));
            var lt5  = Task.Run(() => PlaceOrdersLocked(ordersLocked, "Chicken", 100000));
            var lt6  = Task.Run(() => PlaceOrdersLocked(ordersLocked, "Steak", 100000));
            var lt7  = Task.Run(() => PlaceOrdersLocked(ordersLocked, "Fries", 100000));
            var lt8  = Task.Run(() => PlaceOrdersLocked(ordersLocked, "Soda", 100000));
            var lt9  = Task.Run(() => PlaceOrdersLocked(ordersLocked, "Soup", 100000));
            var lt10 = Task.Run(() => PlaceOrdersLocked(ordersLocked, "Cake", 100000));

            await Task.WhenAll(new[] {lt1, lt2, lt3, lt4, lt5, lt6, lt7, lt8, lt9, lt10});

            var lockedComplete = DateTime.Now;

            var sequentialStart = DateTime.Now;

            // do the same work sequentially
            PlaceOrders(ordersSequential, "Hamburger", 100000);
            PlaceOrders(ordersSequential, "Cheeseburger", 100000);
            PlaceOrders(ordersSequential, "Hot dog", 100000);
            PlaceOrders(ordersSequential, "Salad", 100000);
            PlaceOrders(ordersSequential, "Chicken", 100000);
            PlaceOrders(ordersSequential, "Steak", 100000);
            PlaceOrders(ordersSequential, "Fries", 100000);
            PlaceOrders(ordersSequential, "Soda", 100000);
            PlaceOrders(ordersSequential, "Soup", 100000);
            PlaceOrders(ordersSequential, "Cake", 100000);

            var sequentialComplete = DateTime.Now;

            var concurrentStart = DateTime.Now;

            // kick off a few threads that are placing orders (concurrent)
            var ct1  = Task.Run(() => PlaceOrdersConcurrent(ordersConcurrent, "Hamburger", 100000));
            var ct2  = Task.Run(() => PlaceOrdersConcurrent(ordersConcurrent, "Cheeseburger", 100000));
            var ct3  = Task.Run(() => PlaceOrdersConcurrent(ordersConcurrent, "Hot dog", 100000));
            var ct4  = Task.Run(() => PlaceOrdersConcurrent(ordersConcurrent, "Salad", 100000));
            var ct5  = Task.Run(() => PlaceOrdersConcurrent(ordersConcurrent, "Chicken", 100000));
            var ct6  = Task.Run(() => PlaceOrdersConcurrent(ordersConcurrent, "Steak", 100000));
            var ct7  = Task.Run(() => PlaceOrdersConcurrent(ordersConcurrent, "Fries", 100000));
            var ct8  = Task.Run(() => PlaceOrdersConcurrent(ordersConcurrent, "Soda", 100000));
            var ct9  = Task.Run(() => PlaceOrdersConcurrent(ordersConcurrent, "Soup", 100000));
            var ct10 = Task.Run(() => PlaceOrdersConcurrent(ordersConcurrent, "Cake", 100000));

            await Task.WhenAll(new[] { ct1, ct2, ct3, ct4, ct5, ct6, ct7, ct8, ct9, ct10 });

            var concurrentComplete = DateTime.Now;

            var lockedSeconds = (lockedComplete - lockedStart).TotalSeconds;
            var sequentialSeconds = (sequentialComplete - sequentialStart).TotalSeconds;
            var concurrentSeconds = (concurrentComplete - concurrentStart).TotalSeconds;

            var lockedToSequentialRatio = lockedSeconds / sequentialSeconds;
            var concurrentToSequentialRatio = concurrentSeconds / sequentialSeconds;

            // although it has more workers doing  the work,
            // locked expected to take at least 5% longer to accomplish the same work due to contention
            Assert.InRange(lockedToSequentialRatio, 1.05, 5.0);
            // concurrent expected to be faster than sequentially doing the same work
            Assert.InRange(concurrentToSequentialRatio, 0.6, 0.9);
        }

        private void PlaceOrders(Queue<string> orders, string item, int count)
        {
            for (var i = 0; i < count; ++i)
            {
                orders.Enqueue($"{item} {i}");
            }
        }

        private readonly object _queueLock = new object();

        private void PlaceOrdersLocked(Queue<string> orders, string item, int count)
        {
            for (var i = 0; i < count; ++i)
            {
                lock (_queueLock)
                {
                    orders.Enqueue($"{item} {i}");
                }
            }
        }

        private void PlaceOrdersConcurrent(ConcurrentQueue<string> orders, string item, int count)
        {
            for (var i = 0; i < count; ++i)
            {
                orders.Enqueue($"{item} {i}");
            }
        }
    }
}
