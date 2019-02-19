using System;
using Xunit;

namespace NullTests
{
    public class NullShould
    {
        #region Helpers

        private class SomeClass { }

        #endregion

        [Fact]
        public void BeTheDefaultValueOfReferenceTypes()
        {
            Assert.Null(default(SomeClass));
        }


        [Fact]
        public void BeTheDefaultValueOfNullableTypes()
        {
            Assert.Null(default(int?));
            Assert.Null(default(Nullable<float>));
        }

    }
}
