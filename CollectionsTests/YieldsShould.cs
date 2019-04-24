using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using Xunit;

namespace CollectionTests
{
    public class YieldsShould
    {
        #region Helpers

        #region IEnumerable implementing class

        private class DaysOfWeek : IEnumerable<string>
        {
            public IEnumerator<string> GetEnumerator()
            {
                // instead of implementing separate enumerator class, just yield
                // return new DaysOfWeekEnumerator();

                yield return "Sunday";
                yield return "Monday";
                yield return "Tuesday";
                yield return "Wednesday";
                yield return "Thursday";
                yield return "Friday";
                yield return "Saturday";
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
        #endregion

        #region Methods

        private class DisposableThing : IDisposable
        {
            public static int DisposedCount { get; private set; } = 0;

            public void Dispose()
            {
                ++DisposedCount;
            }
        }

        private static class Yields
        {
            public static int FibonacciFinallyCount { get; private set; } = 0;

            public static IEnumerable<int> CountTo(int input)
            {
                var i = 0;

                while (true)
                {
                    if (i < input)
                        yield return ++i;
                    else
                        yield break;
                }
            }

            public static IEnumerable<int> GetFibonacciNumbers()
            {
                var previous = 0;
                var current = 1;
                var temp = 0;

                yield return 0;

                try
                {
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
                finally
                {
                    ++FibonacciFinallyCount;
                }
            }

            public static IEnumerable<string> GetLines(string filePath)
            {
                using (var textFile = File.OpenText(filePath))
                {
                    string line;

                    while ((line = textFile.ReadLine()) != null)
                    {
                        yield return line;
                    }
                }
            }

            public static IEnumerable<int> GetRandomNumbers()
            {
                var rand = new Random();

                // can use "using" with yield
                using (var dt = new DisposableThing())
                {
                    while (true)
                    {
                        yield return rand.Next();
                    }
                }
            }
        }

        #endregion

        #endregion

        [Fact]
        public void EnumerateDaysOfWeek()
        {
            var expected = new[] {"Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"};

            var index = 0;

            foreach (var day in new DaysOfWeek())
            {
                Assert.Equal(day, expected[index]);
                ++index;
            }
        }

        [Fact]
        public void CountTo()
        {
            var expected = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            var index = 0;

            foreach (var count in Yields.CountTo(10))
            {
                Assert.Equal(count, expected[index]);
                ++index;
            }
        }

        [Fact]
        public void ExecuteFinallyBlockWhenEnumeratorDisposed()
        {
            var previousFinallyCount = Yields.FibonacciFinallyCount;

            using (var fibEnumerator = Yields.GetFibonacciNumbers().GetEnumerator())
            {
                for (int i = 0; i < 10; ++i)
                {
                    fibEnumerator.MoveNext();
                }
            }

            Assert.Equal(previousFinallyCount + 1, Yields.FibonacciFinallyCount);
        }

        [Fact]
        public void ExecuteUsingDisposalWhenEnumeratorDisposed()
        {
            #region explicit dispose
            var previousDisposedCount = DisposableThing.DisposedCount;

            var enumerator = Yields.GetRandomNumbers().GetEnumerator();
            // make something happen
            enumerator.MoveNext();
            enumerator.MoveNext();

            enumerator.Dispose();

            Assert.Equal(previousDisposedCount + 1, DisposableThing.DisposedCount);

            #endregion

            #region
            previousDisposedCount = DisposableThing.DisposedCount;

            // using
            using (var randomEnumerator = Yields.GetRandomNumbers().GetEnumerator())
            {
                for (int i = 0; i < 10; ++i)
                {
                    randomEnumerator.MoveNext();
                    Console.WriteLine(randomEnumerator.Current);
                }
            }

            Assert.Equal(previousDisposedCount + 1, DisposableThing.DisposedCount);

            #endregion

            #region
            //previousDisposedCount = DisposableThing.DisposedCount;

            //// foreach
            //foreach (var rand in Yields.GetFibonacciNumbers())
            //{
            //    if (rand < 10000)
            //        break;
            //}

            //Assert.Equal(previousDisposedCount + 1, DisposableThing.DisposedCount);
            #endregion
        }

        /// <summary>
        /// Demonstrates using yield on a very large set of data
        /// </summary>
        [Fact]
        public void TimeoutReadingLinesOneAtATime()
        {
            int linesRead = 0;

            Action a = () =>
            {
                var lines = Yields.GetLines("TestLines.txt");
                var timeout = DateTime.Now.AddMilliseconds(100);

                foreach (var line in lines)
                {
                    ++linesRead;

                    if (DateTime.Now > timeout)
                        throw new TimeoutException();
                }
            };

            Assert.Throws<TimeoutException>(() => a());
            Assert.InRange(linesRead, 100, 1000000); // actual length 1,187,770 lines
        }

        [Fact]
        public void TimeoutReadingLinesAllAtOnce()
        {
            string text = null;
            bool complete = false;

            var textFile = System.IO.File.OpenText("TestLines.txt");

            Assert.ThrowsAny<Exception>(() =>
            {
                using (var cts = new CancellationTokenSource(50))
                {
                    // when cancellation occurs, dispose of the open file -> throws exception when next buffer read occurs
                    cts.Token.Register(() => textFile.Dispose());
                    text = textFile.ReadToEnd();
                    complete = true;
                }
            });

            textFile.Dispose();

            // failed to complete in less than 50 ms?
            Assert.False(complete);
        }

        [Fact]
        public void ReadFixedNumberOfLinesQuickly()
        {
            int i = 0;
            bool complete = false;

            var lines = Yields.GetLines("TestLines.txt");

            var start = DateTime.Now;

            foreach (var line in lines)
            {
                Console.WriteLine(line);

                ++i;

                if (line.Contains("That's ten!"))
                {
                    complete = true;
                    break;
                }
            }

            var end = DateTime.Now;

            Assert.Equal(10, i);
            Assert.True(complete);
            // less than 10 ms to complete?
            Assert.True((end - start).TotalMilliseconds < 10);
        }
    }
}
