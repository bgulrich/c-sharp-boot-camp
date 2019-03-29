// 3 combinations supported: 1. both defined, 2. only array defined, 3. neither defined
#define ArrayImplementation
#define ExplicitEnumeratorImplementation

using System.Collections;
using System.Collections.Generic;
using Xunit;

namespace CollectionTests
{

    public class BasicEnumerablesShould
    {
        #region Implementation

        // IEnumerable<int> with array backing store
        private class PositiveIntegers : IEnumerable<int>
        {

#if ArrayImplementation
            // array backing store
            private readonly int[] _array;
#else
            private readonly int _count;
#endif

            public PositiveIntegers(int count)
            {
#if ArrayImplementation
                // initialize backing store
                _array = new int[count];

                for (var i = 0; i < count; ++i)
                {
                    _array[i] = i + 1;
                }
#else
                _count = count;
#endif
            }

            public IEnumerator<int> GetEnumerator()
            {
#if ExplicitEnumeratorImplementation
                // return a new instance of the enumerator for each request
                return new PositiveIntegersEnumerator(this);
#else

#if ArrayImplementation

                // OR below (and no explicit IEnumerator implementation needed)
                for (var i = 0; i < _array.Length; ++i)
                {
                    yield return _array[i];
                }
#else

                // No explicit IEnumerator implementation or backing store (array) needed
                // just need a maximum count and a local count
                // this is the "laziest" implementation as only the count initializer executes before enumeration begins
                var iterations = 0;

                while (iterations < _count)
                {
                    yield return ++iterations;
                }
#endif
#endif
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

#if ExplicitEnumeratorImplementation
            private class PositiveIntegersEnumerator : IEnumerator<int>
            {
                private readonly PositiveIntegers _positiveIntegers;
                private int _currentIndex = -1;

                public PositiveIntegersEnumerator(PositiveIntegers positiveIntegers)
                {
                    _positiveIntegers = positiveIntegers;
                }

                public bool MoveNext()
                {
                    if (_currentIndex < _positiveIntegers._array.Length - 1)
                    {
                        ++_currentIndex;
                        Current = _positiveIntegers._array[_currentIndex];

                        return true;
                    }

                    return false;
                }

                public void Reset()
                {
                    _currentIndex = -1;
                    Current = default(int);
                }

                public int Current { get; private set; }

                object IEnumerator.Current => Current;

                public void Dispose()
                {
                    // nothing to do
                }
            }
#endif
        }
#endregion

#region Tests

        [Fact]
        public void EnumerateItems()
        {
            var count = 10;

            var positiveIntegers = new PositiveIntegers(count);

            var i = 0;

            // foreach example
            foreach (var item in positiveIntegers)
            {
                Assert.Equal(++i, item);
            }

            // make sure we went through all of the items
            Assert.Equal(count, i);

            // reset
            i = 0;

            // explicit enumeration example
            using(var enumerator = positiveIntegers.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    Assert.Equal(++i, enumerator.Current);
                }
            }

            // make sure we went through all of the items
            Assert.Equal(count, i);
        }

        [Fact]
        public void EnumerateItemsInEachEnumeratorIndependently()
        {
            var count = 10;

            var positiveIntegers = new PositiveIntegers(count);

            int i = 0, j;

            // nested foreach example - inner j loop will execute 100 times
            foreach (var itemI in positiveIntegers)
            {
                // reset
                j = 0;

                foreach (var itemJ in positiveIntegers)
                {
                    Assert.Equal(++j, itemJ);
                }

                Assert.Equal(++i, itemI);
            }

            // reset
            i = 0;

            // nested explicit enumeration example
            using (var itemIEnumerator = positiveIntegers.GetEnumerator())
            {
                while (itemIEnumerator.MoveNext())
                {
                    // reset
                    j = 0;

                    using (var itemJEnumerator = positiveIntegers.GetEnumerator())
                    {
                        while (itemJEnumerator.MoveNext())
                        {
                            Assert.Equal(++j, itemJEnumerator.Current);
                        }
                    }

                    Assert.Equal(++i, itemIEnumerator.Current);
                }
            }

            // reset
            i = 0;
            j = 0;

            // non-nested explicit enumeration example
            var iEnumerator = positiveIntegers.GetEnumerator();
            var jEnumerator = positiveIntegers.GetEnumerator();

            iEnumerator.MoveNext(); ++i;
            iEnumerator.MoveNext(); ++i;

            Assert.Equal(i, iEnumerator.Current);

            jEnumerator.MoveNext(); ++j;
            jEnumerator.MoveNext(); ++j;
            jEnumerator.MoveNext(); ++j;

            Assert.Equal(i, iEnumerator.Current);
            Assert.Equal(j, jEnumerator.Current);

            // no "using" - dispose directly
            iEnumerator.Dispose();
            jEnumerator.Dispose();
        }

#endregion
    }

}
