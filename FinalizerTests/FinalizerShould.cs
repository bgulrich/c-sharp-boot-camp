using System;
using System.Threading.Tasks;
using Xunit;

namespace FinalizerTests
{
    /// <summary>
    /// Careful in here: since we're using static properties to track finalizer calls, they don't get cleared in between tests/cases.
    /// We pretty much need a set of types for each test/case.
    /// </summary>
    public class FinalizerShould
    {
        #region Helpers

        private class SomeClass
        {
            public static int FinalizerCount { get; private set; } = 0;
            public static DateTime? FirstFinalizerTime { get; private set; }

            ~SomeClass()
            {
                FirstFinalizerTime = FirstFinalizerTime ?? DateTime.Now;
                ++FinalizerCount;
            }
        }

        private class SomeBaseClass
        {
            public static int BaseFinalizerCount { get; private set; } = 0;
            public static DateTime? FirstBaseFinalizerTime { get; private set; }

            ~SomeBaseClass()
            {
                FirstBaseFinalizerTime = FirstBaseFinalizerTime ?? DateTime.Now;
                ++BaseFinalizerCount;
            }
        }

        private class SomeDerivedClass : SomeBaseClass
        {
            public static int DerivedFinalizerCount { get; private set; } = 0;
            public static DateTime? FirstDerivedFinalizerTime { get; private set; }

            ~SomeDerivedClass()
            {
                FirstDerivedFinalizerTime = FirstDerivedFinalizerTime ?? DateTime.Now;
                ++DerivedFinalizerCount;
            }
        }

        #endregion

        private void DoSomething()
        {
            var sc = new SomeClass();
        }

        private void DoSomethingDerived()
        {
            var sc = new SomeDerivedClass();
        }

        [Fact]
        public void RunWhenNoHeldReferencesAndGarbageCollected()
        {
            Assert.Equal(0, SomeClass.FinalizerCount);

            DoSomething();
            // force garbage collection
            GC.Collect();
            // give the garbage collector a chance to do its thing
            GC.WaitForPendingFinalizers();

            Assert.Equal(1, SomeClass.FinalizerCount);
        }

        [Fact]
        public void RunBeforeBaseClassFinalizer()
        {
            Assert.Equal(0, SomeBaseClass.BaseFinalizerCount);
            Assert.Equal(0, SomeDerivedClass.DerivedFinalizerCount);

            DoSomethingDerived();
            // force garbage collection
            GC.Collect();

            // give the garbage collector a chance to do its thing
            GC.WaitForPendingFinalizers();

            Assert.Equal(1, SomeBaseClass.BaseFinalizerCount);
            Assert.Equal(1, SomeDerivedClass.DerivedFinalizerCount);
            // make sure base called after derived
            Assert.True(SomeBaseClass.FirstBaseFinalizerTime > SomeDerivedClass.FirstDerivedFinalizerTime);
        }
    }
}
