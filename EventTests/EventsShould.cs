using System;
using Xunit;

namespace EventTests
{
    public class EventsShould
    {
        #region Helpers      

        #region Custom
        private class CalculateCalledEventArgs : EventArgs
        {
            public int Count { get; }
            public CalculateCalledEventArgs(int count)
            {
                Count = count;
            }
        }

        private delegate void CalculateCalledEventHandler(object sender, CalculateCalledEventArgs args);
        #endregion

        private class SomeClass
        {
            public int _calculationCount = 0;

            // full generic
            //public event EventHandler<int> CalculateCalled;

            // generic delegate + custom event args
            //public event EventHandler<CalculateCalledEventArgs> CalculateCalled;

            // full custom
            public event CalculateCalledEventHandler CalculateCalled;

            public void Calculate(Guid guid)
            {
                ++_calculationCount;

                // null check in case nobody is subscribed

                // generic
                //CalculateCalled?.Invoke(this, _calculationCount);

                // either custom implemeentation
                CalculateCalled?.Invoke(this, new CalculateCalledEventArgs(_calculationCount));
            }
        }

        #endregion


        [Fact]
        public void AppropriatelyHandleSubscriptionAndUnsubscription()
        {
            var sc = new SomeClass();

            var handledCount = 0;
            var lastCalculationCount = 0;

            // call three times before we subscribe
            sc.Calculate(Guid.Empty);
            sc.Calculate(Guid.Empty);
            sc.Calculate(Guid.Empty);

            // not subscribed, no updates
            Assert.Equal(0, handledCount);
            Assert.Equal(0, lastCalculationCount);

            // this works, but we can't unsubscribe later since it's anonymous
            //sc.CalculateCalled += (o, i) =>
            //{
            //    ++handledCount;
            //    lastCalculationCount = i;
            //};

            // generic
            //void EventRaised(object o, int i)
            //{
            //    ++handledCount;
            //    lastCalculationCount = i;
            //}

            // custom
            void EventRaised(object o, CalculateCalledEventArgs args)
            {
                ++handledCount;
                lastCalculationCount = args.Count;
            }

            // that's better
            sc.CalculateCalled += EventRaised;

            sc.Calculate(Guid.Empty);
            sc.Calculate(Guid.Empty);

            // subscribed now, values should be updating
            Assert.Equal(2, handledCount);
            Assert.Equal(5, lastCalculationCount);

            // add the handler a second time (2 events per call)
            sc.CalculateCalled += EventRaised;

            sc.Calculate(Guid.Empty);
            sc.Calculate(Guid.Empty);

            // double subscribed now, x2 handled per call, last count stays the same
            Assert.Equal(6, handledCount);
            Assert.Equal(7, lastCalculationCount);

            // unsubscribe both
            sc.CalculateCalled -= EventRaised;
            sc.CalculateCalled -= EventRaised;

            sc.Calculate(Guid.Empty);
            sc.Calculate(Guid.Empty);

            // unsubscribed -> no change
            Assert.Equal(6, handledCount);
            Assert.Equal(7, lastCalculationCount);
        }
    }
}
