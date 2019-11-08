using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace LinqTests.Operations.Quantifier
{
    public class AnyShould : OperationBase
    {
        [Fact]
        public void ReturnTrueIfTheSequenceContainsAnyElements()
        {
            Assert.True(Vehicles.Any());
        }

        [Fact]
        public void ReturnFalseIfTheSequenceContainsNoElements()
        {
            Assert.False(new int[0].Any());
        }

        [Fact]
        public void ReturnTrueIfTheSequenceContainsAnyElementsThatSatisfyThePredicate()
        {
            Assert.True(Vehicles.Any(v => v.FuelEconomy.Combined > 50));
        }

        [Fact]
        public void ReturnFalseIfTheSequenceContainsNoElementsThatSatisfyThePredicate()
        {
            Assert.False(Vehicles.Any(v => v.FuelEconomy.Combined > 100));
        }
    }
}
