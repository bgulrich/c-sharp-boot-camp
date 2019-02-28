using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace CollectionTests
{
    public class QueuesShould
    {
        [Fact]
        public void BehaveInAFifoWay()
        {
            var toDoList = new Queue<string>();

            toDoList.Enqueue("clean car");
            toDoList.Enqueue("pay rent");
            toDoList.Enqueue("mow lawn");

            Assert.Equal("clean car", toDoList.Dequeue());
            Assert.Equal("pay rent",  toDoList.Dequeue());
            Assert.Equal("mow lawn",  toDoList.Dequeue());
        }

        [Fact]
        public void BePeekable()
        {
            var toDoList = new Queue<string>();

            toDoList.Enqueue("clean car");
            toDoList.Enqueue("pay rent");
            toDoList.Enqueue("mow lawn");

            Assert.Equal("clean car", toDoList.Peek());
            Assert.Equal("clean car", toDoList.Dequeue());
            Assert.Equal("pay rent", toDoList.Peek());
            Assert.Equal("pay rent", toDoList.Dequeue());
            Assert.Equal("mow lawn", toDoList.Peek());
            Assert.Equal("mow lawn", toDoList.Dequeue());
        }

        [Fact]
        public void ThrowInvalidOperationExceptionIfExaminedWhileEmpty()
        {
            var toDoList = new Queue<string>();

            toDoList.Enqueue("clean car");
            toDoList.Enqueue("pay rent");
            toDoList.Enqueue("mow lawn");

            toDoList.Dequeue();
            toDoList.Dequeue();
            toDoList.Dequeue();

            // Use TryDequeue/Peek to check if you don't want to catch an exception
            Assert.False(toDoList.TryDequeue(out string dequeuedBook));
            Assert.False(toDoList.TryPeek(out string peekedItem));

            Assert.Throws<InvalidOperationException>(() => toDoList.Dequeue());
            Assert.Throws<InvalidOperationException>(() => toDoList.Peek());
        }

        [Fact]
        public void ImplementNonDestructiveIEnumerable()
        {
            var toDoList = new Queue<string>();

            toDoList.Enqueue("clean car");
            toDoList.Enqueue("pay rent");
            toDoList.Enqueue("mow lawn");

            int count = 0;

            // iterate through the entire queue
            foreach (var b in toDoList)
            {
                ++count;
            }

            Assert.Equal(3, count);

            // the collection remains unchanged (foreach does not dequeue)
            Assert.Equal("clean car", toDoList.Dequeue());
            Assert.Equal("pay rent", toDoList.Dequeue());
            Assert.Equal("mow lawn", toDoList.Dequeue());
        }
    }
}
