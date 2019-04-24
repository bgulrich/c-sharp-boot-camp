using System;
using System.Collections.Concurrent;
using System.Text;
using Xunit;

namespace ConcurrentCollectionsTests
{
    public class ConcurrentQueuesShould
    {
        [Fact]
        public void BehaveInAFifoWay()
        {
            var toDoList = new ConcurrentQueue<string>();

            toDoList.Enqueue("clean car");
            toDoList.Enqueue("pay rent");
            toDoList.Enqueue("mow lawn");

            Assert.True(toDoList.TryDequeue(out var first));
            Assert.Equal("clean car", first);

            Assert.True(toDoList.TryDequeue(out var second));
            Assert.Equal("pay rent", second);

            Assert.True(toDoList.TryDequeue(out var third));
            Assert.Equal("mow lawn", third);
        }

        [Fact]
        public void BePeekable()
        {
            var toDoList = new ConcurrentQueue<string>();

            toDoList.Enqueue("clean car");
            toDoList.Enqueue("pay rent");
            toDoList.Enqueue("mow lawn");

            Assert.True(toDoList.TryPeek(out var first));
            Assert.Equal("clean car", first);
            Assert.True(toDoList.TryDequeue(out first));
            Assert.Equal("clean car", first);

            Assert.True(toDoList.TryPeek(out var second));
            Assert.Equal("pay rent", second);
            Assert.True(toDoList.TryDequeue(out second));
            Assert.Equal("pay rent", second);

            Assert.True(toDoList.TryPeek(out var third));
            Assert.Equal("mow lawn", third);
            Assert.True(toDoList.TryDequeue(out third));
            Assert.Equal("mow lawn", third);
        }

        [Fact]
        public void ReturnFalseIfExaminedWhileEmpty()
        {
            var toDoList = new ConcurrentQueue<string>();

            toDoList.Enqueue("clean car");
            toDoList.Enqueue("pay rent");
            toDoList.Enqueue("mow lawn");

            toDoList.TryDequeue(out _);
            toDoList.TryDequeue(out _);
            toDoList.TryDequeue(out _);

            // Use TryDequeue/Peek to check if you don't want to catch an exception
            Assert.False(toDoList.TryDequeue(out _));
            Assert.False(toDoList.TryPeek(out _));
        }

        [Fact]
        public void ImplementNonDestructiveIEnumerable()
        {
            var toDoList = new ConcurrentQueue<string>();

            toDoList.Enqueue("clean car");
            toDoList.Enqueue("pay rent");
            toDoList.Enqueue("mow lawn");

            var count = 0;

            // iterate through the entire queue
            foreach (var b in toDoList)
            {
                ++count;
            }

            Assert.Equal(3, count);

            // the collection remains unchanged (foreach does not dequeue)
            Assert.True(toDoList.TryDequeue(out var first));
            Assert.Equal("clean car", first);

            Assert.True(toDoList.TryDequeue(out var second));
            Assert.Equal("pay rent", second);

            Assert.True(toDoList.TryDequeue(out var third));
            Assert.Equal("mow lawn", third);
        }
    }
}
