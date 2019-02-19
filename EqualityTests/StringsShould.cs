using System;
using Xunit;

namespace EqualityTests
{
    public class StringsShould
    {
        #region .Equals()

        [Fact]
        public void ReturnTrueWhenEqualsMethodCalledOnStringsWithSameReference()
        {
            var s1 = "Hello, world!";
            var s2 = s1;

            var s1EqualsS2 = s1.Equals(s2);
            var s2EqualsS1 = s2.Equals(s1);

            Assert.True(s1EqualsS2);
            Assert.True(s2EqualsS1);
        }

        [Fact]
        public void ReturnTrueWhenEqualsMethodCalledOnStringsWithSameContent()
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
        public void ReturnTrueWhenEqualityOperatorCalledOnStringsWithNullReferences()
        {
            string s1 = null;
            string s2 = null;

            Assert.True(s1 == s2);
        }

        [Fact]
        public void ReturnTrueWhenEqualityOperatorCalledOnStringsWithSameReference()
        {
            var s1 = "Hello, world!";
            var s2 = s1;

            Assert.True(s1 == s2);
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
