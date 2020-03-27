using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace TypeTests
{
    public class ReferenceTypesShould
    {
        #region Types

        private class Pixel
        {
            public byte R { get; set; }
            public byte G { get; set; }
            public byte B { get; set; }

        }

        #endregion

        [Fact]
        public void HaveReferencesCopiedOnAssignment()
        {
            Pixel p1 = new Pixel { R = 255, G = 128, B = 0 };

            // p1 reference copied to p2
            Pixel p2 = p1;

            // modify p1, p2 affected
            p1.R = 0;

            // verify p2 updated
            Assert.Equal(0, p2.R);

            // verify same instances
            Assert.Same(p1, p2);
        }
    }
}
