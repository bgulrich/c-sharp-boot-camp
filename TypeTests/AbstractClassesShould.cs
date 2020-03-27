using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace TypeTests
{
    public class AbstractClassesShould
    {
        #region Types

        private abstract class WorkerBase
        {
            public int WorkIterations { get; protected set; }
            public abstract void DoSomeWork();
            public abstract float TimeInSeconds { get; set; }
        }

        private class Worker : WorkerBase
        {
            public override void DoSomeWork()
            {
                WorkIterations++;
            }

            public override float TimeInSeconds { get; set; }
        }

        private class Manager : WorkerBase
        {
            public override float TimeInSeconds { get; set; }

            public override void DoSomeWork()
            {
                --WorkIterations;
            }
        }

        #endregion

        [Fact]
        public void NotBeInstantiatable()
        {
            // I can't do this
            //var wb = new WorkerBase();
        }

        [Fact]
        public void BeAssignableFromDerivedTypes()
        {
            var worker = new Manager();

            Assert.Equal(0, worker.WorkIterations);

            WorkerBase workerBase = worker;

            workerBase.DoSomeWork();

            Assert.Equal(-1, workerBase.WorkIterations);
        }
    }
}
