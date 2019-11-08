using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace LinqTests.Operations.Filtering
{
    public class WhereShould : OperationBase
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
        public void BeCombinable()
        {
            var mostFuelEfficientGMVehicles = Vehicles.Where(v => v.Manufacturer.Name == "General Motors")
                                                      .Where(v => v.FuelEconomy.Combined >= 35);

            foreach (var v in mostFuelEfficientGMVehicles)
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
