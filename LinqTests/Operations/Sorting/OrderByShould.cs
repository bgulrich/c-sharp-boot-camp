using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using System.Linq;
using LinqTests.Model;

namespace LinqTests.Operations.Sorting
{
    public class OrderByShould : OperationBase
    {
        [Fact]
        public void OrderByIndicatedPropertyAscending()
        {
            var vehiclesOrderedByEngineDisplacement = Vehicles.OrderBy(v => v.Engine.DisplacementLiters);

            var previous = vehiclesOrderedByEngineDisplacement.First();

            foreach(var v in vehiclesOrderedByEngineDisplacement.Skip(1))
            {
                Assert.True(v.Engine.DisplacementLiters >= previous.Engine.DisplacementLiters);
                previous = v;
            }
        }

        [Fact]
        public void OrderByIndicatedPropertyDescending()
        {
            var vehiclesOrderedByEngineDisplacement = Vehicles.OrderByDescending(v => v.Engine.DisplacementLiters);

            var previous = vehiclesOrderedByEngineDisplacement.First();

            foreach (var v in vehiclesOrderedByEngineDisplacement.Skip(1))
            {
                Assert.True(v.Engine.DisplacementLiters <= previous.Engine.DisplacementLiters);
                previous = v;
            }
        }

        [Fact]
        public void OrderByDerivedValue()
        {
            var vehiclesOrderedByMakeAndModel = Vehicles.OrderBy(v => $"{v.Make} {v.Model}");

            var previous = vehiclesOrderedByMakeAndModel.First();

            foreach (var v in vehiclesOrderedByMakeAndModel.Skip(1))
            {
                Assert.True($"{v.Make} {v.Model}".CompareTo($"{previous.Make} {previous.Model}") >= 0);
                previous = v;
            }
        }

        [Fact]
        public void ThrowInvalidOperationExceptionIfSelectedKeyDoesNotImplementIComparableAndNoComparerIsProvided()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                var vehiclesOrderedByMakeAndModel = Vehicles.OrderBy(v => v.FuelEconomy)
                                                            .ToArray(); // force execution
            });
        }

        public class FuelEconomyComparer : IComparer<FuelEconomy>
        {
            public int Compare(FuelEconomy x, FuelEconomy y)
            {
                // combined, then highway, then city
                if (x.Combined < y.Combined)
                    return -1;
                if (x.Combined > y.Combined)
                    return 1;

                if (x.Highway < y.Highway)
                    return -1;
                if (x.Highway > y.Highway)
                    return 1;

                if (x.City < y.City)
                    return -1;
                if (x.City > y.City)
                    return 1;

                return 0;
            }
        }

        [Fact]
        public void OrderUsingProvidedComparer()
        {
            var vehiclesOrderedByMakeAndModel = Vehicles.OrderBy(v => v.FuelEconomy, new FuelEconomyComparer());
            // happens to be equivalent to
            //var vehiclesOrderedByMakeAndModel = Vehicles.OrderBy(v => v.FuelEconomy.Combined)
            //                                            .ThenBy(v => v.FuelEconomy.Highway)
            //                                            .ThenBy(v => v.FuelEconomy.City);

            var previous = vehiclesOrderedByMakeAndModel.First();

            foreach (var v in vehiclesOrderedByMakeAndModel.Skip(1))
            {
                Assert.True(v.FuelEconomy.Combined > previous.FuelEconomy.Combined
                          || (v.FuelEconomy.Combined == previous.FuelEconomy.Combined && (v.FuelEconomy.Highway > previous.FuelEconomy.Highway
                          || (v.FuelEconomy.Highway == previous.FuelEconomy.Highway && v.FuelEconomy.City >= previous.FuelEconomy.City))));

                previous = v;
            }
        }

        [Fact]
        public void OrderByPrimaryThenBySecondaryValues()
        {
            var vehiclesOrderedByMakeAndModel = Vehicles.OrderBy(v => v.Make)
                                                        .ThenBy(v => v.Model);

            var previous = vehiclesOrderedByMakeAndModel.First();

            foreach (var v in vehiclesOrderedByMakeAndModel.Skip(1))
            {
                var makeComparison = v.Make.CompareTo(previous.Make);

                Assert.True(makeComparison >= 0);

                // same manufacturer - make sure model values supply secondary sort order
                if(makeComparison == 0)
                {
                    Assert.True(v.Model.CompareTo(previous.Model) >= 0);
                }

                previous = v;
            }
        }

        [Fact]
        public void OnlyUseTheFinalPrimaryOrderBySequenceForOrdering()
        {
            // this is just silly
            var silly = Vehicles.OrderBy(v => v.FuelEconomy) // this will throw an exception which means it executes
                                .OrderBy(v => v.Make)
                                .ThenBy(v => v.Model);

            Assert.Throws<InvalidOperationException>(() => { var x = silly.First();});

            var vehiclesOrderedByMakeAndModel = Vehicles.OrderBy(v => v.Engine.DisplacementLiters).ThenBy(v => v.Model)
                                                        .OrderBy(v => v.Make)
                                                        .OrderBy(v => v.Make)
                                                        .ThenBy(v => v.Model);

            var previous = vehiclesOrderedByMakeAndModel.First();

            foreach (var v in vehiclesOrderedByMakeAndModel.Skip(1))
            {
                var makeComparison = v.Make.CompareTo(previous.Make);

                Assert.True(makeComparison >= 0);

                // same manufacturer - make sure model values supply secondary sort order
                if (makeComparison == 0)
                {
                    Assert.True(v.Model.CompareTo(previous.Model) >= 0);
                }

                previous = v;
            }
        }

     [Fact]
        public void SupportQueryExpressionSyntaxUsage()
        {
            var vehiclesOrderedByMakeAndModel = from vehicle in Vehicles
                                                orderby vehicle.Make, vehicle.Model 
                                                select vehicle;

            var previous = vehiclesOrderedByMakeAndModel.First();

            foreach (var v in vehiclesOrderedByMakeAndModel.Skip(1))
            {
                var makeComparison = v.Make.CompareTo(previous.Make);

                Assert.True(makeComparison >= 0);

                // same manufacturer - make sure model values supply secondary sort order
                if (makeComparison == 0)
                {
                    Assert.True(v.Model.CompareTo(previous.Model) >= 0);
                }

                previous = v;
            }
        }
    }
}
