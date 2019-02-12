using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Xunit;

namespace CollectionTests
{
    public class YieldsShould
    {
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
                var timeout = DateTime.Now.AddSeconds(5);

                foreach (var line in lines)
                {
                    // write to the console to slow us down
                    Console.WriteLine(line);

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
