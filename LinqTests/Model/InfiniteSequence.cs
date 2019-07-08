using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace LinqTests.Model
{
    public class InfiniteSequence : IEnumerable<int>
    {
        private readonly Random _random = new Random();


        public IEnumerator<int> GetEnumerator()
        {
            while (true)
            {
                yield return _random.Next();
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
