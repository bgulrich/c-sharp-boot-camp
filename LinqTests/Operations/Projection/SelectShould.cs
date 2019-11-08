using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace LinqTests.Operations.Projection
{
    public class SelectShould : OperationBase
    {
        [Fact]
        public void ProjectToASequenceOfIndividualSelectedProperties()
        {
            var lamborghiniModels = Vehicles.Where(v => v.Make == "Lamborghini")
                                            .Select(v => v.Model);

            Assert.Equal(7, lamborghiniModels.Count());
            Assert.Contains(lamborghiniModels, m => m == "Aventador Roadster");
        }

        [Fact]
        public void ProjectToASequenceOfAnonymouslyTypedElements()
        {
            var lamborghiniModelsAndEngines = Vehicles.Where(v => v.Make == "Lamborghini")
                                                      .Select(v => new { v.Model, v.Engine });

            Assert.Equal(7, lamborghiniModelsAndEngines.Count());
            Assert.Contains(lamborghiniModelsAndEngines, m => m.Model == "Aventador Roadster" && m.Engine.Cylinders == 12);
        }

        [Fact]
        public void SupportQueryExpressionSyntaxUsage()
        {
            var lamborghiniModelsAndEngines = from v in Vehicles
                                              where v.Make == "Lamborghini"
                                              select new { v.Model, v.Engine };

            Assert.Equal(7, lamborghiniModelsAndEngines.Count());
            Assert.Contains(lamborghiniModelsAndEngines, m => m.Model == "Aventador Roadster" && m.Engine.Cylinders == 12);
        }
    }
}
