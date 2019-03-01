using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace CollectionTests
{
    public class ForeachShould
    {
        #region Helpers

        #region Explicit Enumerable

        public class FibonacciEnumerable : IEnumerable<int>
        {
            public IEnumerator<int> GetEnumerator()
            {
                var previous = 0; var current = 1; var temp = 0;

                yield return 0;

                while (true)
                {
                    yield return current;
                    temp = previous;
                    previous = current;

                    // prevent overflow
                    if (temp + (long)previous > int.MaxValue)
                        yield break;

                    current = temp + previous;
                }
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
        #endregion

        #region Implicit Enumerable
        private class RandomEnumerator
        {
            private Random _random = new Random();
            private int _current;

            public int Current => _current;

            public bool MoveNext()
            {
                _current = _random.Next();
                return true;
            }
        }

        private class RandomEnumerable
        {
            public RandomEnumerator GetEnumerator() => new RandomEnumerator();
        }
        #endregion      

        #endregion

        #region Looping
        [Fact]
        public void IterateAllItems()
        {
            var integers = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            var currentValue = 0;

            foreach(var i in integers)
            {
                Assert.Equal(++currentValue, i);
            }

            Assert.Equal(10, currentValue);
        }

        [Fact]
        public void IterateAllFibonacciValues()
        {
            var largestCount = 0;
            var largest = 0;

            foreach(var f in new FibonacciEnumerable())
            {
                largest = f;
                ++largestCount;
            }

            // largest 31-bit (signed) fibonacci number
            Assert.Equal(1836311903, largest);
            Assert.Equal(47, largestCount);
        }

        [Fact]
        public void ExitWithBreakStatement()
        {
            var integers = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            var currentValue = 0;

            foreach (var i in integers)
            {
                if (currentValue == 4)
                    break;

                Assert.Equal(++currentValue, i);             
            }

            Assert.Equal(4, currentValue);
        }

        [Fact]
        public void ExitWhenExceptionThrown()
        {
            var integers = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            var currentValue = 0;

            try
            {
                foreach (var i in integers)
                {
                    if (currentValue == 4)
                        throw new Exception();

                    Assert.Equal(++currentValue, i);
                }
            }
            catch { }

            Assert.Equal(4, currentValue);
        }

        [Fact]
        public void ExitWithReturnStatement()
        {
            var integers = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            var currentValue = 0;

            try
            {
                foreach (var i in integers)
                {
                    if (currentValue == 4)
                        return;

                    Assert.Equal(++currentValue, i);
                }
            }
            finally { Assert.Equal(4, currentValue); }            
        }

        [Fact]
        public void SkipIterationWithContinue()
        {
            var integers = new[] { 1, 2, 3, 4, 5 };

            var sum = 0;

            foreach (var i in integers)
            {
                if (i == 3)
                    continue;

                sum += i;
            }

            Assert.Equal(integers.Sum() - 3, sum);
        }

        [Fact]
        public void ThrowInvalidOperationExceptionIfListModificationAttemptedInForeachLoop()
        {
            var days = new List<string> { "Sunday", "Monday", "Tuesday", "Wednesday", null, "Friday", "Saturday"};
            int index = 0;

            // modification not allowed for List<T> and most other collection types, but not all
            Assert.Throws<InvalidOperationException>(() =>
            {
                foreach (var day in days)
                {
                    if (day == null)
                    {
                        // invalid - foreach iteration variables are read-only (even if they weren't, this wouldn't accomplish the intended goal)
                        // day = "Thursday";
                        days[index] = "Thursday";
                    }

                    ++index;
                }
            });
        }

        [Fact]
        public void AllowModificationOfArrayInForeachLoop()
        {
            var days = new string[] { "Sunday", "Monday", "Tuesday", "Wednesday", null, "Friday", "Saturday" };
            int index = 0;

            // modification of array is allowed since compiler converts it to a for loop
            foreach (var day in days)
            {
                if (day == null)
                {
                    // invalid - foreach iteration variables are read-only (even if they weren't, this wouldn't accomplish the intended goal)
                    // day = "Thursday";
                    days[index] = "Thursday";
                }

                ++index;
            }

            Assert.Equal("Thursday", days[4]);
        }
        #endregion

        #region Compatible Types

        [Fact]
        public void BeCompatibleWithExplicitEnumerableTypes()
        {
            var fib = new FibonacciEnumerable();

            var count = 0;

            foreach (var f in fib)
            {
                Assert.True(true);

                Console.WriteLine(f);

                if (++count == 10)
                    break;
            }
        }

        [Fact]
        public void BeCompatibleWithImplicitEnumerableTypes()
        {
            var re = new RandomEnumerable();

            var count = 0;

            foreach(var r in re)
            {
                Assert.True(true);

                if (++count == 10)
                    break;
            }                
        }

        #endregion
    }
}
