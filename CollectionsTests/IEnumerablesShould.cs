using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace CollectionTests
{
    public class IEnumerablesShould
    {
        #region Enumeration
        /// <summary>
        /// The compiler does something like this behind the scenes for a foreach loop
        /// </summary>
        [Fact]
        public void BeEnumerableExplicitly()
        {
            IEnumerable<int> integers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            var count = 0;

            using (var enumerator = integers.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    Assert.Equal(++count, enumerator.Current);
                }
            }

            Assert.Equal(10, count);
        }

        [Fact]
        public void BeEnumerableWithForeach()
        {
            IEnumerable<int> integers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            var count = 0;
         
            foreach(var i in integers)
            { 
                Assert.Equal(++count, i);
            }

            Assert.Equal(10, count);
        }
        #endregion

        #region Pitfalls

        #region Helpers

        private class Thing
        {
            public int A { get; set; }
            public int B { get; set; }
        }
        
        private static class Helpers
        {
            #region Things
            public static IEnumerable<Thing> Things
            {
                get
                {
                    var i = 1;
                    do
                    {
                        yield return new Thing { A = i, B = i };

                    } while (++i < 20);
                }
            }

            public static void DoubleBValues(IEnumerable<Thing> things)
            {
                foreach (var thing in things)
                    thing.B = thing.B << 1;
            }

            public static IEnumerable<Thing> DoubleBValuesEnumerably(IEnumerable<Thing> things)
            {
                foreach(var thing in things)
                {
                    thing.B = thing.B << 1;
                    yield return thing;
                }
            }
            #endregion

            #region Alphabet subset
            public static IEnumerable<char> AlphabetSubsetNaive(char start, char end)
            {
                // because this method has a yield, it's logic is included in the generated state machine and execution will not being
                // until enumeration begins
                if (start < 'a' || start > 'z')
                    throw new ArgumentOutOfRangeException(paramName: nameof(start), message: "start must be a letter");
                if (end < 'a' || end > 'z')
                    throw new ArgumentOutOfRangeException(paramName: nameof(end), message: "end must be a letter");

                if (end <= start)
                    throw new ArgumentException($"{nameof(end)} must be greater than {nameof(start)}");

                for (var c = start; c < end; c++)
                    yield return c;
            }

            public static IEnumerable<char> AlphabetSubset(char start, char end)
            {
                // this method returns the IEnumerable state machine after validation, so validation will occur immediately
                if (start < 'a' || start > 'z')
                    throw new ArgumentOutOfRangeException(paramName: nameof(start), message: "start must be a letter");
                if (end < 'a' || end > 'z')
                    throw new ArgumentOutOfRangeException(paramName: nameof(end), message: "end must be a letter");

                if (end <= start)
                    throw new ArgumentException($"{nameof(end)} must be greater than {nameof(start)}");

                return AlphabetSubsetImplementation(start, end);
            }

            private static IEnumerable<char> AlphabetSubsetImplementation(char start, char end)
            {
                for (var c = start; c < end; c++)
                    yield return c;
            }

            public static IEnumerable<char> AlphabetSubsetLocalFunction(char start, char end)
            {
                // this method returns the IEnumerable state machine after validation, so validation will occur immediately
                if (start < 'a' || start > 'z')
                    throw new ArgumentOutOfRangeException(paramName: nameof(start), message: "start must be a letter");
                if (end < 'a' || end > 'z')
                    throw new ArgumentOutOfRangeException(paramName: nameof(end), message: "end must be a letter");

                if (end <= start)
                    throw new ArgumentException($"{nameof(end)} must be greater than {nameof(start)}");

                // better still, use a local function since the implementation of alphabet subset should never 
                // be called independent of validation
                IEnumerable<char> AlphabetSubsetLocalFunction()
                {
                    for (var c = start; c < end; c++)
                        yield return c;
                }

                return AlphabetSubsetLocalFunction();
            }
        
            #endregion
        }
        #endregion

        [Fact]
        public void CauseMeToBeCarefulWhenModifying()
        {
            var things = Helpers.Things;

            // this will enumerate things once through and return its modified values nowhere
            Helpers.DoubleBValues(things);

            // this will start enumerating things again (values not doubled)
            Assert.Equal(1, things.First().B);

            var i = 1;

            // grab each doubled value one-by-one gives us the expected behavior
            foreach(var doubledBThing in Helpers.DoubleBValuesEnumerably(Helpers.Things))
            {
                Assert.Equal((i++) * 2, doubledBThing.B);
            }
        }

        [Fact]
        public void CauseMeToBeCarefulWhenValidating()
        {
            // validation does not occur here (delayed until enumeration begins)
            var subsetNaive = Helpers.AlphabetSubsetNaive('f', 'a');

            // delayed validation causes error when enumeration begins
            Assert.Throws<ArgumentException>(() =>
            {
                foreach (var letter in subsetNaive)
                    Console.Write($"{letter}, ");
            });

            // validation occurs immediately
            Assert.Throws<ArgumentOutOfRangeException>(() =>
             {
                 var subset = Helpers.AlphabetSubset('!', 'a');
             });

            // validation occurs immediately
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                var subset = Helpers.AlphabetSubsetLocalFunction('a', '#');
            });
        }

        #endregion
    }
}
