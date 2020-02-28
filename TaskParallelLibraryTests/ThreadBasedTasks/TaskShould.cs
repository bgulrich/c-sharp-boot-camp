using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace TPLTests.ThreadBasedTasks
{
    public class TaskShould
    {
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task ProgressThroughStatesOfExecution(bool withError)
        {
            var exception = new Exception("test");

            var t = new Task<int>(() =>
            {
                var counter = 0;

                for (var i = 0; i < 20; ++i)
                {
                    Task.Delay(100).Wait();
                    ++counter;
                }

                if (withError)
                    throw exception;

                return counter;
            });

            Assert.Equal(TaskStatus.Created, t.Status);

            t.Start();

            Assert.Equal(TaskStatus.WaitingToRun, t.Status);

            while (t.Status == TaskStatus.WaitingToRun)
            {
                await Task.Delay(100);
            }

            Assert.Equal(TaskStatus.Running, t.Status);

            while (t.Status == TaskStatus.Running)
            {
                await Task.Delay(100);
            }

            if (withError)
            {
                Assert.Equal(TaskStatus.Faulted, t.Status);
            }
            else
            {
                Assert.Equal(TaskStatus.RanToCompletion, t.Status);
            }
        }

        [Fact]
        public async Task ReturnResultIfExecutionCompletedSuccessfullyWhenAccessingResultProperty()
        {
            var tcs = new TaskCompletionSource<int>();

            tcs.SetResult(5);

            Assert.Equal(5, tcs.Task.Result);
        }

        [Fact]
        public async Task ThrowWrappingAggregateExceptionIfAnExceptionThrownDuringExecutionWhenAccessingResultProperty()
        {
            var ex = new Exception("test");
            var tcs = new TaskCompletionSource<int>();

            tcs.SetException(ex);

            var thrownException  = Assert.Throws<AggregateException>(() => tcs.Task.Result);
            Assert.Same(ex, thrownException.InnerException);
        }
    }
}
