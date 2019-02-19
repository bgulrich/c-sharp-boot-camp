using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace TypeTests
{
    public class ReferenceEqualsShould
    {
        #region Helpers

        private class Point
        {
            public int X { get; set; }
            public int Y { get; set; }
        }

        #endregion

        #region Value Types
        [Fact]
        public void ReturnFalseForValueTypes()
        {
            int x = 5;
            int y = x;

            // always false
            Assert.False(object.ReferenceEquals(x, y));
           
            object o1 = x;
            object o2 = x;

            // event when boxed
            Assert.False(object.ReferenceEquals(o1, o2));
        }
        #endregion

        #region Reference Types
        [Fact]
        public void ReturnFalseForDifferentReferenceTypeInstances()
        {
            // two point instances with identical values
            var p1 = new Point {X = 5, Y = 5};
            var p2 = new Point {X = 5, Y = 5};

            Assert.False(object.ReferenceEquals(p1, p2));
        }

        [Fact]
        public void ReturnTrueForIdenticalReferenceTypes()
        {
            // two point instances with identical values
            var p1 = new Point { X = 5, Y = 5 };
            var p2 = p1;

            // Same reference
            Assert.True(object.ReferenceEquals(p1, p2));

            p2.Y = 7;

            // Still
            Assert.True(object.ReferenceEquals(p1, p2));

            p2 = new Point { X = 5, Y = 5 };

            // Not anymore
            Assert.False(object.ReferenceEquals(p1, p2));
        }
        #endregion

        #region Strings

        [Fact]
        public void ReturnTrueForIdenticalInternedStrings()
        {
            // Constant strings within the same assembly are always interned by the runtime.
            // This means they are stored in the same location in memory. Therefore, 
            // the two strings have reference equality although no assignment takes place.
            var first  = "Hello world!";
            var second = "Hello world!";

            // make sure the strings are interned
            Assert.NotNull(string.IsInterned(first));

            // same instance (interned)
            Assert.True(object.ReferenceEquals(first, second));

            // After a new string is assigned to strA, strA and strB
            // are no longer interned and no longer have reference equality.
            first = "Goodbye world!";
            Assert.False(object.ReferenceEquals(first, second));
        }

        [Fact]
        public void ReturnFalseForIdenticalStringsIfAnyAreNotInterned()
        {
            // Constant strings within the same assembly are always interned by the runtime.
            var first = "Hello world!";

            // A string that is created at runtime cannot be interned.
            var second = new StringBuilder("Hello world!").ToString();

            Assert.False(object.ReferenceEquals(first, second));
        }

        [Fact]
        public void ReturnFalseForIdenticalStringsIfNoneAreNotInterned()
        {
            // A string that is created at runtime cannot be interned.
            var first = new StringBuilder("Hello world!").ToString();

            // A string that is created at runtime cannot be interned.
            var second = new StringBuilder("Hello world!").ToString();

            Assert.False(object.ReferenceEquals(first, second));
        }

        [Fact]
        public void ReturnTrueIfAssignmentOperatorUsedAndStringIsInterned()
        {
            // Constant strings within the same assembly are always interned by the runtime.
            var first = "hello world";

            // assign the same reference
            var second = first;

            Assert.True(object.ReferenceEquals(first, second));
        }

        [Fact]
        public void ReturnTrueIfAssignmentOperatorUsedAndStringIsNotInterneds()
        {
            // A string that is created at runtime cannot be interned.
            var first = new StringBuilder("Hello world!").ToString();

            // assign the same reference
            var second = first;

            Assert.True(object.ReferenceEquals(first, second));
        }

        #endregion
    }
}
