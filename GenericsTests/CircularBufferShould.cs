using Generics;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace GenericsTests
{
    public class GenericCircularBufferShould
    {
        const int PERFORMANCE_TEST_CAPACITY = 10000000;

        #region Functionality
        [Fact]
        public void BeEmptyWhenNew()
        {
            var buffer = new CircularBuffer<double>();
            Assert.True(buffer.IsEmpty);
        }

        [Fact]
        public void BeFullAfterCapacityItemsAdded()
        {
            var buffer = new CircularBuffer<double>(capacity: 3);
            buffer.Write(1);
            buffer.Write(1);
            buffer.Write(1);
            Assert.True(buffer.IsFull);
        }

        [Fact]
        public void ReturnItemsInFirstInFirstOutManner()
        {
            var buffer = new CircularBuffer<double>(capacity: 3);
            var value1 = 1.1;
            var value2 = 2.0;

            buffer.Write(value1);
            buffer.Write(value2);

            Assert.Equal(value1, buffer.Read());
            Assert.Equal(value2, buffer.Read());
            Assert.True(buffer.IsEmpty);
        }

        [Fact]
        public void OverwriteWhenOverCapacity()
        {
            var buffer = new CircularBuffer<double>(capacity: 3);
            var values = new[] { 1.0, 2.0, 3.0, 4.0, 5.0 };

            foreach (var value in values)
            {
                buffer.Write(value);
            }

            Assert.True(buffer.IsFull);
            Assert.Equal(values[2], buffer.Read());
            Assert.Equal(values[3], buffer.Read());
            Assert.Equal(values[4], buffer.Read());
            Assert.True(buffer.IsEmpty);
        }
        #endregion

        [Fact]
        public void HaveSimilarPerformanceToDoubleBuffer()
        {
            var tDoubleStart = DateTime.Now;

            var doubleBuffer = new CircularBuffer_Double(capacity: PERFORMANCE_TEST_CAPACITY);

            for(double i = 0; i < PERFORMANCE_TEST_CAPACITY; ++i)
            {
                doubleBuffer.Write(i);
            }

            Assert.True(doubleBuffer.IsFull);

            for(int i = 0; i < PERFORMANCE_TEST_CAPACITY; ++i)
            {
                var value = doubleBuffer.Read();
                Assert.Equal(i, value);
            }

            Assert.True(doubleBuffer.IsEmpty);

            var tGenericStart = DateTime.Now;

            var genericBuffer = new CircularBuffer<double>(capacity: PERFORMANCE_TEST_CAPACITY);

            for (double i = 0; i < PERFORMANCE_TEST_CAPACITY; ++i)
            {
                genericBuffer.Write(i);
            }

            Assert.True(genericBuffer.IsFull);

            for (int i = 0; i < PERFORMANCE_TEST_CAPACITY; ++i)
            {
                // strongly typed and no boxing/unboxing
                var value = genericBuffer.Read();
                Assert.Equal(i, value);
            }

            var tEnd = DateTime.Now;

            var durationDouble = tGenericStart - tDoubleStart;
            var durationGeneric = tEnd - tGenericStart;

            // +/- 10%
            Assert.InRange(durationGeneric, durationDouble * 0.9, durationDouble * 1.1);
        }

        [Fact]
        public void HaveBetterPerformanceThanObjectBuffer()
        {
            var tobjectStart = DateTime.Now;

            var objectBuffer = new CircularBuffer_Object(capacity: PERFORMANCE_TEST_CAPACITY);

            for (double i = 0; i < PERFORMANCE_TEST_CAPACITY; ++i)
            {
                // boxing (impllicit) is expensive
                objectBuffer.Write(i);
            }

            Assert.True(objectBuffer.IsFull);

            for (int i = 0; i < PERFORMANCE_TEST_CAPACITY; ++i)
            {
                // Unboxing (explicit) is expensive
                var value = (double)objectBuffer.Read();
                Assert.Equal(i, value);
            }

            Assert.True(objectBuffer.IsEmpty);

            var tGenericStart = DateTime.Now;

            var genericBuffer = new CircularBuffer<double>(capacity: PERFORMANCE_TEST_CAPACITY);

            for (double i = 0; i < PERFORMANCE_TEST_CAPACITY; ++i)
            {
                genericBuffer.Write(i);
            }

            Assert.True(genericBuffer.IsFull);

            for (int i = 0; i < PERFORMANCE_TEST_CAPACITY; ++i)
            {
                // strongly typed and no boxing/unboxing
                var value = genericBuffer.Read();
                Assert.Equal(i, value);
            }

            var tEnd = DateTime.Now;

            var durationObject = tGenericStart - tobjectStart;
            var durationGeneric = tEnd - tGenericStart;

            // at most 85% of duration
            Assert.InRange(durationGeneric/durationObject, 0.0, 0.85);
        }
    }
}
