using System;
using System.Collections.Generic;
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
    }
}
