using System;
using System.Collections.Generic;
using Xunit;

namespace ConstructorTests
{
    public class StaticConstructorsShould
    {
        private class SomeClass
        {
            // just accessing any members will force the static constructor to be called
            public static DateTime? StaticConstructorTime { get; } = null;
            public static int StaticConstructorCalls { get; }  = 0;
            public static DateTime? FirstInstanceConstructorTime { get; private set; } = null;
            public static int InstanceConstructorCalls { get; private set; } = 0;

            static SomeClass()
            {
                StaticConstructorTime = DateTime.Now;
                ++StaticConstructorCalls;
            }

            public SomeClass()
            {
                FirstInstanceConstructorTime = FirstInstanceConstructorTime ?? DateTime.Now;
                ++InstanceConstructorCalls;
            }
        }

        [Fact]
        public void BeCalledBeforeInstanceConstructor()
        {
            var sc = new SomeClass();
            Assert.True(SomeClass.StaticConstructorTime < SomeClass.FirstInstanceConstructorTime);
        }

        [Fact]
        public void BeCalledOnlyOnce()
        {
            // not zero because the compiler will call the static constructor before we execute this line
            Assert.Equal(1, SomeClass.StaticConstructorCalls);
            Assert.Equal(0, SomeClass.InstanceConstructorCalls);

            var list = new List<SomeClass>();

            for(var i = 0; i < 100; ++i)
            {
                list.Add(new SomeClass());
            }

            // still only one call to the static constructor
            Assert.Equal(1, SomeClass.StaticConstructorCalls);
            Assert.Equal(100, SomeClass.InstanceConstructorCalls);
        }
    }
}
