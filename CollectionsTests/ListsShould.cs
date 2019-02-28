using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace CollectionTests
{
    public class ListsShould
    {
        #region Count
        [Fact]
        public void ReportTheCorrectCount()
        {
            var list = new List<int> { 1, 2, 3 };
            Assert.Equal(3, list.Count);

            list.Add(4);
            Assert.Equal(4, list.Count);
        }
        #endregion

        #region Enumerable

        [Fact]
        public void BeEnumerable()
        {
            var array = new List<int> { 1, 2, 3 };

            #region enumerator implementation
            var enumerator = array.GetEnumerator();

            enumerator.MoveNext();
            var value = (int)enumerator.Current;

            Assert.Equal(array[0], value);

            int index = 1;

            while (enumerator.MoveNext())
            {
                Assert.Equal(array[index], enumerator.Current);
                ++index;
            }
            #endregion

            #region foreach implementation
            value = 1;

            foreach (var arrayElement in array)
            {
                Assert.Equal(value++, arrayElement);
            }

            #endregion
        }

        #endregion

        #region Indexing
        [Fact]
        public void ReturnElementsInZeroBasedIndexOrder()
        {
            var list = new List<int> { 0, 1, 2, 3, 4 };

            for (int i = 0; i < list.Count; ++i)
                Assert.Equal(i, list[i]);
        }

        [Fact]
        public void ThrowOutOfRangeExceptionIfIndexedBeyondSize()
        {
            var list = new List<int> { 1, 2, 3 };
            Assert.Throws<ArgumentOutOfRangeException>(() => { var y = list[10]; });
        }

        #endregion

        #region Capacity

        [Fact]
        public void GrowInCapacityAsElementsAdded()
        {
            // start with a size of 2
            var list = new List<int>(2);

            Assert.Equal(2, list.Capacity);

            var previousCapacity = 2;
            var capacityBumps = new List<(int index, int newCapacity)>();

            for (var i = 0; i < 100; ++i)
            {
                list.Add(i);

                if (list.Capacity != previousCapacity)
                {
                    capacityBumps.Add((i, list.Capacity));
                    previousCapacity = list.Capacity;
                }
            }

            // expecting at least two capacity bumps to handle 100 elements
            // probably 6 if the capacity doubles each time
            Assert.True(capacityBumps.Count > 2);
        }

        [Fact]
        public void HaveSlowerAdditionsAtCapacityBoundries()
        {
            // start with a size of 2
            var list = new List<int>();

            var normalAddTimes = new List<TimeSpan>();
            var capacityAddTimes = new List<TimeSpan>();

            for (var i = 0; i < 100000; ++i)
            {
                var targetList = list.Count == list.Capacity ? capacityAddTimes : normalAddTimes;

                var start = DateTime.Now;

                list.Add(i);

                targetList.Add(DateTime.Now - start);
            }

            // expecting much slower capacity adds on average (x10)
            Assert.InRange(capacityAddTimes.Average(t => t.TotalMilliseconds) / normalAddTimes.Average(t => t.TotalMilliseconds), 10, 200);
            // expecting much slower last capacity add than first (x10)
            Assert.InRange(capacityAddTimes.Last() / capacityAddTimes.First(), 10, 200);

        }

        #endregion

        #region Read-only
        [Fact]
        public void SupportReadOnlyWrapper()
        {
            var list = new List<int> { 1, 2, 3 };

            // wrapped in ReadOnlyCollection<T> which doesn't allow access to underlying list
            // and implements IList but throws exceptions if modification is attempted
            var readOnlyList = list.AsReadOnly();
            // or
            // var readOnlyList = new ReadOnlyCollection<int>(list);

            Assert.Equal(3, readOnlyList.Count);
            Assert.Throws<NotSupportedException>(() => (readOnlyList as IList<int>).Add(5));
            Assert.Throws<NotSupportedException>(() => (readOnlyList as IList<int>).RemoveAt(0));

            // original list modifications reflected in read-only version
            list.Add(4);
            list.Add(5);
            Assert.Equal(5, readOnlyList.Count);
        }
        #endregion
    }
}
