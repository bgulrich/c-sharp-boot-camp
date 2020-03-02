using System;
using System.Collections.Generic;
using System.Text;

namespace LinqTests.Model
{
    public class Manufacturer
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        // Reverse-naviation properties
        public ICollection<Vehicle> Vehicles { get; set; }
    }
}
