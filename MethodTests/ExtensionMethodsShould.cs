using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace MethodTests
{
    public class ExtensionMethodsShould
    {
        [Fact]
        public void EnableExtendingSealedClasses()
        {
            // Event though the host class is sealed, we can use the sum method as though it were a member
            var ssc = new SomeSealedClass {A = 6, B = 200};

            Assert.Equal(206, ssc.Sum());
        }

        [Fact]
        public void EnableExtendingAnInterface()
        {
            var items = new List<double> {1, 2, 3, 3, 9, 10};

            Assert.InRange(items.StandardDeviation(), 3.495, 3.497);
        }
    }

    public sealed class SomeSealedClass
    {
        public int A { get; set; }
        public int B { get; set; }
    }

    public static class Extensions
    {
        public static int Sum(this SomeSealedClass ssc)
        {
            return ssc.A + ssc.B;
        }

        public static double StandardDeviation(this IEnumerable<double> items)
        {
            if (!items.Any())
                throw new ArgumentException(nameof(items));

            var mean = items.Average();
            var variance = items.Sum(d => Math.Pow(d - mean, 2))/items.Count(); 
            return  Math.Sqrt(variance);
        }
    }
}
