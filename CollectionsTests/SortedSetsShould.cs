using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace CollectionTests
{
    public class SortedSetsShould
    {
        #region Basics

        [Fact]
        public void NotAllowDuplicateEntries()
        {
            var cities = new SortedSet<string>
            {
                "Los Angeles", "Houston", "New York", "Atlanta"
            };

            Assert.Equal(4, cities.Count);

            // adding Houston again results in no change
            Assert.False(cities.Add("Houston"));

            Assert.Equal(4, cities.Count);
        }

        [Fact]
        public void EnumerateItemsInSortedOrder()
        {
            var cities = new SortedSet<string>
            {
                "Los Angeles", "Houston", "New York", "Atlanta"
            };

            string previous = null;

            // make sure they arrive in ascending order
            foreach (var city in cities)
            {
                Assert.True(previous == null || city.CompareTo(previous) > 0);
                previous = city;
            }
        }

        #endregion

        #region Set Operations

        [Fact]
        public void ComputeIntersectionWithAnotherCollection()
        {
            var cities = new SortedSet<string>
            {
                "Los Angeles", "Houston", "New York", "Atlanta"
            };

            // IEnumerable<T> - need not be a set
            var eastCities = new string[]
            {
               "New York", "Atlanta", "Washington, D.C.", "Miami"
            };

            // set modified in place
            // items in both sets
            cities.IntersectWith(eastCities);

            Assert.Equal(2, cities.Count);
            Assert.Contains("New York", cities);
            Assert.Contains("Atlanta", cities);
        }

        [Fact]
        public void ComputeUnionWithAnotherCollection()
        {
            var cities = new SortedSet<string>
            {
                "Los Angeles", "Houston", "New York", "Atlanta"
            };

            // IEnumerable<T> - need not be a set
            var eastCities = new string[]
            {
                "New York", "Atlanta", "Washington, D.C.", "Miami"
            };

            // all items from either set
            cities.UnionWith(eastCities);

            Assert.Equal(6, cities.Count);
        }

        [Fact]
        public void ComputeDifferenceWithAnotherCollection()
        {
            var cities = new SortedSet<string>
            {
                "Los Angeles", "Houston", "New York", "Atlanta"
            };

            // IEnumerable<T> - need not be a set
            var eastCities = new string[]
            {
                "New York", "Atlanta", "Washington, D.C.", "Miami"
            };

            // items not in other set
            cities.ExceptWith(eastCities);

            Assert.Equal(2, cities.Count);
            Assert.Contains("Los Angeles", cities);
            Assert.Contains("Houston", cities);
        }

        [Fact]
        public void ComputeSymmetricDifferenceWithAnotherCollection()
        {
            var cities = new SortedSet<string>
            {
                "Los Angeles", "Houston", "New York", "Atlanta"
            };

            // IEnumerable<T> - need not be a set
            var eastCities = new string[]
            {
                "New York", "Atlanta", "Washington, D.C.", "Miami"
            };

            // items in either set, but not both
            cities.SymmetricExceptWith(eastCities);

            Assert.Equal(4, cities.Count);
            Assert.Contains("Los Angeles", cities);
            Assert.Contains("Houston", cities);
            Assert.Contains("Washington, D.C.", cities);
            Assert.Contains("Miami", cities);
        }

        #endregion

        #region Set Comparison
        [Fact]
        public void ComputeSetEquality()
        {
            var cities = new SortedSet<string>
            {
                "Los Angeles", "Houston", "New York", "Atlanta"
            };

            // IEnumerable<T> - need not be a set
            var eastCities = new string[]
            {
                "New York", "Atlanta", "Washington, D.C.", "Miami"
            };

            var rearrangedCities = new string[]
            {
                "Houston", "Los Angeles", "Atlanta", "New York", "Atlanta"
            };

            var rearrangedAndRepeatedCities = new string[]
            {
                "Los Angeles", "Houston", "Los Angeles", "Atlanta", "New York", "Atlanta", "Houston"
            };

            Assert.False(cities.SetEquals(eastCities));
            Assert.True(cities.SetEquals(rearrangedCities));
            Assert.True(cities.SetEquals(rearrangedAndRepeatedCities));
        }

        [Fact]
        public void ComputeIsSubsetOf()
        {
            var cities = new SortedSet<string>
            {
                "Los Angeles", "Houston", "New York", "Atlanta",  "Washington, D.C.", "Miami"
            };

            // IEnumerable<T> - need not be a set
            var eastCities = new SortedSet<string>
            {
                "New York", "Atlanta", "Washington, D.C.", "Miami"
            };

            Assert.False(cities.IsSubsetOf(eastCities));
            Assert.True(eastCities.IsSubsetOf(cities));
        }

        [Fact]
        public void ComputeIsProperSubsetOf()
        {
            var cities = new SortedSet<string>
            {
                "Los Angeles", "New York", "Atlanta",  "Washington, D.C."
            };

            var eastCities = new string[]
            {
                "Los Angeles", "New York", "Atlanta",  "Washington, D.C."
            };

            var eastCitiesExtended = new string[]
            {
                "Los Angeles", "Houston", "New York", "Atlanta",  "Washington, D.C.", "Miami"
            };

            // proper subset = subset and at least one member excluded
            Assert.False(cities.IsProperSubsetOf(eastCities));
            Assert.True(cities.IsProperSubsetOf(eastCitiesExtended));
        }

        [Fact]
        public void ComputeIsSupersetOf()
        {
            var cities = new SortedSet<string>
            {
                "Los Angeles", "Houston", "New York", "Atlanta",  "Washington, D.C.", "Miami"
            };

            var eastCities = new SortedSet<string>
            {
                "New York", "Atlanta", "Washington, D.C.", "Miami"
            };

            Assert.False(eastCities.IsSupersetOf(cities));
            Assert.True(cities.IsSupersetOf(eastCities));
        }

        [Fact]
        public void ComputeIsProperSupersetOf()
        {
            var cities = new SortedSet<string>
            {
                "Los Angeles", "Houston", "New York", "Atlanta",  "Washington, D.C.", "Miami"
            };

            var eastCities = new string[]
            {
                "New York", "Atlanta", "Washington, D.C.", "Miami"
            };

            var eastCitiesExtended = new string[]
            {
                "Los Angeles", "Houston", "New York", "Atlanta",  "Washington, D.C.", "Miami"
            };

            // proper superset = superset and at least one additional member
            Assert.True(cities.IsProperSupersetOf(eastCities));
            Assert.False(cities.IsProperSupersetOf(eastCitiesExtended));
        }

        [Fact]
        public void ComputeOverlaps()
        {
            var cities = new SortedSet<string>
            {
                "Los Angeles", "Houston", "New York", "Atlanta",  "Washington, D.C.", "Miami"
            };

            // IEnumerable<T> - need not be a set
            var eastCities = new string[]
            {
                "New York", "Philadelphia", "Boston"
            };

            var westCities = new string[]
            {
                "San Francisco", "Portland", "Las Vegas"
            };

            // at least one overlapping element
            Assert.True(cities.Overlaps(eastCities));
            Assert.False(cities.Overlaps(westCities));
        }
        #endregion

        #region Equality Comparer
        [Fact]
        public void AllowEqualityComparerOverriding()
        {
            var cities = new SortedSet<string>
            {
                "Los Angeles", "Houston", "New York", "Atlanta"
            };

            Assert.Equal(4, cities.Count);

            // default string comparer is case-sensitive, so we can do this
            cities.Add("HOUSTON");

            // and now we have two Houston entries
            Assert.Equal(5, cities.Count);

            var caseInsensitiveCities = new SortedSet<string>(StringComparer.InvariantCultureIgnoreCase)
            {
                "Los Angeles", "Houston", "New York", "Atlanta"
            };

            // now "Houston" == "houston", so adding is not allowed
            Assert.False(caseInsensitiveCities.Add("HOUSTON"));

            Assert.Equal(4, caseInsensitiveCities.Count);
        }

        #region Custom Equality Comparer

        private class CaselessStringComparer : IComparer<string>
        {
            public int Compare(string x, string y)
            {
                return x.ToLowerInvariant().CompareTo(y.ToLowerInvariant());
            }
        }

        #endregion


        [Fact]
        public void AllowEqualityComparerOverriding_Custom()
        {
            var cities = new SortedSet<string>
            {
                "Los Angeles", "Houston", "New York", "Atlanta"
            };

            Assert.Equal(4, cities.Count);

            // default string comparer is case-sensitive, so we can do this
            cities.Add("HOUSTON");

            // and now we have two Houston entries
            Assert.Equal(5, cities.Count);

            var caseInsensitiveCities = new SortedSet<string>(new CaselessStringComparer())
            {
                "Los Angeles", "Houston", "New York", "Atlanta"
            };

            // now "Houston" == "houston", so adding is not allowed
            Assert.False(caseInsensitiveCities.Add("HOUSTON"));

            Assert.Equal(4, caseInsensitiveCities.Count);
        }
        #endregion
    }
}
