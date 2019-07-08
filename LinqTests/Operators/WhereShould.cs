using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace LinqTests.Operators
{
    public class WhereShould : OperatorBase
    {
        [Fact]
        public void OnlyIncludeItemsThatSatisfyPredicate()
        {
            var mostFuelEfficientGMVehicles = Vehicles.Where(v => v.Manufacturer.Name == "General Motors" && v.FuelEconomy.Combined >= 35);

            foreach(var v in mostFuelEfficientGMVehicles)
            {
                Assert.True(v.Manufacturer.Name == "General Motors");
                Assert.True(v.FuelEconomy.Combined >= 35);
            }
        }

        [Fact]
        public void SupportQueryExpressionSyntaxUsage()
        {
            var mostFuelEfficientGMVehicles = from v in Vehicles
                                              where v.Manufacturer.Name == "General Motors" && v.FuelEconomy.Combined >= 35
                                              select v;

            foreach (var v in mostFuelEfficientGMVehicles)
            {
                Assert.True(v.Manufacturer.Name == "General Motors");
                Assert.True(v.FuelEconomy.Combined >= 35);
            }
        }
    }
}
