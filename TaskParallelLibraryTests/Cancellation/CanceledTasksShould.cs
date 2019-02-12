using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Xunit;

namespace TPL.CancellationTests
{
    public class CanceledTasksShould
    {
        public interface ITaskInterface
        {
            Task<bool> SomeMethod();
        }

        #region Cancellation Optional/Opt-in

        [Fact]
        public async Task RunToCompletionIfOperationHasNotOptedIn()
        {
            using (var cts = new CancellationTokenSource())
            {
                // no cancellation support within body
                // accessing cts/calling cancellation from body is unusual, but an easy way ensure cancellation occurs after operation begins
                var t = Task.Factory.StartNew(() => { for(int i = 0; i < 100; ++i){ cts.Cancel(); } }, cts.Token);
                await t;

                Assert.Equal(TaskStatus.RanToCompletion, t.Status);
            }
        }

        [Fact]
        public async Task RunToCompletionIfCanceledAfterPointOfNoReturn()
        {
            using (var cts = new CancellationTokenSource())
            {
                // try to cancel after point of no return
                // accessing cts/calling cancellation from body is unusual, but an easy way ensure cancellation occurs after point of no return
                var t = Task.Factory.StartNew(() =>
                {
                    cts.Token.ThrowIfCancellationRequested();
                    cts.Cancel();
                }, cts.Token);

                // give the cancellation some time to occur
                await t;
                Assert.Equal(TaskStatus.RanToCompletion, t.Status);
            }
        }
        #endregion

        #region Exceptions
        [Fact]
        public void HaveNoExceptionWhenPutInCanceledState()
        {
            using (var cts = new CancellationTokenSource())
            {
                var t = new Task(() => { cts.Cancel(); ; cts.Token.ThrowIfCancellationRequested(); }, cts.Token);

                t.RunSynchronously();

                // throws task canceled exception wrapped in aggregate exception
                Assert.Null(t.Exception);
            }
        }

        [Fact]
        public void ThrowWrappedTaskCanceledExceptionWhenWaited()
        {
            using (var cts = new CancellationTokenSource())
            {
                // initiate cancellation
                cts.Cancel();

                var t = Task.Factory.StartNew(() => { cts.Cancel();cts.Token.ThrowIfCancellationRequested(); }, cts.Token);

                // throws task canceled exception wrapped in aggregate exception
                var aggException = Assert.Throws<AggregateException>(() => t.Wait());
                Assert.True(aggException.InnerExceptions.Single() is TaskCanceledException);
            }
        }

        [Fact]
        public void ThrowWrappedTaskCanceledExceptionWhenResultAccessed()
        {
            using (var cts = new CancellationTokenSource())
            {
                // initiate cancellation
                cts.Cancel();

                // note: no cancellation support internally
                var t = Task.Factory.StartNew(() => { cts.Token.ThrowIfCancellationRequested(); return true; }, cts.Token);

                // throws task canceled exception wrapped in aggregate exception
                var aggException = Assert.Throws<AggregateException>(() => t.Result);
                Assert.True(aggException.InnerExceptions.Single() is TaskCanceledException);
            }
        }

        [Fact]
        public async Task ThrowTaskCanceledExceptionWhenAwaited()
        {
            using (var cts = new CancellationTokenSource())
            {
                // initiate cancellation
                cts.Cancel();

                // note: no cancellation support internally
                var t = Task.Factory.StartNew(() => { cts.Token.ThrowIfCancellationRequested(); }, cts.Token);

                await Assert.ThrowsAsync<TaskCanceledException>(async () => { await t; });
            }
        }
        #endregion

        #region Cancellation and Task Status
        /// <summary>
        /// Demonstrates the TPL cancelling a task before the body runs 
        /// </summary>
        /// <returns></returns>
        [Fact]
        public void EnterCanceledStateImmediatelyIfLongRunningWorkAlreadyCanceled()
        {
            var mockTaskInterface = new Mock<ITaskInterface>();
            mockTaskInterface.Setup(m => m.SomeMethod()).ReturnsAsync(true);

            using (var cts = new CancellationTokenSource())
            {
                // initiate cancellation
                cts.Cancel();

                // note: no cancellation support internally
                var t = new Task(() => { mockTaskInterface.Object.SomeMethod(); }, cts.Token);

                // Canceled state
                Assert.Equal(TaskStatus.Canceled, t.Status);
                // Never called method
                mockTaskInterface.Verify(m => m.SomeMethod(), Times.Never());
            }
        }

        [Fact]
        public void EnterCanceledStateWhenMatchingCancellationTokenProvided()
        {
            using (var cts = new CancellationTokenSource())
            {
                // accessing cts/calling cancellation from body is unusual, but an easy way ensure cancellation occurs after operation begins
                var t = new Task(() => { cts.Cancel(); throw new OperationCanceledException(cts.Token); }, cts.Token);

                t.RunSynchronously();

                Assert.Equal(TaskStatus.Canceled, t.Status);
            }
        }

        [Fact]
        public void EnterCanceledStateWhenThrowIfCancellationRequestedAndMatchingTokenProvided()
        {
            using (var cts = new CancellationTokenSource())
            {
                // accessing cts/calling cancellation from body is unusual, but an easy way ensure cancellation occurs after operation begins
                var t = new Task(() => { cts.Cancel(); cts.Token.ThrowIfCancellationRequested(); }, cts.Token);

                // give the cancellation some time to occur
                t.RunSynchronously();

                Assert.Equal(TaskStatus.Canceled, t.Status);
            }
        }

        [Fact]
        public void EnterFaultedStateWhenMatchingCancellationTokenProvidedButCancellationNotRequested()
        {
            using (var cts = new CancellationTokenSource())
            {
                // don't request cancellation before throwing exception
                var t = new Task(() => { throw new OperationCanceledException(cts.Token); }, cts.Token);

                // give the cancellation some time to occur
                t.RunSynchronously();

                Assert.Equal(TaskStatus.Faulted, t.Status);
            }
        }

        [Fact]
        public void EnterFaultedStateWhenNoCancellationTokenProvidedToTask()
        {
            using (var cts = new CancellationTokenSource())
            {
                // initiate cancellation
                cts.Cancel();

                var t = new Task(() => { throw new OperationCanceledException(cts.Token); });

                t.RunSynchronously();

                Assert.Equal(TaskStatus.Faulted, t.Status);
            }
        }

        [Fact]
        public async Task EnterFaultedStateWhenThrowIfCancellationRequestedAndAlreadyCanceled()
        {
            using (var cts = new CancellationTokenSource())
            {
                // initiate cancellation
                cts.Cancel();

                var t = new Task(() => { cts.Token.ThrowIfCancellationRequested(); });

                // give the cancellation some time to occur
                t.RunSynchronously();

                Assert.Equal(TaskStatus.Faulted, t.Status);
            }
        }

        [Fact]
        public async Task EnterFaultedStateWhenCancellationTokensDoNotMatch()
        {
            // use two token sources and utilize one in each of task and body (mismatch)
            using (var cts1 = new CancellationTokenSource())
            {
                using (var cts2 = new CancellationTokenSource())
                {
                    var t = Task.Factory.StartNew(() => { throw new OperationCanceledException(cts1.Token); }, cts2.Token);

                    // initiate cancellation
                    cts1.Cancel();

                    // give the cancellation some time to occur
                    await Task.Delay(10);

                    Assert.Equal(TaskStatus.Faulted, t.Status);
                }
            }
        }

        #endregion
    }
}
