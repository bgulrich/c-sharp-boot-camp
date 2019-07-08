using System;
using System.Collections.Generic;
using System.Text;
using LinqTests.Data;
using LinqTests.Model;

namespace LinqTests.Operators
{
    public abstract class OperatorBase
    {
        protected IEnumerable<Vehicle> Vehicles { get; } = DataLoader.LoadVehiclesFromExcel();
    }
}
