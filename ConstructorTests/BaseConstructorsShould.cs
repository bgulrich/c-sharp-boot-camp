using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ConstructorTests
{
    public class BaseConstructorsShould
    {
        #region Helpers
        private class SomeBaseClass
        {
            public static DateTime? FloatTime { get; private set; } = null;

            private float F { set { FloatTime = FloatTime ?? DateTime.Now; } }

            public SomeBaseClass(float f)
            {
                F = f;
            }
        }

        private class SomeClass : SomeBaseClass
        {
            public static DateTime? IntTime { get; private set; } = null;

            private int I { set { IntTime = IntTime ?? DateTime.Now; } }

            public SomeClass(int i, float f)
                : base(f)
            {
                I = i;
            }
        }

        private class SomeDerivedClass : SomeClass
        {
            public static DateTime? StringTime { get; private set; } = null;
            private string S { set { StringTime = StringTime ?? DateTime.Now; } }

            public SomeDerivedClass(string s, int i, float f)
                : base(i, f)
            {
                S = s;
            }
        }
        #endregion

        [Fact]
        public void BeCalledBeforeConstructorBody()
        {
            var sc = new SomeDerivedClass("blah", 5, 6.0f);

            Assert.True(SomeDerivedClass.FloatTime<SomeDerivedClass.IntTime);
            Assert.True(SomeDerivedClass.IntTime<SomeDerivedClass.StringTime);
        }
    }
}
