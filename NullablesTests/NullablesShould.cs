using System;
using Xunit;

namespace NullablesTests
{
    public class NullableShould
    {
        [Fact]
        public void ReportCorrectHasValue()
        {
            int? nullableInt = null;

            Assert.False(nullableInt.HasValue);

            nullableInt = 50;

            Assert.True(nullableInt.HasValue);
        }

        [Fact]
        public void StoreCorrectValue()
        {
            int? nullableInt = null;

            Assert.Null(nullableInt);

            nullableInt = 50;

            Assert.NotNull(nullableInt);
            Assert.Equal(50, nullableInt.Value);
        }

        [Fact]
        public void ReturnCorrectValueWhenGetValueOrdefaultIsCalled()
        {
            int? nullableInt = null;

            Assert.Equal(default(int), nullableInt.GetValueOrDefault());

            nullableInt = 50;

            Assert.Equal(50, nullableInt.GetValueOrDefault());
        }

        [Fact]
        public void ReturnCorrectValueWhenGetValueOrdefaultIsCalledWithSpecifiedDefaultValue()
        {
            int? nullableInt = null;

            Assert.Equal(10, nullableInt.GetValueOrDefault(10));

            nullableInt = 50;

            Assert.Equal(50, nullableInt.GetValueOrDefault(10));
        }
    }
}
