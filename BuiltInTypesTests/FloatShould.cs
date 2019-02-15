using System;
using Xunit;


namespace BuiltInTypesTests
{
    public class UFloatShould
    {

        [Fact]
        public void BeOfTheSameTypesAsTheirAliases()
        {
            Assert.Equal(typeof(float),  typeof(Single));

        }

        [Fact]
        public void CheckIfSomeFloatIsInRange()
        {
            var someFloat = 2 * Math.Pow(10, 38);


            Assert.InRange(someFloat, Single.MinValue, Single.MaxValue);


        }

        [Fact]
        public void CheckIfSomeFloatIsOutOfRange()
        {
            var someFloat = -9 * Math.Pow(10, 39);

            ;
            Assert.NotInRange(someFloat * Math.Pow(10,32), Single.MinValue, Single.MaxValue);


        }



    }
}
