using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace TypeTests
{
    public class AbstractClassesShould
    {
        #region Helpers

        private abstract class WorkerBase
        {
            public int WorkIterations { get; protected set; }
            public abstract void DoSomeWork();
        }

        private class Worker : WorkerBase
        {          
            public override void DoSomeWork()
            {
                WorkIterations++;
            }
        }

        #endregion

        [Fact]
        public void BeAssignableFromDerivedTypes()
        {
            var worker = new Worker();

            Assert.Equal(0, worker.WorkIterations);

            WorkerBase workerBase = worker;

            workerBase.DoSomeWork();

            Assert.Equal(1, workerBase.WorkIterations);
        }
    }
}
