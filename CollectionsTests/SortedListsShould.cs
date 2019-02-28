using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xunit;

namespace CollectionTests
{
    public class SortedListsShould
    {
        #region Basics
        [Fact]
        public void RetrieveValuesByIndexingOnTheirKey()
        {
            // initializer
            var items = new SortedList<int, string>
            {
                {123456, "bubble gum"},
                {456789, "popcorn" }
            };

            // can also add items with Add method
            // items.Add(123456, "bubble gum");
            // and indexer
            // items[123456] = "bubble gum";

            Assert.Equal("bubble gum", items[123456]);
            Assert.Equal("popcorn",    items[456789]);
        }

        [Fact]
        public void ReplaceValuesByIndexingOnTheirKey()
        {
            var items = new SortedList<int, string>
            {
                {123456, "bubble gum"},
                {456789, "popcorn" }
            };

            items[456789] = "potato chips";

            Assert.Equal("bubble gum",   items[123456]);
            Assert.Equal("potato chips", items[456789]);
        }

        [Fact]
        public void RemoveValuesUsingRemoveMethod()
        {
            var items = new SortedList<int, string>
            {
                {123456, "bubble gum"},
                {456789, "popcorn" }
            };

            Assert.True(items.ContainsKey(123456));
            Assert.True(items.ContainsKey(456789));

            // just remove by key
            Assert.True(items.Remove(123456));

            // can also read the value as you remove
            items.Remove(456789, out var value);

            Assert.Equal("popcorn", value);


            Assert.False(items.ContainsKey(123456));
            Assert.False(items.ContainsKey(456789));
        }

        [Fact]
        public void ImplementIEnumerableOfKeyValuePairsInSortedOrder()
        {
            var items = new SortedList<int, string>
            {
                {567890, "chips" },
                {123456, "bubble gum"},
                {456789, "popcorn" },
                {345678, "soda" }
            };

            var previous = -1;

            foreach (var kvp in items)
            {
                Assert.True(previous < kvp.Key);
                previous = kvp.Key;
            }
        }

        [Fact]
        public void ExposeAKeysCollection()
        {
            var items = new SortedList<int, string>
            {
                {123456, "bubble gum"},
                {456789, "popcorn" }
            };

            var sum = 0;

            foreach (var key in items.Keys)
            {
                sum += key;
            }

            Assert.Equal(123456 + 456789, sum);
        }

        [Fact]
        public void ExposeAValuesCollection()
        {
            var items = new SortedList<int, string>
            {
                {123456, "bubble gum"},
                {456789, "popcorn" }
            };

            var values = new List<string>();

            foreach (var value in items.Values)
            {
                values.Add(value);
            }

            Assert.Contains("bubble gum", values);
            Assert.Contains("popcorn", values);
        }

        [Fact]
        public void NotAllowDuplicateKeysToBeInserted()
        {
            var items = new SortedList<int, string>();

            items.Add(123456, "bubble gum");

            Assert.Throws<ArgumentException>(() => items.Add(123456, "popcorn"));

            // use ContainsKey or TryAdd to avoid exceptions
            Assert.True(items.ContainsKey(123456));
            Assert.False(items.TryAdd(123456, "popcorn"));
        }

        [Fact]
        public void ThrowKeyNotFoundExceptionWhenIndexingOnAMissingKey()
        {
            var items = new SortedList<int, string>();

            items.Add(123456, "bubble gum");

            Assert.Throws<KeyNotFoundException>(() => Console.WriteLine(items[456789]));

            // use ContainsKey or TryGetValue to avoid exceptions
            Assert.False(items.ContainsKey(456789));
            Assert.False(items.TryGetValue(456789, out var item));
        }
        #endregion

        #region Comparer
        [Fact]
        public void AllowComparerOverriding()
        {
            var livestockCounts = new SortedList<string, int>
            {
                {"chickens", 500},
                {"cows", 50 }
            };

            // default string comparer is case-sensitive, so we can do this
            livestockCounts.Add("Chickens", 502);

            // and now we have two entries
            Assert.Equal(500, livestockCounts["chickens"]);
            Assert.Equal(502, livestockCounts["Chickens"]);

            var caseInsensitiveLivestockCounts = new SortedList<string, int>(StringComparer.InvariantCultureIgnoreCase)
            {
                {"chickens", 500},
                {"cows", 50 }
            };

            // now "Chickens" == "chickens", so adding is not allowed
            Assert.Throws<ArgumentException>(() => caseInsensitiveLivestockCounts.Add("Chickens", 502));

            // and we can update
            caseInsensitiveLivestockCounts["Chickens"] = 502;

            Assert.Equal(502, caseInsensitiveLivestockCounts["chickens"]);
        }

        #region Custom Comparer

        private class CaselessStringComparer : IComparer<string>
        {
            public int Compare(string x, string y)
            {
                return x.ToLowerInvariant().CompareTo(y.ToLowerInvariant());
            }
        }

        #endregion


        [Fact]
        public void AllowComparerOverriding_Custom()
        {
            var livestockCounts = new SortedList<string, int>
            {
                {"chickens", 500},
                {"cows", 50 }
            };

            // default string comparer is case-sensitive, so we can do this
            livestockCounts.Add("Chickens", 502);

            // and now we have two entries
            Assert.Equal(500, livestockCounts["chickens"]);
            Assert.Equal(502, livestockCounts["Chickens"]);

            var caseInsensitiveLivestockCounts = new SortedList<string, int>(new CaselessStringComparer())
            {
                {"chickens", 500},
                {"cows", 50 }
            };

            // now "Chickens" == "chickens", so adding is not allowed
            Assert.Throws<ArgumentException>(() => caseInsensitiveLivestockCounts.Add("Chickens", 502));

            // and we can update
            caseInsensitiveLivestockCounts["Chickens"] = 502;

            Assert.Equal(502, caseInsensitiveLivestockCounts["chickens"]);
        }
        #endregion

        #region Read-only
        [Fact]
        public void SupportReadOnlyWrapper()
        {
            // initializer
            var sortedList = new SortedList<int, string>
            {
                {123456, "bubble gum"},
                {456789, "popcorn" }
            };

            // wrapped in ReadOnlyDictionary<TKey,TValue> which doesn't allow access to underlying dictionary
            var readOnlySortedList = new ReadOnlyDictionary<int,string>(sortedList);

            // IDictionary implemented explicitly -> nave to cast to use
            Assert.Equal(2, readOnlySortedList.Count);
            Assert.Throws<NotSupportedException>(() => (readOnlySortedList as IDictionary<int, string>).Add(987654, "blah"));
            Assert.Throws<NotSupportedException>(() => (readOnlySortedList as IDictionary<int, string>)[987654] = "blah");
            Assert.Throws<NotSupportedException>(() => (readOnlySortedList as IDictionary<int,string>).Remove(123456));

            Assert.Equal(2, readOnlySortedList.Count);

            // original list modifications reflected in read-only version
            sortedList.Add(24680, "some new item");
            sortedList.Add(13579, "some other item");
            Assert.Equal(4, readOnlySortedList.Count);
        }
        #endregion
    }
}
