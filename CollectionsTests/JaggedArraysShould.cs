using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace CollectionTests
{
    public class JaggedArraysShould
    {
        [Fact]
        public void DoJaggedArrayStuff()
        {
            // NOT
            //var jagged = new float[3][2][2];

            // can only instantiate one dimension at a time
            // x (outermost array)
            var jagged = new float[3][][];

            // Rank is 1, not 3
            Assert.Equal(1, jagged.Rank);

            // inner arrays are null at this point
            Assert.Null(jagged[0]);
            Assert.Null(jagged[1]);
            Assert.Null(jagged[2]);
           
            // y - instantiate inner arrays
            jagged[0] = new float[2][];
            jagged[1] = new float[2][];
            jagged[2] = new float[2][];

            // inner-inner arrays are null at this point
            Assert.Null(jagged[0][0]);
            Assert.Null(jagged[1][0]);
            Assert.Null(jagged[2][0]);
            Assert.Null(jagged[0][1]);
            Assert.Null(jagged[1][1]);
            Assert.Null(jagged[2][1]);

            // z - instantiate inner-inner arrays
            jagged[0][0] = new float[2];
            jagged[0][1] = new float[2];

            jagged[1][0] = new float[2];
            jagged[1][1] = new float[2];

            jagged[2][0] = new float[2];
            jagged[2][1] = new float[2];

            // or... could be done in a loop
            for (var x = 0; x < jagged.Length; ++x)
            {
                jagged[x] = new float[2][];

                for (var y = 0; y < jagged[x].Length; ++y)
                {
                    jagged[x][y] = new float[2];

                    for (var z = 0; z < jagged[x][y].Length; ++z)
                    {
                        jagged[x][y][z] = (float)(Math.Pow((2 * x), 2) + 5 * y + z);
                    }
                }
            }

            // this array of arrays is not truly jagged and therefore won't give us any benefit over a multidimensional array
        }
    }
}
