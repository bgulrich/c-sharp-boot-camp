using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace CollectionTests
{
    public class StacksShould
    {
        [Fact]
        public void BehaveInALifoWay()
        {
            var bookStack = new Stack<string>();

            bookStack.Push("War and Peace");
            bookStack.Push("The Cat in the Hat");
            bookStack.Push("The Lion, the Witch, and the Wardrobe");

            Assert.Equal("The Lion, the Witch, and the Wardrobe", bookStack.Pop());
            Assert.Equal("The Cat in the Hat",                    bookStack.Pop());
            Assert.Equal("War and Peace",                         bookStack.Pop());
        }

        [Fact]
        public void BePeekable()
        {
            var bookStack = new Stack<string>();

            bookStack.Push("War and Peace");
            bookStack.Push("The Cat in the Hat");
            bookStack.Push("The Lion, the Witch, and the Wardrobe");

            Assert.Equal("The Lion, the Witch, and the Wardrobe", bookStack.Peek());
            Assert.Equal("The Lion, the Witch, and the Wardrobe", bookStack.Pop());
            Assert.Equal("The Cat in the Hat",                    bookStack.Peek());
            Assert.Equal("The Cat in the Hat",                    bookStack.Pop());
            Assert.Equal("War and Peace",                         bookStack.Pop());
        }

        [Fact]
        public void ThrowInvalidOperationExceptionIfExaminedWhileEmpty()
        {
            var bookStack = new Stack<string>();

            bookStack.Push("War and Peace");
            bookStack.Push("The Cat in the Hat");
            bookStack.Push("The Lion, the Witch, and the Wardrobe");

            bookStack.Pop();
            bookStack.Pop();
            bookStack.Pop();

            // Use TryPop/Peek to check if you don't want to catch an exception
            Assert.False(bookStack.TryPop(out string poppedBook));
            Assert.False(bookStack.TryPeek(out string peekedBook));

            Assert.Throws<InvalidOperationException>(() => bookStack.Pop());
            Assert.Throws<InvalidOperationException>(() => bookStack.Peek());
        }

        [Fact]
        public void ImplementNonDestructiveIEnumerable()
        {
            var bookStack = new Stack<string>();

            bookStack.Push("War and Peace");
            bookStack.Push("The Cat in the Hat");
            bookStack.Push("The Lion, the Witch, and the Wardrobe");

            int count = 0;

            // iterate through the entire stack
            foreach (var b in bookStack)
            {
                ++count;
            }

            Assert.Equal(3, count);

            // the collection remains unchanged (foreach does not pop)
            Assert.Equal("The Lion, the Witch, and the Wardrobe", bookStack.Pop());
            Assert.Equal("The Cat in the Hat", bookStack.Pop());
            Assert.Equal("War and Peace", bookStack.Pop());
        }
    }
}
