using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LinqTests.Model;
using Xunit;

namespace LinqTests.Operations.Quantifier
{
    public class ContainsShould : OperationBase
    {
        [Fact]
        public void ReturnTrueIfTheSequenceContainsTheSpecifiedElement()
        {
            // ensure we're comparing the same objects since default behavior compares with reference equality
            var actualizedVehicles = Vehicles.ToArray();
            var knownContainedVehicle = actualizedVehicles[100];

            Assert.True(actualizedVehicles.Contains(knownContainedVehicle));
        }

        [Fact]
        public void ReturnFalseIfTheSequenceDoesNotContainTheSpecifiedElement()
        {
            var actualizedVehicles = Vehicles.ToArray();

            Assert.False(actualizedVehicles.Contains(new Vehicle { Make = "BrandonCo.", Model = "Super-fast Sports Car" }));
        }

        [Fact]
        public void ReturnTrueIfTheSequenceContainsTheSpecifiedElementUsingTheProvidedEqualityComparer()
        {
            var actualizedVehicles = Vehicles.ToArray();

            Assert.True(actualizedVehicles.Contains(new Vehicle { ModelYear = 2019, Make =  "Ferrari", Model = "488 Pista Spider" }, new VehicleEqualityComparer() ));
        }

        [Fact]
        public void ReturnFalseIfTheSequenceDoesNotContainTheSpecifiedElementUsingTheProvidedEqualityCompare()
        {
            var actualizedVehicles = Vehicles.ToArray();
            Assert.False(actualizedVehicles.Contains(new Vehicle { Make = "BrandonCo.", Model = "Super-fast Sports Car" }, new VehicleEqualityComparer()));
        }

        private class VehicleEqualityComparer : IEqualityComparer<Vehicle>
        {
            public bool Equals(Vehicle x, Vehicle y)
            {
                return GetHashCode(x) == GetHashCode(y);
            }

            public int GetHashCode(Vehicle vehicle)
            {
                return HashCode.Combine(vehicle.Make, vehicle.Model, vehicle.ModelYear);
            }
        }
    }
}
