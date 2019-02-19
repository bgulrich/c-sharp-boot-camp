using System;
using Xunit;

namespace EqualityTests
{
    public class ObjectsShould
    {
        #region .Equals()

        [Fact]
        public void ReturnTrueWhenEqualsMethodCalledOnObjectsWithSameReference()
        {
            var o1 = new object();
            var o2 = o1;
            
            var o1EqualsO2 = o1.Equals(o2);
            var o2EqualsO1 = o2.Equals(o1);

            Assert.True(o1EqualsO2);
            Assert.True(o2EqualsO1);
        }

        [Fact]
        public void ReturnTrueWhenEqualsMethodCalledOnObjectsWithSameContent()
        {
            var s1 = "Hello, world!";
            var s2 = new System.Text.StringBuilder().Append("Hello, world!").ToString();

            Assert.False(object.ReferenceEquals(s1, s2));

            var s1EqualsS2 = s1.Equals(s2);
            var s2EqualsS1 = s2.Equals(s1);

            Assert.True(s1EqualsS2);
            Assert.True(s2EqualsS1);
        }

        #endregion

        #region == Operator

        [Fact]
        public void ReturnTrueWhenEqualityOperatorCalledOnObjectsWithNullReferences()
        {
            object o1 = null;
            object o2 = null;

            Assert.True(o1 == o2);
        }

        [Fact]
        public void ReturnTrueWhenEqualityOperatorCalledOnObjectsWithSameReference()
        {
            var o1 = new object();
            var o2 = o1;

            Assert.True(o1 == o2);
        }

        [Fact]
        public void ReturnTrueWhenEqualityOperatorCalledOnStringsWithSameContent()
        {
            var s1 = "Hello, world!";
            var s2 = new System.Text.StringBuilder().Append("Hello, world!").ToString();

            Assert.False(object.ReferenceEquals(s1, s2));
            Assert.True(s1 == s2);
        }

        #endregion
    }
}
