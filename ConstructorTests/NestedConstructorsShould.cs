using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ConstructorTests
{
    public class NestedConstructorsShould
    {
        private class SomeClass
        {
            public static DateTime? StringTime { get; private set; } = null;
            public static DateTime? IntTime { get; private set; } = null;
            public static DateTime? FloatTime { get; private set; } = null;

            private string S { set { StringTime = StringTime ?? DateTime.Now; } }
            private int I { set { IntTime = IntTime ?? DateTime.Now; } }
            private float F { set { FloatTime = FloatTime ?? DateTime.Now; } }

            public SomeClass(string s, int i, float f)
                : this(i, f)
            {
                S = s;
            }

            private SomeClass(int i, float f)
                : this(f)
            {
                I = i;
            }

            private SomeClass(float f)
            {
                F = f;
            }
        }

        [Fact]
        public void BeCalledBeforeBeforeConstructorBody()
        {
            var sc = new SomeClass("blah", 5, 6.0f);

            Assert.True(SomeClass.FloatTime < SomeClass.IntTime);
            Assert.True(SomeClass.IntTime < SomeClass.StringTime);
        }
    }
}
