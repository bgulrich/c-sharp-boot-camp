using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xunit;

namespace CollectionTests
{
    public class KeyedCollectionsShould
    {
        private class StoreItem
        {
            public int Upc { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public decimal Price { get; set; }
        }

        private class StoreItemsByUpcDictionary : KeyedCollection<int, StoreItem>
        {
            protected override int GetKeyForItem(StoreItem item)
            {
                return item.Upc;
            }
        }


        #region Basics
        [Fact]
        public void RetrieveValuesByIndexingOnTheirKey()
        {
            // initializer
            var items = new StoreItemsByUpcDictionary
            {
                new StoreItem{ Upc = 123456, Name = "bubble gum"},
                new StoreItem{ Upc = 456789,  Name = "popcorn" }
            };

            Assert.Equal("bubble gum", items[123456].Name);
            Assert.Equal("popcorn", items[456789].Name);
        }

        [Fact]
        public void RetrieveValuesByIndexingOnTheirIndex()
        {
            // initializer
            var items = new StoreItemsByUpcDictionary
            {
                new StoreItem{ Upc = 123456, Name = "bubble gum"},
                new StoreItem{ Upc = 456789,  Name = "popcorn" }
            };

            // cast only necessary because our key is an integer and its indexer is given priority
            Assert.Equal("bubble gum", ((IList < StoreItem>)items)[0].Name);
            Assert.Equal("popcorn",    ((IList<StoreItem>)items)[1].Name);
        }

        [Fact]
        public void ReplaceValuesByIndexingIntoIList()
        {
            var items = new StoreItemsByUpcDictionary
            {
                new StoreItem{ Upc = 123456, Name = "bubble gum"},
                new StoreItem{ Upc = 456789, Name = "popcorn" }
            };

            // crazy, but necessary since can't replace by key directly
            (items as IList<StoreItem>)[items.IndexOf(items[456789])] = new StoreItem { Upc = 456789, Name = "potato chips" };

            Assert.Equal("bubble gum", items[123456].Name);
            Assert.Equal("potato chips", items[456789].Name);
        }

        [Fact]
        public void RemoveValuesUsingRemoveMethod()
        {
            var items = new StoreItemsByUpcDictionary
            {
                new StoreItem{ Upc = 123456, Name = "bubble gum"},
                new StoreItem{ Upc = 456789, Name = "popcorn" }
            };

            Assert.True(items.Contains(123456));
            Assert.True(items.Contains(456789));

            // just remove by key
            Assert.True(items.Remove(123456));

            // can also remove by specifying the item if available

            Assert.False(items.Contains(123456));
            Assert.True(items.Contains(456789));
        }

        [Fact]
        public void ImplementIEnumerableOfItems()
        {
            var items = new StoreItemsByUpcDictionary
            {
                new StoreItem{ Upc = 123456, Name = "bubble gum"},
                new StoreItem{ Upc = 456789, Name = "popcorn" }
            };

            foreach (var item in items)
            {
                if (item.Upc == 123456)
                    Assert.Equal("bubble gum", item.Name);
                else
                    Assert.Equal("popcorn", item.Name);
            }
        }

        [Fact]
        public void NotAllowDuplicateKeysToBeInserted()
        {
            var items = new StoreItemsByUpcDictionary
            {
                new StoreItem{ Upc = 123456, Name = "bubble gum"},
                new StoreItem{ Upc = 456789, Name = "popcorn" }
            };

            Assert.Throws<ArgumentException>(() => items.Add(new StoreItem { Upc = 123456, Name = "bubble gum" }));

            // use Contains to avoid exceptions
            Assert.True(items.Contains(123456));
        }

        [Fact]
        public void ThrowKeyNotFoundExceptionWhenIndexingOnAMissingKey()
        {
            var items = new StoreItemsByUpcDictionary
            {
                new StoreItem{ Upc = 123456, Name = "bubble gum"}
            };

            Assert.Throws<KeyNotFoundException>(() => Console.WriteLine(items[456789]));

            // use Contains or TryGetValue to avoid exceptions
            Assert.False(items.Contains(456789));
            Assert.False(items.TryGetValue(456789, out var item));
        }
        #endregion

        #region Read-only
        [Fact]
        public void SupportReadOnlyWrapper()
        {
            // initializer
            var dictionary = new StoreItemsByUpcDictionary
            {
                new StoreItem{ Upc = 123456, Name = "bubble gum"},
                new StoreItem{ Upc = 456789, Name = "popcorn" }
            };

            // wrapped in ReadOnlyDictionary<TKey,TValue> which doesn't allow access to underlying dictionary
            var readOnlyCollection = new ReadOnlyCollection<StoreItem>(dictionary);

            // IDictionary implemented explicitly -> nave to cast to use
            Assert.Equal(2, readOnlyCollection.Count);
            Assert.Throws<NotSupportedException>(() => (readOnlyCollection as IList<StoreItem>).Add(new StoreItem { Upc = 987654, Name = "blah" }));
            Assert.Throws<NotSupportedException>(() => (readOnlyCollection as IList<StoreItem>)[987654] = new StoreItem { Upc = 987654, Name = "blah" });
            Assert.Throws<NotSupportedException>(() => (readOnlyCollection as IList<StoreItem>).RemoveAt(0));

            Assert.Equal(2, readOnlyCollection.Count);

            // original list modifications reflected in read-only version
            dictionary.Add(new StoreItem { Upc = 24680, Name = "some new item" } );
            dictionary.Add(new StoreItem { Upc = 13457, Name = "some other item" });
            Assert.Equal(4, readOnlyCollection.Count);
        }
        #endregion

    }
}
