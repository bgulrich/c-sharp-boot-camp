using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace LinqTests.Operators
{
    public class ReverseShould : OperatorBase
    {
        [Fact]
        public void ReverseTheOrderOfTheProvidedSequence()
        {
            var vehiclesOrderedByEngineDisplacementAscending = Vehicles.OrderBy(v => v.Engine.DisplacementLiters);

            var vehiclesOrderedByEngineDisplacementDescending = vehiclesOrderedByEngineDisplacementAscending.Reverse();

            var previous = vehiclesOrderedByEngineDisplacementDescending.First();

            foreach (var v in vehiclesOrderedByEngineDisplacementDescending.Skip(1))
            {
                Assert.True(v.Engine.DisplacementLiters <= previous.Engine.DisplacementLiters);
                previous = v;
            }
        }
    }
}
