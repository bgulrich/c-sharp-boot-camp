using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace TypeTests
{
    public class ValueTypesShould
    {
        #region Helpers

        private struct Pixel
        {
            public byte R { get; set; }
            public byte G { get; set; }
            public byte B { get; set; }

        }
        #endregion

        [Fact]
        public void HaveValuesCopiedOnAssignment()
        {
            int x = 5;

            // y does not refer to the same memory as x, rather, x's value is copied into y
            int y = x;

            // set x to 10, y unaffected
            x = 10;

            // verify
            Assert.Equal(10, x);
            Assert.Equal(5,  y);

            // set y to 7, x unaffected
            y = 7;

            // verify
            Assert.Equal(10, x);
            Assert.Equal(7,  y);

            Pixel p1 = new Pixel { R = 255, G = 128, B = 0 };

            // p2 does not refer to the same memory as p1, rather, p1's values are copied into p2
            Pixel p2 = p1;

            // set p1 to something else, p2 unaffected
            p1 = new Pixel { R = 100, G = 150, B = 200 };

            // verify
            Assert.Equal(new Pixel { R = 100, G = 150, B = 200 }, p1);
            Assert.Equal(new Pixel { R = 255, G = 128, B = 0 }, p2);

            // set p2 to yet another set of values, p1 unaffected
            p2 = new Pixel { R = 0, G = 0, B = 0 };

            // verify
            Assert.Equal(new Pixel { R = 100, G = 150, B = 200 }, p1);
            Assert.Equal(new Pixel { R = 0, G = 0, B = 0 }, p2);
        }
    }
}
