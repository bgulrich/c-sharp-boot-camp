using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ConstructorTests
{
    public class DefaultConstructorsShould
    {
        #region Struct
        public struct Point
        {
            public int X { get; set; }
            public int Y { get; set; }

            public Point(int x, int y)
            {
                X = x;
                Y = y;
            }

            // Illegal - structs cannot contain explicit parameterless constructors
            //public Point()
            //{
            //}

            // Illegal - struct constructors must initialize all members
            //public Point(int x)
            //{
            //    X = x;
            //}
        }

        [Fact]
        public void BeGeneratedByCompilerForAllStructs()
        {
            var p = new Point();
        }

        [Fact]
        public void SetDefaultValuesForAllStructMembers()
        {
            var p = new Point();

            Assert.Equal(default(int), p.X);
            Assert.Equal(default(int), p.Y);
        }
        #endregion

        #region Class
        private class SomeClass
        {
            public int S { get; set; }
            public float T { get; set; }

            static SomeClass() { }
        }

        private class SomeClassWithAFactory
        {
            private static int _instanceCount = 0;
            public int InstanceCount { get; private set; }

            private SomeClassWithAFactory(int count)
            {
                InstanceCount = count;
            }

            public static SomeClassWithAFactory GetInstance()
            {
                return new SomeClassWithAFactory(_instanceCount++);
            }
        }

        [Fact]
        public void BeGeneratedByCompilerIfNoInstanceConstructorsAreProvided()
        {
            var sc = new SomeClass();
        }

        [Fact]
        public void SetDefaultValuesForAllClassMembers()
        {
            var sc = new SomeClass();

            Assert.Equal(default(int), sc.S);
            Assert.Equal(default(float), sc.T);
        }

        [Fact]
        public void NotBeGeneratedByCompilerIfAPrivateConstructorIsProvided()
        {
            // invalid
            // var sfc = new SomeClassWithAFactory();

            for(var i = 0; i < 10; ++i)
            {
                Assert.Equal(i, SomeClassWithAFactory.GetInstance().InstanceCount);
            }
        }
        #endregion   
    }
}
