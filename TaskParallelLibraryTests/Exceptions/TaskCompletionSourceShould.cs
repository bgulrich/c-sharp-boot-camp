using System;
using System.Threading.Tasks;
using Xunit;

namespace TPL.ExceptionTests
{
    public class TaskCompletionSourceShould
    {
        [Fact]
        public void EnterFaultedStatusWhenExceptionSet()
        {
            var tcs = new TaskCompletionSource<bool>();

            var exception = new Exception("Some Exception");

            tcs.SetException(exception);

            Assert.Equal(TaskStatus.Faulted, tcs.Task.Status);
        }

        [Fact]
        public void ThrowWrappingAggregagteExceptionWhenWaited()
        {
            var tcs = new TaskCompletionSource<bool>();

            var exception = new Exception("Some Exception");

            tcs.SetException(exception);

            var aggException = Assert.Throws<AggregateException>(() => tcs.Task.Wait());
            Assert.Equal(exception, aggException.InnerException);
        }

        [Fact]
        public void ThrowWrappingAggregagteExceptionWhenResultAccessed()
        {
            var tcs = new TaskCompletionSource<bool>();

            var exception = new Exception("Some Exception");

            tcs.SetException(exception);

            var thrownException = Assert.Throws<AggregateException>(() => { return tcs.Task.Result; });
            Assert.Equal(exception, thrownException.InnerException);
        }

        [Fact]
        public async Task RethrowExceptionWhenAwaited()
        {
            var tcs = new TaskCompletionSource<bool>();

            var exception = new Exception("Some Exception");

            tcs.SetException(exception);

            var thrownException = await Assert.ThrowsAsync<Exception>(() => tcs.Task);
            Assert.Equal(exception, thrownException);
        }
    }
}
