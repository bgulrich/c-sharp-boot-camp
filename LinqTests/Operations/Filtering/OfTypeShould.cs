using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace LinqTests.Operations.Filtering
{
    public class OfTypeShould
    {
        private class Automobile { }

        private class Car : Automobile { }

        private class Truck : Automobile { }

        private class HeavyDutyTruck : Truck { }

        private class SportUtilityVehicle : Automobile { }


        [Fact]
        public void OnlyIncludeElementsOfTheSpecifiedTypeAndDerivedTypes()
        {
            var trucks = new[] { new Truck(), new Truck(), new HeavyDutyTruck() };

            var autos = new Automobile[] { trucks[0], trucks[1], trucks[2], new Car(), new Car(), new SportUtilityVehicle() };

            var filteredTrucks = autos.OfType<Truck>();

            Assert.Equal(3, filteredTrucks.Count());

            Assert.Contains(filteredTrucks, t => object.ReferenceEquals(t, trucks[0]));
            Assert.Contains(filteredTrucks, t => object.ReferenceEquals(t, trucks[1]));
            Assert.Contains(filteredTrucks, t => object.ReferenceEquals(t, trucks[2]));
        }
    }
}
