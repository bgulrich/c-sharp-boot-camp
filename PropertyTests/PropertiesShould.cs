using System;
using Xunit;

namespace PropertyTests
{
    public class PropertiesShould
    {
        #region Helpers

        // properties can be defined in interfaces and define only getter...
        private interface IReadOnlyPoint
        {
            float X { get; }
            float Y { get; }
        }

        // or setter...
        private interface IWriteOnlyPoint
        {
            float X { set; }
            float Y { set; }
        }

        // or both.
        private interface IReadWritePoint : IReadOnlyPoint, IWriteOnlyPoint
        { }

        // interface properties can be implemented by structs...
        private struct PointStruct : IReadOnlyPoint
        {
            // public float X { get; }

            // with backing field
            private float _x;

            //public float X { get { return _x; } }

            // getter with expression body
            //public float X { get => _x; }

            // expression body property (read-only)
            public float X => _x;
            public float Y { get; }

            public PointStruct(float x, float y)
            {
                _x = x;
                Y = y;
            }
        }

        // or classes.
        private class PointClass : IReadWritePoint
        {
             public float X { get; set; }

            // backer + expression body accessors
            //private float _x;

            //public float X
            //{
            //    get => _x;
            //    //set => _x = value;
            //}

            public float Y { get; set; }
        }

        // sometimes, you can confuse your user
        private class ConfusedPoint : IReadOnlyPoint
        {
            // marked as read-only, but public setter makes it read/write
            public float X { get; set; }
            public float Y { get; set; }
        }
        #endregion


        [Fact]
        public void GetAndSetValuesAsExpected()
        {
            var pc = new PointClass();

            Assert.Equal(default(float), pc.X);
            Assert.Equal(default(float), pc.Y);

            pc.X = 659.234f;
            pc.Y = -565.654f;

            Assert.Equal(659.234f, pc.X);
            Assert.Equal(-565.654f, pc.Y);
        }
    }
}
