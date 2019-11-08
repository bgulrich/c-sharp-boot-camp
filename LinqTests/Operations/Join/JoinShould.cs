using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LinqTests.Model;
using Xunit;

namespace LinqTests.Operations.Join
{
    public class JoinShould : OperationBase
    {
        private class CarEnthusiast
        {
            public string Name { get; set; }
            public string FavoriteCar { get; set; }
        }

        private IEnumerable<CarEnthusiast> MotorHeads => new []
        {
            new CarEnthusiast{ Name = "Bob", FavoriteCar = "Porsche 911 Turbo S" },
            new CarEnthusiast{ Name = "Andrea", FavoriteCar = "Ferrari 488 Pista Spider" },
            new CarEnthusiast{ Name = "Johnny", FavoriteCar = "Bugatti Chiron" },
            new CarEnthusiast{ Name = "Alice", FavoriteCar = "Bugatti Chiron" }
        };

        [Fact]
        public void JoinSequencesOnKey()
        {
            // key need not be defined property - composing make/model
            var favoriteCylinderCounts = Vehicles.Join(MotorHeads, v => $"{v.Make} {v.Model}", ce => ce.FavoriteCar,
                                                       (v,ce) => new { ce.Name, v.Engine.Cylinders });

            Assert.Equal(4, favoriteCylinderCounts.Count());

            Assert.Contains(favoriteCylinderCounts, x => x.Name == "Bob" && x.Cylinders == 6);
            Assert.Contains(favoriteCylinderCounts, x => x.Name == "Andrea" && x.Cylinders == 8);
            Assert.Contains(favoriteCylinderCounts, x => x.Name == "Johnny" && x.Cylinders == 16);
            Assert.Contains(favoriteCylinderCounts, x => x.Name == "Alice"  && x.Cylinders == 16);
        }

        [Fact]
        public void ExcludeItemsWithNoMatchingKey()
        {
            var motorHeads = MotorHeads.Append(new CarEnthusiast { Name = "Billy", FavoriteCar = "Lightning McQueen" });

            var favoriteCars = Vehicles.Join(MotorHeads, v => $"{v.Make} {v.Model}", ce => ce.FavoriteCar,
                                             (v, ce) => new { ce.Name, v.Engine.Cylinders });

            // Sorry, Billy.  That's not a real car.
            Assert.DoesNotContain(favoriteCars, x => x.Name == "Billy");
        }

        [Fact]
        public void SupportQueryExpressionSyntaxUsage()
        {
            var favoriteCylinderCounts = from v in Vehicles
                                         join mh in MotorHeads
                                         on $"{v.Make} {v.Model}" equals mh.FavoriteCar
                                         select new { mh.Name, v.Engine.Cylinders };

            Assert.Equal(4, favoriteCylinderCounts.Count());

            Assert.Contains(favoriteCylinderCounts, x => x.Name == "Bob" && x.Cylinders == 6);
            Assert.Contains(favoriteCylinderCounts, x => x.Name == "Andrea" && x.Cylinders == 8);
            Assert.Contains(favoriteCylinderCounts, x => x.Name == "Johnny" && x.Cylinders == 16);
            Assert.Contains(favoriteCylinderCounts, x => x.Name == "Alice" && x.Cylinders == 16);
        }
    }
}
