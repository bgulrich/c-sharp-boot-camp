using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using System.Linq;

namespace ConcurrentCollectionsTests
{
    public class ConcurrentBagsShould
    {
        #region Helpers
        private class AsyncStepper
        {
            private readonly TaskCompletionSource<object> _tcs = new TaskCompletionSource<object>();

            public async Task WaitAsync()
            {
                await _tcs.Task;
            }

            public void Step()
            {
                _tcs.SetResult(null);
            }
        }

        #endregion

        [Fact]
        public async Task BehaveInALifoWayWhenAccessedFromASingleThread()
        {
            var bookBag = new ConcurrentBag<string>();

            var t1 = Task.Run(() =>
            {
                bookBag.Add("War and Peace");
                bookBag.Add("The Cat in the Hat");
                bookBag.Add("The Lion, the Witch, and the Wardrobe");

                Assert.True(bookBag.TryTake(out var first));
                Assert.Equal("The Lion, the Witch, and the Wardrobe", first);

                Assert.True(bookBag.TryTake(out var second));
                Assert.Equal("The Cat in the Hat", second);

                Assert.True(bookBag.TryTake(out var third));
                Assert.Equal("War and Peace", third);
            });

            var t2 = Task.Run(() =>
            {
                bookBag.Add("The Grapes of Wrath");
                bookBag.Add("Charlie and the Chocolate Factory");
                bookBag.Add("Pilgrim's Progress");

                Assert.True(bookBag.TryTake(out var first));
                Assert.Equal("Pilgrim's Progress", first);

                Assert.True(bookBag.TryTake(out var second));
                Assert.Equal("Charlie and the Chocolate Factory", second);

                Assert.True(bookBag.TryTake(out var third));
                Assert.Equal("The Grapes of Wrath", third);
            });

            await Task.WhenAll(t1, t2);
        }

        [Fact]
        public async Task TakeBooksByStealingIfThreadIsStarved()
        {
            var bookBag = new ConcurrentBag<string>();

            AsyncStepper s1 = new AsyncStepper(), s2 = new AsyncStepper(), s3 = new AsyncStepper(), s4 = new AsyncStepper();

            var orderedTakes = new List<string>();

            // Sequence => 
            // s1 t1: Add "WAP", Add "TCITH", Add "LWW",
            // s2 t2: Remove "LWW", Remove "TCITH", Add "GW", Add "CCF", Add "PP"
            // s3 t1: Remove "WAP", Remove "PP", Remove "CCF"
            // s4 t2: Remove "GW"

            var t1 = Task.Run(async () =>
            {
                // s1 - add 3 books
                bookBag.Add("War and Peace");
                bookBag.Add("The Cat in the Hat");
                bookBag.Add("The Lion, the Witch, and the Wardrobe");

                // s1 complete
                s1.Step();

                // wait for t2 to take 2 books (both stolen) then add 3 books
                await s2.WaitAsync();

                // s3 - take 3 books (one from this thread, 2 stolen)
                Assert.True(bookBag.TryTake(out var third));
                orderedTakes.Add(third);

                Assert.True(bookBag.TryTake(out var fourth));
                orderedTakes.Add(fourth);

                Assert.True(bookBag.TryTake(out var fifth));
                orderedTakes.Add(fifth);

                // s3 complete
                s3.Step();

                await s4.WaitAsync();
            });

            var t2 = Task.Run(async () =>
            {
                // wait for t1 to add 3 books
                await s1.WaitAsync();

                // s2 - steal 2 books from t1, then add 3 books
                Assert.True(bookBag.TryTake(out var first));
                orderedTakes.Add(first);

                Assert.True(bookBag.TryTake(out var second));
                orderedTakes.Add(second);

                bookBag.Add("The Grapes of Wrath");
                bookBag.Add("Charlie and the Chocolate Factory");
                bookBag.Add("Pilgrim's Progress");

                // s2 complete
                s2.Step();

                // wait for t1 to take 3 books
                await s3.WaitAsync();

                // s4 - take last book
                Assert.True(bookBag.TryTake(out var sixth));
                orderedTakes.Add(sixth);

                Assert.False(bookBag.TryTake(out _));

                // s4 complete
                s4.Step();
            });

            await Task.WhenAll(t1, t2);

            var fifoSequence = new[] {"War and Peace", "The Cat in the Hat", "The Lion, the Witch, and the Wardrobe", "The Grapes of Wrath", "Charlie and the Chocolate Factory", "Pilgrim's Progress" };
            var lifoSequence = (string[])fifoSequence.Clone();
            Array.Reverse(lifoSequence);

            // verify the takes consists of the same items
            Assert.Equal(6, orderedTakes.Count);
            Assert.True(!orderedTakes.Except(fifoSequence).Any());

            // sequence is not fifo or lifo version of books
            Assert.False(orderedTakes.SequenceEqual(fifoSequence));
            Assert.False(orderedTakes.SequenceEqual(lifoSequence));
        }

        [Fact]
        public void BePeekable()
        {
            var bookBag = new ConcurrentBag<string>();

            bookBag.Add("War and Peace");
            bookBag.Add("The Cat in the Hat");
            bookBag.Add("The Lion, the Witch, and the Wardrobe");

            Assert.True(bookBag.TryPeek(out var first));
            Assert.Equal("The Lion, the Witch, and the Wardrobe", first);
            Assert.True(bookBag.TryTake(out first));
            Assert.Equal("The Lion, the Witch, and the Wardrobe", first);

            Assert.True(bookBag.TryPeek(out var second));
            Assert.Equal("The Cat in the Hat", second);
            Assert.True(bookBag.TryTake(out second));
            Assert.Equal("The Cat in the Hat", second);

            Assert.True(bookBag.TryPeek(out var third));
            Assert.Equal("War and Peace", third);
            Assert.True(bookBag.TryTake(out third));
            Assert.Equal("War and Peace", third);
        }

        [Fact]
        public void ReturnFalseIfExaminedWhileEmpty()
        {
            var bookBag = new ConcurrentBag<string>();

            bookBag.Add("War and Peace");
            bookBag.Add("The Cat in the Hat");
            bookBag.Add("The Lion, the Witch, and the Wardrobe");

            bookBag.TryTake(out _);
            bookBag.TryTake(out _);
            bookBag.TryTake(out _);

            // Use TryTake/Peek to check if you don't want to catch an exception
            Assert.False(bookBag.TryTake(out _));
            Assert.False(bookBag.TryPeek(out _));
        }

        [Fact]
        public void ImplementNonDestructiveIEnumerable()
        {
            var bookBag = new ConcurrentBag<string>();

            bookBag.Add("War and Peace");
            bookBag.Add("The Cat in the Hat");
            bookBag.Add("The Lion, the Witch, and the Wardrobe");

            var count = 0;

            // iterate through the entire bag
            foreach (var b in bookBag)
            {
                ++count;
            }

            Assert.Equal(3, count);

            // the collection remains unchanged (foreach does not pop)
            Assert.True(bookBag.TryTake(out var first));
            Assert.Equal("The Lion, the Witch, and the Wardrobe", first);

            Assert.True(bookBag.TryTake(out var second));
            Assert.Equal("The Cat in the Hat", second);

            Assert.True(bookBag.TryTake(out var third));
            Assert.Equal("War and Peace", third);
        }
    }
}
