using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace LinqTests.Operators
{
    public class SkipWhileShould : OperatorBase
    {
        [Fact]
        public void OnlyIncludeSequenceLengthItemsIfTheSequenceLengthIsLessThanCount()
        {
            var vehiclesUnder50Mpg = Vehicles.OrderByDescending(v => v.FuelEconomy.Combined)
                                             .SkipWhile(v => v.FuelEconomy.Combined >= 50);

            foreach (var v in vehiclesUnder50Mpg)
            {
                Assert.True(v.FuelEconomy.Combined < 50);
            }
        }

        [Fact]
        public void ReturnAnEmptySequenceIfTheSpecifiedConditionIsAlwaysMet()
        {
            var vehiclesUnder1Mpg = Vehicles.OrderByDescending(v => v.FuelEconomy.Combined)
                                            .SkipWhile(v => v.FuelEconomy.Combined > 1);

            Assert.Empty(vehiclesUnder1Mpg);
        }
    }
}
