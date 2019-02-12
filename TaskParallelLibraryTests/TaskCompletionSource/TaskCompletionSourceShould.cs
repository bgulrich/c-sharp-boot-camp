using System;
using System.Threading.Tasks;
using Xunit;

namespace TPL.TaskCompletionSourceTests
{
    public class TaskCompletionSourceShould
    {
        #region States
        [Fact]
        public void HaveAnInitialStatusOfWaitingForActivation()
        {
            var tcs = new TaskCompletionSource<object>();
            Assert.Equal(TaskStatus.WaitingForActivation, tcs.Task.Status);
        }

        [Fact]
        public void EnterRanToCompletionStateWhenResultIsSet()
        {
            var tcs = new TaskCompletionSource<object>();
            tcs.SetResult(null);
            Assert.Equal(TaskStatus.RanToCompletion, tcs.Task.Status);
        }

        [Fact]
        public void EnterFaultedStateWhenExceptionIsSet()
        {
            var tcs = new TaskCompletionSource<object>();
            tcs.SetException(new Exception());
            Assert.Equal(TaskStatus.Faulted, tcs.Task.Status);
        }

        [Fact]
        public void EnterCanceledStateWhenCanceledIsSet()
        {
            var tcs = new TaskCompletionSource<object>();
            tcs.SetCanceled();
            Assert.Equal(TaskStatus.Canceled, tcs.Task.Status);
        }

        #endregion

        #region Exceptions
        [Fact]
        public void WrapExceptionInAggregateException()
        {
            const string message = "test exception";
            var tcs = new TaskCompletionSource<object>();
            tcs.SetException(new Exception(message));
            var aggException = Assert.IsAssignableFrom<AggregateException>(tcs.Task.Exception);
            Assert.Equal(message, aggException.InnerException.Message);
        }

        [Fact]
        public void WrapExceptionsInAggregateException()
        {
            const string message1 = "test message 1", message2 = "test message 2";
            var tcs = new TaskCompletionSource<object>();
            tcs.SetException(new Exception[]{new Exception(message1), new Exception(message2)});
            var aggException = Assert.IsAssignableFrom<AggregateException>(tcs.Task.Exception);
            Assert.Contains(aggException.InnerExceptions, e => e.Message == message1);
            Assert.Contains(aggException.InnerExceptions, e => e.Message == message2);
        }
        #endregion

        #region One-time use
        [Fact]
        public void FailToSetResultAfterCompletion()
        {
            var tcs = new TaskCompletionSource<object>();
            tcs.SetResult(null);
            Assert.False(tcs.TrySetResult(null));
        }

        [Fact]
        public void FailToSetCanceledAfterCompletion()
        {
            var tcs = new TaskCompletionSource<object>();
            tcs.SetResult(null);
            Assert.False(tcs.TrySetCanceled());
        }

        [Fact]
        public void FailToSetExceptionAfterCompletion()
        {
            var tcs = new TaskCompletionSource<object>();
            tcs.SetResult(null);
            Assert.False(tcs.TrySetException(new Exception()));
        }
        #endregion
    }
}
