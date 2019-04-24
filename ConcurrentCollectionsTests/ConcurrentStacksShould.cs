using System;
using System.Collections.Concurrent;
using System.Text;
using Xunit;

namespace CollectionTests
{
    public class ConcurrentStacksShould
    {
        [Fact]
        public void BehaveInALifoWay()
        {
            var bookStack = new ConcurrentStack<string>();

            bookStack.Push("War and Peace");
            bookStack.Push("The Cat in the Hat");
            bookStack.Push("The Lion, the Witch, and the Wardrobe");

            Assert.True(bookStack.TryPop(out var first));
            Assert.Equal("The Lion, the Witch, and the Wardrobe", first);

            Assert.True(bookStack.TryPop(out var second));
            Assert.Equal("The Cat in the Hat", second);

            Assert.True(bookStack.TryPop(out var third));
            Assert.Equal("War and Peace", third);
        }

        [Fact]
        public void BePeekable()
        {
            var bookStack = new ConcurrentStack<string>();

            bookStack.Push("War and Peace");
            bookStack.Push("The Cat in the Hat");
            bookStack.Push("The Lion, the Witch, and the Wardrobe");

            Assert.True(bookStack.TryPeek(out var first));
            Assert.Equal("The Lion, the Witch, and the Wardrobe", first);
            Assert.True(bookStack.TryPop(out first));
            Assert.Equal("The Lion, the Witch, and the Wardrobe", first);

            Assert.True(bookStack.TryPeek(out var second));
            Assert.Equal("The Cat in the Hat", second);
            Assert.True(bookStack.TryPop(out second));
            Assert.Equal("The Cat in the Hat", second);

            Assert.True(bookStack.TryPeek(out var third));
            Assert.Equal("War and Peace", third);
            Assert.True(bookStack.TryPop(out third));
            Assert.Equal("War and Peace", third);
        }

        [Fact]
        public void ReturnFalseIfExaminedWhileEmpty()
        {
            var bookStack = new ConcurrentStack<string>();

            bookStack.Push("War and Peace");
            bookStack.Push("The Cat in the Hat");
            bookStack.Push("The Lion, the Witch, and the Wardrobe");

            bookStack.TryPop(out _);
            bookStack.TryPop(out _);
            bookStack.TryPop(out _);

            // Use TryPop/Peek to check if you don't want to catch an exception
            Assert.False(bookStack.TryPop(out _));
            Assert.False(bookStack.TryPeek(out _));
        }

        [Fact]
        public void ImplementNonDestructiveIEnumerable()
        {
            var bookStack = new ConcurrentStack<string>();

            bookStack.Push("War and Peace");
            bookStack.Push("The Cat in the Hat");
            bookStack.Push("The Lion, the Witch, and the Wardrobe");

            var count = 0;

            // iterate through the entire stack
            foreach (var b in bookStack)
            {
                ++count;
            }

            Assert.Equal(3, count);

            // the collection remains unchanged (foreach does not pop)
            Assert.True(bookStack.TryPop(out var first));
            Assert.Equal("The Lion, the Witch, and the Wardrobe", first);

            Assert.True(bookStack.TryPop(out var second));
            Assert.Equal("The Cat in the Hat", second);

            Assert.True(bookStack.TryPop(out var third));
            Assert.Equal("War and Peace", third);
        }
    }
}
