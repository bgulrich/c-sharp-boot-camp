using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace LinqTests.Operators
{
    public class TakeShould : OperatorBase
    {
        [Fact]
        public void OnlyIncludeCountItemsIfTheSequenceLengthIsGreaterThanCount()
        {
            var top100FuelEfficientVehicles = Vehicles.OrderByDescending(v => v.FuelEconomy.Combined)
                                                      .Take(100);

            Assert.Equal(100, top100FuelEfficientVehicles.Count());
        }

        [Fact]
        public void OnlyIncludeSequenceLengthItemsIfTheSequenceLengthIsLessThanCount()
        {
            var top10FuelEfficientVehiclesOver50Mpg = Vehicles.Where(v => v.FuelEconomy.Combined >= 50)
                                                              .OrderByDescending(v => v.FuelEconomy.Combined)
                                                              .Take(10);

            Assert.True(top10FuelEfficientVehiclesOver50Mpg.Count() < 10);
        }
    }
}
