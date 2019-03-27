using System;
using Xunit;

namespace ModifierTests
{
    public class VirtualAndSealedMethodsShould
    {
        #region Helpers

        public class A
        {
            public virtual string DoSomeWork(int input)
            {
                return $"{nameof(A)}.{nameof(DoSomeWork)}({input}) called";
            }

            public virtual string WhatAreYou()
            {
                return nameof(A);
            }

            public virtual string Blah()
            {
                return "blah";
            }

            // properties, indexers, and events may be virtual too
            public virtual int SomeProperty { get; set; }
            public virtual string this[int index] => "something";
            public virtual event EventHandler<int> SomeEvent;
        }

        public class B : A
        {
            // does not override DoSomeWork

            public override string WhatAreYou()
            {
                return nameof(B);
            }

            // overrides and seals Blah
            public sealed override string Blah()
            {
                return $"{base.Blah()} from {nameof(B)}!";
            }
        }

        public sealed class C : B
        {
            // can still override DoSomeWork
            public override string DoSomeWork(int input)
            {
                return $"{nameof(C)}.{nameof(DoSomeWork)}({input}) called";
            }

            public override string WhatAreYou()
            {
                return nameof(C);
            }

            // cannot override Blah
            //public override string blah()
            //{
            //    return base.blah();
            //}
        }

        // cannot override sealed C
        //public class D : C
        //{
        //}

        #endregion


        [Fact]
        public void InvokeTheMostDerivedTypesOverridenMethod()
        {
            // all derive from A, so let's declare them as As
            A a = new A();
            A b = new B();
            A c = new C();

            // Do some work
            // A is base type, so A's will be used
            Assert.Equal("A.DoSomeWork(5) called", a.DoSomeWork(5));
            // B does not override, so A's will be called
            Assert.Equal("A.DoSomeWork(6) called", b.DoSomeWork(6));
            // C overrides, so C's will be used
            Assert.Equal("C.DoSomeWork(7) called", c.DoSomeWork(7));

            // What are you - overridden in each, so we expect the class name
            Assert.Equal("A", a.WhatAreYou());
            Assert.Equal("B", b.WhatAreYou());
            Assert.Equal("C", c.WhatAreYou());

            // Blah - B overrides but seals
            Assert.Equal("blah", a.Blah());
            Assert.Equal("blah from B!", b.Blah());

            Assert.Equal("blah from B!", c.Blah());

        }
    }
}
