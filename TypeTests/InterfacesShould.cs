using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace TypeTests
{
    public class InterfacesShould
    {
        #region Helpers

        private interface IWorker
        {
            void DoSomeWork();
        }

        private interface IOtherWorker
        {
            void DoSomeWork();

            void ResetWorkCount();
        }

        private class Worker : IWorker, IOtherWorker
        {
            public int WorkIterations { get; private set; }
            public int OtherWorkIterations { get; private set; }

            public void DoSomeWork()
            {
                WorkIterations++;
            }

            void IOtherWorker.DoSomeWork()
            {
                DoSomeWork();
                DoSomeWork();
                OtherWorkIterations++;
            }

            void IOtherWorker.ResetWorkCount()
            {
                OtherWorkIterations = 0;
            }
        }

        #endregion

        #region Basics

        [Fact]
        public void BeAssignableFromImplementingTypes()
        {
            var worker = new Worker();

            Assert.Equal(0, worker.WorkIterations);

            // this is illegal
            // var iWorker = new IWorker();

            IWorker iWorker = worker;

            iWorker.DoSomeWork();

            Assert.Equal(1, worker.WorkIterations);
        }

        #endregion

        #region Explicit Implementation

        [Fact]
        public void ProvideAccessToExplicitlyImplementedMembers()
        {
            var worker = new Worker();

            Assert.Equal(0, worker.WorkIterations);
            Assert.Equal(0, worker.OtherWorkIterations);

            // call IWorker's DoSomeWork method
            worker.DoSomeWork();

            Assert.Equal(1, worker.WorkIterations);

            IOtherWorker iOtherWorker = worker;

            // call IOtherWorker's DoSomeWork method
            iOtherWorker.DoSomeWork();

            Assert.Equal(3, worker.WorkIterations);
            Assert.Equal(1, worker.OtherWorkIterations);

            // illegal
            //worker.ResetWorkCount();

            // reset explicit work count
            iOtherWorker.ResetWorkCount();

            Assert.Equal(3, worker.WorkIterations);
            Assert.Equal(0, worker.OtherWorkIterations);
        }

        #endregion
    }
}
