using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace LinqTests.Operations.Partitioning
{
    public class TakeWhileShould : OperationBase
    {
        [Fact]
        public void OnlyIncludeSequenceLengthItemsIfTheSequenceLengthIsLessThanCount()
        {
            var vehiclesOver50Mpg = Vehicles.OrderByDescending(v => v.FuelEconomy.Combined)
                                            .TakeWhile(v => v.FuelEconomy.Combined > 50);

            foreach (var v in vehiclesOver50Mpg)
            {
                Assert.True(v.FuelEconomy.Combined > 50);
            }
        }

        [Fact]
        public void ReturnAnEmptySequenceIfTheSpecifiedConditionIsNeverMet()
        {
            var vehiclesOver100Mpg = Vehicles.OrderByDescending(v => v.FuelEconomy.Combined)
                                             .TakeWhile(v => v.FuelEconomy.Combined > 100);

            Assert.Empty(vehiclesOver100Mpg);
        }
    }
}
