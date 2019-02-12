using System;
using System.Collections;
using System.Collections.Generic;
using Xunit;

namespace IndexerTests
{ 
    public class IndexersShould
    {
        #region Helpers

        #region Read-only fibonaci 
        private class FibonacciSequence : IEnumerable<int>
        {
            public int this[int index]
            {
                get
                {
                    var enumerator = GetEnumerator();

                    do
                    {
                        enumerator.MoveNext();
                        --index;

                    } while (index > -1);

                    return enumerator.Current;
                }
            }

            public IEnumerator<int> GetEnumerator()
            {
                var previous = 0; var current = 1; var temp = 0;

                yield return 0;

                while (true)
                {
                    yield return current;
                    temp = previous;
                    previous = current;
                    current = temp + previous;
                }
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
        #endregion

        #region My collection

        private class MyCollection<T> : ICollection<T>
        {
            private List<T> _backingStore = new List<T>();

            public T this[int index]
            {
                get { return _backingStore[index]; }
                set { _backingStore[index] = value; }
            }
            
            #region ICollection Stuff
            public int Count => _backingStore.Count;

            public bool IsReadOnly => false;

            public void Add(T item)
            {
                _backingStore.Add(item);
            }

            public void Clear()
            {
                _backingStore.Clear();
            }

            public bool Contains(T item)
            {
                return _backingStore.Contains(item);
            }

            public void CopyTo(T[] array, int arrayIndex)
            {
                _backingStore.CopyTo(array, arrayIndex);
            }

            public IEnumerator<T> GetEnumerator()
            {
                return _backingStore.GetEnumerator();
            }

            public bool Remove(T item)
            {
                return _backingStore.Remove(item);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return _backingStore.GetEnumerator();
            }
            #endregion
        }

        #endregion

        #endregion

        [Fact]
        public void GetValueAtProvidedIndex()
        {
            var fib = new FibonacciSequence();

            Assert.Equal(55, fib[10]);
        }

        [Fact]
        public void SetValueAtProvidedIndex()
        {
            var mc = new MyCollection<int>();

            mc.Add(10);
            mc.Add(5);
            mc.Add(3);

            Assert.Equal(3, mc[2]);

            mc[2] = 100;

            Assert.Equal(100, mc[2]);      
        }
    }
}
