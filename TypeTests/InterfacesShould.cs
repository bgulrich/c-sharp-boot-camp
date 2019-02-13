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

        private interface IExplicitWorker
        {
            void DoSomeWork();

            void ResetWorkCount();
        }

        private class Worker : IWorker, IExplicitWorker
        {
            public int WorkIterations { get; private set; }
            public int OtherWorkIterations { get; private set; }

            public void DoSomeWork()
            {
                WorkIterations++;
            }

            void IExplicitWorker.DoSomeWork()
            {
                DoSomeWork();
                DoSomeWork();
                OtherWorkIterations++;
            }

            void IExplicitWorker.ResetWorkCount()
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

            IExplicitWorker iExplicitWorker = worker;

            // call IExplicitWorker's DoSomeWork method
            iExplicitWorker.DoSomeWork();

            Assert.Equal(3, worker.WorkIterations);
            Assert.Equal(1, worker.OtherWorkIterations);

            // illegal
            //worker.ResetWorkCount();

            // reset explicit work count
            iExplicitWorker.ResetWorkCount();

            Assert.Equal(3, worker.WorkIterations);
            Assert.Equal(0, worker.OtherWorkIterations);
        }

        #endregion
    }
}
