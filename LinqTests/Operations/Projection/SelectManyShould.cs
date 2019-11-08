using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LinqTests.Model;
using Xunit;

namespace LinqTests.Operations.Projection
{
    public class SelectManyShould : OperationBase
    {
        [Fact]
        public void FlattenASequenceOfSequneces()
        {
            // create a collection of manufacturers, each of which has a collection of cars
            var manufacturers = Vehicles.GroupBy(v => v.Manufacturer)
                                        .Select(g => new
                                        {
                                            Manufacturer = g.Key,
                                            Vehicles = g.AsEnumerable()
                                        });

            Assert.Equal(22, manufacturers.Count());

            var flattenedVehicles = manufacturers.SelectMany(m => m.Vehicles);

            Assert.Equal(1253, flattenedVehicles.Count());
        }
    }
}
