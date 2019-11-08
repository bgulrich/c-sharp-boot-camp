using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace LinqTests.Operations.Quantifier
{
    public class AllShould : OperationBase
    {
        [Fact]
        public void ReturnTrueIfTheSequenceContainsOnlyElementsThatSatisfyThePredicate()
        {
            Assert.True(Vehicles.All(v => v.FuelEconomy.Combined > 5));
        }

        [Fact]
        public void ReturnFalseIfTheSequenceContainsAnyThatDoNotSatisfyThePredicate()
        {
            Assert.False(Vehicles.All(v => v.FuelEconomy.Combined > 20));
        }
    }
}
