using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ThreadBasedTaskTests
{
    public class ContinueWithShould
    {
        [Fact]
        public async Task ReturnImmediately()
        {
            var counter = 0;

            var t = Task.Factory.StartNew(() =>
            {
                Task.Delay(100).Wait();
                return ++counter;
            });

            var continuationTask = t.ContinueWith((continueFromTask) =>
            {
                Task.Delay(100).Wait();
                return continueFromTask.Result + 1;
            });

            counter = 100;

            Assert.Equal(100, counter);
            Assert.Equal(101, await t);
            Assert.Equal(102, await continuationTask);
            Assert.Equal(101, counter);
        }

        [Theory]
        [InlineData(TaskStatus.RanToCompletion)]
        [InlineData(TaskStatus.Faulted)]
        [InlineData(TaskStatus.Canceled)]
        public async Task RespectSuppliedContinuationOptions(TaskStatus baseTaskFinalStatus)
        {
            using (var cts = new CancellationTokenSource())
            {
                Task baseTask;
                void LocalFunction() { ; }

                TaskStatus BaseStatusToRanOrCanceled(Func<TaskStatus, bool> statusChecker)
                {
                    // continuations will go to RanToCompletion if executed or Canceled if not
                    return statusChecker(baseTaskFinalStatus) ? TaskStatus.RanToCompletion : TaskStatus.Canceled;
                }

                // setup base task according to supplied parameter
                switch (baseTaskFinalStatus)
                {
                    case TaskStatus.Faulted:
                        baseTask = Task.FromException(new Exception());
                        break;
                    case TaskStatus.Canceled:
                        cts.Cancel();
                        baseTask = Task.FromCanceled(cts.Token);
                        break;
                    default:
                        baseTask = Task.CompletedTask;
                        break;
                }

                var onlyOnRanToCompletionTask = baseTask.ContinueWith(t => { LocalFunction(); }, TaskContinuationOptions.OnlyOnRanToCompletion);
                var onlyOnFaultedTask = baseTask.ContinueWith(t => { LocalFunction(); }, TaskContinuationOptions.OnlyOnFaulted);
                var onlyOnCanceledTask = baseTask.ContinueWith(t => { LocalFunction(); }, TaskContinuationOptions.OnlyOnCanceled);
                var notOnFaultedTask = baseTask.ContinueWith(t => { LocalFunction(); }, TaskContinuationOptions.NotOnFaulted);
                var notOnCanceledTask = baseTask.ContinueWith(t => { LocalFunction(); }, TaskContinuationOptions.NotOnCanceled);
                var notOnRanToCompletionTask = baseTask.ContinueWith(t => { LocalFunction(); }, TaskContinuationOptions.NotOnRanToCompletion);

                // wait 1/10 second for all continuations to complete
                await Task.Delay(100);

                // make sure all continuations entered expected states
                Assert.Equal(BaseStatusToRanOrCanceled(s => s == TaskStatus.RanToCompletion), onlyOnRanToCompletionTask.Status);
                Assert.Equal(BaseStatusToRanOrCanceled(s => s == TaskStatus.Faulted), onlyOnFaultedTask.Status);
                Assert.Equal(BaseStatusToRanOrCanceled(s => s == TaskStatus.Canceled), onlyOnCanceledTask.Status);
                Assert.Equal(BaseStatusToRanOrCanceled(s => s != TaskStatus.Faulted), notOnFaultedTask.Status);
                Assert.Equal(BaseStatusToRanOrCanceled(s => s != TaskStatus.Canceled), notOnCanceledTask.Status);
                Assert.Equal(BaseStatusToRanOrCanceled(s => s != TaskStatus.RanToCompletion), notOnRanToCompletionTask.Status);
            }
        }
    }
}
