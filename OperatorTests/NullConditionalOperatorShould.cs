using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace OperatorTests
{
    public class NullConditionalOperatorShould
    {
        #region Helpers

        private class SomeClass
        {
            public int GetAFive() { return 5; }

            public string this[string name]
            {
                get { return $"Hello, {name}."; }
            }
        }


        #endregion

        #region Accessing Members
        [Fact]
        public void ReturnNullIfValueIsNull()
        {
            SomeClass sc = null;
            Assert.Null(sc?.GetAFive());
        }

        [Fact]
        public void CallMemberIfValueIsNotNull()
        {
            SomeClass sc = new SomeClass();
            Assert.Equal(5, sc?.GetAFive());
        }
        #endregion

        #region Indexers
        [Fact]
        public void ReturnNullIfValueIsNullForIndexer()
        {
            SomeClass sc = null;
            Assert.Null(sc?["Bob"]);
        }

        [Fact]
        public void CallIndexerIfValueIsNotNull()
        {
            SomeClass sc = new SomeClass();
            Assert.Equal("Hello, Bob.", sc?["Bob"]);
        }
        #endregion

        #region Arrays
        [Fact]
        public void ReturnNullIfValueIsNullForArray()
        {
            int[] array = null;
            Assert.Null(array?[0]);
        }

        [Fact]
        public void CallArrayIndexerIfValueIsNotNull()
        {
            int[] array = {1, 2, 3};
            Assert.Equal(1, array?[0]);
        }
        #endregion

        #region thread-safe delegates
        [Fact]
        public void AllowThreadSafeDelegateInvocations()
        {
            Func<int> getAFiveDelegate = new SomeClass().GetAFive;

            // this is not thread safe (if someone clears our delegate after the null check, we're hosed)
            //if (getAFiveDelegate != null)
            //    getAFiveDelegate();

            int? delegateReturned = null;

            // we used to have to do this to ensure some other thread didn't clear our delegate between null checking and calling
            var copy = getAFiveDelegate;
            if (copy != null)
                delegateReturned = copy();

            Assert.Equal(5, delegateReturned.Value);

            // clear it out
            delegateReturned = null;

            delegateReturned = getAFiveDelegate?.Invoke();

            Assert.Equal(5, delegateReturned.Value);
        }

        #endregion
    }
}
