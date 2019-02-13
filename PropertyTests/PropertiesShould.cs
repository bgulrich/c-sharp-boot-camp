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
        }

        // or setter...
        private interface IWriteOnlyPoint
        {
            float X { set; }
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
        }

        // or classes.
        private class PointClass : IReadWritePoint
        {
            // public float X { get; set; }

            // backer + expresion body accessors
            private float _x;

            public float X
            {
                get => _x;
                set => _x = value;
            }
        }

        // sometimes, you can confuse your user
        private class ConfusedPoint : IReadOnlyPoint
        {
            // marked as read-only, but public setter makes it read/write
            public float X { get; set; }
        }


        #endregion


        [Fact]
        public void Test1()
        {

        }
    }
}
