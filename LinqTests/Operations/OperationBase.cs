using System;
using System.Collections.Generic;
using System.Text;
using LinqTests.Data;
using LinqTests.Model;

namespace LinqTests.Operations
{
    public abstract class OperationBase
    {
        protected IEnumerable<Vehicle> Vehicles { get; } = DataLoader.LoadVehiclesFromExcel();
    }
}
