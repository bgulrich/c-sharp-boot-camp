using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xunit;

namespace CollectionTests
{
    public class DictionariesShould
    {
        #region Basics
        [Fact]
        public void RetrieveValuesByIndexingOnTheirKey()
        {
            // initializer
            var items = new Dictionary<int, string>
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
            var items = new Dictionary<int, string>
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
            var items = new Dictionary<int, string>
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
        public void ImplementIEnumerableOfKeyValuePairs()
        {
            var items = new Dictionary<int, string>
            {
                {123456, "bubble gum"},
                {456789, "popcorn" }
            };

            foreach (var kvp in items)
            {
                if (kvp.Key == 123456)
                    Assert.Equal("bubble gum", kvp.Value);
                else
                    Assert.Equal("popcorn", kvp.Value);
            }
        }

        [Fact]
        public void ExposeAKeysCollection()
        {
            var items = new Dictionary<int, string>
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
            var items = new Dictionary<int, string>
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
            var items = new Dictionary<int, string>();

            items.Add(123456, "bubble gum");

            Assert.Throws<ArgumentException>(() => items.Add(123456, "popcorn"));

            // use ContainsKey or TryAdd to avoid exceptions
            Assert.True(items.ContainsKey(123456));
            Assert.False(items.TryAdd(123456, "popcorn"));
        }

        [Fact]
        public void ThrowKeyNotFoundExceptionWhenIndexingOnAMissingKey()
        {
            var items = new Dictionary<int, string>();

            items.Add(123456, "bubble gum");

            Assert.Throws<KeyNotFoundException>(() => Console.WriteLine(items[456789]));

            // use ContainsKey or TryGetValue to avoid exceptions
            Assert.False(items.ContainsKey(456789));
            Assert.False(items.TryGetValue(456789, out var item));
        }
        #endregion

        #region Equality Comparer
        [Fact]
        public void AllowEqualityComparerOverriding()
        {
            var livestockCounts = new Dictionary<string, int>
            {
                {"chickens", 500},
                {"cows", 50 }
            };

            // default string comparer is case-sensitive, so we can do this
            livestockCounts.Add("Chickens", 502);

            // and now we have two entries
            Assert.Equal(500, livestockCounts["chickens"]);
            Assert.Equal(502, livestockCounts["Chickens"]);

            var caseInsensitiveLivestockCounts = new Dictionary<string, int>(StringComparer.InvariantCultureIgnoreCase)
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

        #region Custom Equality Comparer

        private class CaselessStringComparer : IEqualityComparer<string>
        {
            public bool Equals(string x, string y)
            {
                return x.ToLowerInvariant() == y.ToLowerInvariant();
            }

            // Dictionary will use this hash code to map items to buckets
            public int GetHashCode(string input)
            {
                // map lower-case version so strings that only differ in case will
                // map to the same buckets
                return input.ToLowerInvariant().GetHashCode();
            }
        }

        #endregion


        [Fact]
        public void AllowEqualityComparerOverriding_Custom()
        {
            var livestockCounts = new Dictionary<string, int>
            {
                {"chickens", 500},
                {"cows", 50 }
            };

            // default string comparer is case-sensitive, so we can do this
            livestockCounts.Add("Chickens", 502);

            // and now we have two entries
            Assert.Equal(500, livestockCounts["chickens"]);
            Assert.Equal(502, livestockCounts["Chickens"]);

            var caseInsensitiveLivestockCounts = new Dictionary<string, int>(new CaselessStringComparer())
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
            var dictionary = new Dictionary<int, string>
            {
                {123456, "bubble gum"},
                {456789, "popcorn" }
            };

            // wrapped in ReadOnlyDictionary<TKey,TValue> which doesn't allow access to underlying dictionary
            var readOnlyDictionary = new ReadOnlyDictionary<int,string>(dictionary);

            // IDictionary implemented explicitly -> nave to cast to use
            Assert.Equal(2, readOnlyDictionary.Count);
            Assert.Throws<NotSupportedException>(() => (readOnlyDictionary as IDictionary<int, string>).Add(987654, "blah"));
            Assert.Throws<NotSupportedException>(() => (readOnlyDictionary as IDictionary<int, string>)[987654] = "blah");
            Assert.Throws<NotSupportedException>(() => (readOnlyDictionary as IDictionary<int,string>).Remove(123456));

            Assert.Equal(2, readOnlyDictionary.Count);

            // original list modifications reflected in read-only version
            dictionary.Add(24680, "some new item");
            dictionary.Add(13579, "some other item");
            Assert.Equal(4, readOnlyDictionary.Count);
        }
        #endregion
    }
}
