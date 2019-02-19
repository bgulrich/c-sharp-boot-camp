using System;
using Xunit;

namespace OperatorTests
{
    public class DefaultShould
    {
        #region Helpers
        private class SomeClass
        {
            
        }

        private class SomeGenericClass<T>
        {

        }

        private enum SomeEnum
        {
            SomeValue,
            SomeOtherValue
        }

        private struct SomeStruct
        {
            public int Integer { get; set; }
            public float Float { get; set; }
        }
        #endregion

        [Fact]
        public void ReturnNullForReferenceTypes()
        {
            Assert.Null(default(SomeClass));
            Assert.Null(default(SomeGenericClass<int>));
            Assert.Null(default(Delegate));
            Assert.Null(default(EventHandler));
        }

        [Fact]
        public void ReturnExpectedValuesForBuiltInValueTypes()
        {
            // numerics: zero
            Assert.Equal(0, default(int));
            Assert.Equal(0f, default(float));

            // enum: zero value
            Assert.Equal((SomeEnum)0, default(SomeEnum));

            // bool: false
            Assert.False(default(bool));

            // char: '\0'
            Assert.Equal('\0', default(char));
        }

        [Fact]
        public void ReturnStructWithDefaultMembersForStruct()
        {
            var defaultStruct = default(SomeStruct);

            Assert.Equal(default(int), defaultStruct.Integer);
            Assert.Equal(default(float), defaultStruct.Float);
        }
    }
}
