using System;
using Xunit;

namespace EqualityTests
{
    public class FloatsShould
    {
        #region .Equals()

        [Theory]
        [InlineData(5.0f, 5.0f)]
        [InlineData(3.0f, 3.0f)]
        [InlineData(float.MinValue, float.MinValue)]
        [InlineData(float.MaxValue, float.MaxValue)]
        [InlineData(float.Epsilon, float.Epsilon)]
        [InlineData(float.NegativeInfinity, float.NegativeInfinity)]
        [InlineData(float.PositiveInfinity, float.PositiveInfinity)]
        // only difference from ==
        [InlineData(float.NaN, float.NaN)]
        public void ReturnTrueWhenEqualsMethodCalledAndValuesAreTheSame(float f1, float f2)
        {
            Assert.True(f1.Equals(f2));
        }

        #endregion

        #region == Operator

        [Theory]
        [InlineData(5.0f, 5.0f)]
        [InlineData(3.0f, 3.0f)]
        [InlineData(float.MinValue, float.MinValue)]
        [InlineData(float.MaxValue, float.MaxValue)]
        [InlineData(float.Epsilon, float.Epsilon)]
        [InlineData(float.NegativeInfinity, float.NegativeInfinity)]
        [InlineData(float.PositiveInfinity, float.PositiveInfinity)]
        public void ReturnTrueWhenEqualityOperatorCalledAndValuesAreTheSame(float f1, float f2)
        {
            Assert.True(f1 == f2);
        }

        [Theory]
        [InlineData(float.NaN, 5.0f)]
        [InlineData(3.0f, float.NaN)]
        [InlineData(float.NaN, float.NaN)]
        [InlineData(float.NegativeInfinity, float.NaN)]
        [InlineData(float.PositiveInfinity, float.NaN)]
        public void ReturnFalseWhenEqualityOperatorCalledAndEitherValueIsNaN(float f1, float f2)
        {
            Assert.False(f1 == f2);
        }

        #endregion
    }
}
